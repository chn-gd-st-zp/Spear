using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

using Autofac;
using AutoMapper;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.Core.AppEntrance
{
    public delegate void LoadRunningSettingsDelegate(IConfigurationBuilder configBuilder, string[] args);

    /// <summary>
    /// 程序启动辅助
    /// </summary>
    public static class AppInitHelper
    {
        /// <summary>
        /// 环境变量 - 运行环境
        /// </summary>
        public static Enum_Environment EEnvironment
        {
            get
            {
                Enum_Environment eEnvironment = Enum_Environment.None;

                try
                {
                    eEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Convert2Enum<Enum_Environment>();
                }
                catch (Exception)
                {
                    throw new Exception("无效的环境变量[Environment]");
                }

                return eEnvironment;
            }
        }

        /// <summary>
        /// 环境变量 - 测试模式
        /// </summary>
        public static bool IsTestMode
        {
            get
            {
                bool isTest = EEnvironment == Enum_Environment.PRO || EEnvironment == Enum_Environment.Production ? false : true;

                try
                {
                    var value = Environment.GetEnvironmentVariable("ASPNETCORE_ISTESTMODE");
                    if (!value.IsEmptyString())
                        isTest = bool.Parse(value);
                }
                catch (Exception)
                {
                    throw new Exception("无效的环境变量[IsTestMode]");
                }

                return isTest;
            }
        }

        /// <summary>
        /// 程序根目录
        /// </summary>
        public static string RootPath
        {
            get
            {
                string rootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
                return rootPath.EndsWith("/") ? rootPath : rootPath + "/";
            }
        }

        /// <summary>
        /// 生成路径
        /// </summary>
        /// <param name="ePathMode"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GenericPath(Enum_PathMode ePathMode, string path)
        {
            var rootPath = ePathMode == Enum_PathMode.ABS ?
                path
                :
                (
                    RootPath +
                    (
                        path.StartsWith("/") ?
                        path.Substring(1)
                        :
                        path
                    )
                );

            return rootPath.EndsWith("/") ? rootPath : rootPath + "/";
        }

        /// <summary>
        /// 根据文件名、文件类型，获取完整路径
        /// </summary>
        /// <param name="eInitFile">文件类型</param>
        /// <param name="patterns">匹配公式</param>
        /// <param name="fileNames">文件名</param>
        /// <returns></returns>
        public static List<string> GetPaths(Enum_InitFile eInitFile, string[] patterns = null, string[] fileNames = null)
        {
            string suffix = "." + eInitFile.ToString().ToLower();

            var filePaths = new List<string>();

            if ((patterns == null || patterns.Length == 0) && (fileNames == null || fileNames.Length == 0))
                return filePaths;

            if (patterns == null || patterns.Length == 0)
            {
                Directory.GetFiles(RootPath)
                    .ToList()
                    .ForEach(o =>
                    {
                        var fileName = fileNames.Where(oo => o.EndsWith(oo + suffix, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                        if (!fileName.IsEmptyString())
                            filePaths.Add(o);
                    });
            }
            else
            {
                foreach (var pattern in patterns)
                    filePaths.AddRange(Directory.GetFiles(RootPath, pattern + suffix).ToList());
            }

            return filePaths;
        }

        /// <summary>
        /// 获取所有类库
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="dlls"></param>
        /// <returns></returns>
        public static List<Assembly> GetAllAssemblies(string[] patterns = null, string[] dlls = null)
        {
            List<Assembly> result = new List<Assembly>();

            if (patterns != null && patterns.Length > 0)
                patterns = patterns.Select(o => o.Replace("*", "")).ToArray();

            var assemblyNameList_source = DependencyContext.Default.RuntimeLibraries.Select(o => o.Name).ToList();
            var assemblyNameList_target = new List<string>();

            if (assemblyNameList_target.Count() == 0 && dlls != null)
            {
                foreach (var dll in dlls)
                {
                    var sourceName = assemblyNameList_source
                        .Where(sourceName => string.Compare(dll, sourceName, true) == 0)
                        .FirstOrDefault();

                    if (!sourceName.IsEmptyString() && !assemblyNameList_target.Any(targetName => string.Compare(targetName, sourceName, true) == 0))
                        assemblyNameList_target.Add(sourceName);
                }
            }

            if (assemblyNameList_target.Count() == 0 && patterns != null)
            {
                foreach (var pattern in patterns)
                {
                    var pat = pattern.Replace("*", "");
                    pat = pat.EndsWith(".") ? pat : pat + ".";

                    assemblyNameList_source
                        .Where(sourceName => sourceName.StartsWith(pat, StringComparison.OrdinalIgnoreCase))
                        .ToList()
                        .ForEach(sourceName =>
                        {
                            if (!assemblyNameList_target.Any(targetName => string.Compare(targetName, sourceName, true) == 0))
                                assemblyNameList_target.Add(sourceName);
                        });
                }
            }

            foreach (var assemblyName in assemblyNameList_target)
            {
                if (!File.Exists(RootPath + assemblyName + ".dll"))
                    continue;

                result.Add(Assembly.Load(new AssemblyName(assemblyName)));
            }

            return result;
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="dlls"></param>
        /// <returns></returns>
        public static List<Type> GetAllType(string[] patterns = null, string[] dlls = null)
        {
            List<Type> result = new List<Type>();

            GetAllAssemblies(patterns, dlls)
                .ForEach(assembly =>
                {
                    var types = assembly.GetTypes();
                    result.AddRange(types);
                });

            return result;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="configFiles"></param>
        /// <returns></returns>
        public static IConfigurationBuilder LoadConfiguration(this IConfigurationBuilder configBuilder, params string[] configFiles)
        {
            var envName = EEnvironment.ToString();
            Printor.PrintText("环境变量：" + envName);

            configBuilder
                .SetBasePath(RootPath)
                .AddJsonFile($"appsettings.json", true, true);

            if (EEnvironment != Enum_Environment.None)
                configBuilder.AddJsonFile($"appsettings.{envName.ToLower()}.json", true, true);

            if (configFiles != null)
            {
                foreach (var file in configFiles)
                    configBuilder.AddJsonFile(file, true, true);
            }

            return configBuilder;
        }

        /// <summary>
        /// 加载运行参数
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="args"></param>
        /// <param name="funcArray"></param>
        /// <returns></returns>
        public static IConfigurationBuilder LoadRunningSettings(this IConfigurationBuilder configBuilder, string[] args, params LoadRunningSettingsDelegate[] funcArray)
        {
            args = args != null && args.Length > 0 ? args : new string[0];

            Printor.PrintText("启动参数：");
            Printor.PrintText("{");
            args.ToList().ForEach(o => { Printor.PrintText("  " + o); });
            Printor.PrintText("}");
            Printor.PrintLine();

            foreach (var func in funcArray)
                func(configBuilder, args);

            return configBuilder;
        }

        /// <summary>
        /// 添加AutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            List<Type> allTypeInApp = new List<Type>();

            GetAllAssemblies()
            .ForEach(o =>
            {
                try
                {
                    var types = o.GetTypes().Where(o => o.IsClass && !o.IsAbstract && o.IsImplementedType<IAutoMapperProfile>()).ToList();
                    if (types.Count() > 0)
                        allTypeInApp.AddRange(types);
                }
                catch
                {
                    //
                }
            });

            if (allTypeInApp.Count() != 0)
                services.AddAutoMapper(allTypeInApp.ToArray());

            return services;
        }
    }
}
