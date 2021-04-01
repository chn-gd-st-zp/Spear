using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using AutoMapper;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.AppEntrance
{
    public delegate void LoadRunningSettingsDelegate(IConfigurationBuilder configBuilder, List<string> argList);

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
                bool isTest = EEnvironment == Enum_Environment.Production ? false : true;

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
        /// 根据文件名、文件类型，获取完整路径
        /// </summary>
        /// <param name="filelNames">文件名</param>
        /// <param name="eInitFile">文件类型</param>
        /// <param name="defaultPattern">默认匹配公式</param>
        /// <returns></returns>
        public static List<string> GetPaths(this List<string> filelNames, Enum_InitFile eInitFile, string defaultPattern = null)
        {
            string suffix = "." + eInitFile.ToString().ToLower();

            var dllPaths = new List<string>();

            if ((filelNames == null || filelNames.Count() == 0) && defaultPattern.IsEmptyString())
                return dllPaths;

            if (filelNames == null || filelNames.Count() == 0)
            {
                dllPaths = Directory.GetFiles(RootPath, defaultPattern + suffix).ToList();
            }
            else
            {
                filelNames.ForEach(o =>
                {
                    o = o.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) ? o : o + suffix;
                    dllPaths.AddRange(Directory.GetFiles(RootPath, o).ToList());
                });
            }

            return dllPaths;
        }

        /// <summary>
        /// 获取所有类库
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAllAssemblies()
        {
            var returnAssemblies = new List<Assembly>();
            var loadedAssemblies = new Dictionary<string, Assembly>();
            var assembliesToCheck = new Queue<Assembly>();

            assembliesToCheck.Enqueue(Assembly.GetEntryAssembly());

            while (assembliesToCheck.Any())
            {
                var assemblyToCheck = assembliesToCheck.Dequeue();

                foreach (var reference in assemblyToCheck.GetReferencedAssemblies())
                {
                    if (!loadedAssemblies.ContainsKey(reference.FullName))
                    {
                        var assembly = Assembly.Load(reference);
                        assembliesToCheck.Enqueue(assembly);
                        loadedAssemblies.Add(reference.FullName, assembly);
                        returnAssemblies.Add(assembly);
                    }
                }
            }

            return returnAssemblies.OrderByDescending(o => o.FullName).ToList();
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <param name="diPattern"></param>
        /// <returns></returns>
        public static List<Type> GetAllType()
        {
            List<Type> result = new List<Type>();

            GetAllAssemblies()
                .ForEach(assembly =>
                {
                    var types = assembly.GetTypes();
                    result.AddRange(types);
                });

            return result;
        }

        /// <summary>
        /// 获取运行类库
        /// </summary>
        /// <param name="diPattern"></param>
        /// <returns></returns>
        public static List<Assembly> GetRunningAssemblies()
        {
            IQueryable<Assembly> assembliesQuery = null;

            assembliesQuery = AppDomain.CurrentDomain.GetAssemblies().AsQueryable();
            //assembliesQuery = Assembly.GetEntryAssembly().GetReferencedAssemblies().AsQueryable();

            var result = assembliesQuery.OrderByDescending(o => o.FullName).ToList();
            return result;
        }

        /// <summary>
        /// 获取运行类型
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetRunningType()
        {
            List<Type> result = new List<Type>();

            GetRunningAssemblies()
                .ForEach(assembly =>
                {
                    var types = assembly.GetTypes();
                    result.AddRange(types);
                });

            return result;
        }

        /// <summary>
        /// 获取运行类型
        /// </summary>
        /// <param name="diPattern"></param>
        /// <returns></returns>
        public static List<Type> GetRunningType(this string diPattern)
        {
            List<Type> result = new List<Type>();

            GetRunningAssemblies()
                .Where(oo => oo.GetName().Name.StartsWith(diPattern.Replace("*", "")))
                .OrderBy(o => o.FullName)
                .ToList()
                .ForEach(assembly =>
                {
                    var types = assembly.GetTypes();
                    result.AddRange(types);
                });

            return result;
        }

        /// <summary>
        /// 获取运行类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="startup"></param>
        /// <returns></returns>
        public static List<Type> GetRunningType<T>(this StartupBasic<T> startup) where T : AppSettingsBasic
        {
            List<Type> result = new List<Type>();

            var runningAssemblies = GetRunningAssemblies();

            if (startup.CurConfig.AutoFacSettings.Dlls == null || startup.CurConfig.AutoFacSettings.Dlls.Count() == 0)
            {
                result.AddRange(startup.CurConfig.AutoFacSettings.DefaultPattern.GetRunningType());
            }
            else
            {
                runningAssemblies
                    .Where(o => startup.CurConfig.AutoFacSettings.Dlls.Contains(o.GetName().Name))
                    .OrderBy(o => o.FullName)
                    .ToList()
                    .ForEach(assembly =>
                    {
                        var types = assembly.GetTypes();
                        result.AddRange(types);
                    });
            }

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
                .AddJsonFile($"appsettings.json", true, false);

            if (!envName.IsEmptyString())
                configBuilder.AddJsonFile($"appsettings.{envName}.json", true, false);

            if (configFiles != null)
            {
                foreach (var file in configFiles)
                    configBuilder.AddJsonFile(file, true, false);
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
            List<string> argList = args != null && args.Length > 0 ? args.ToList() : new List<string>();
            Printor.PrintText("启动参数：");
            Printor.PrintText("{");
            argList.ForEach(o => { Printor.PrintText("  " + o); });
            Printor.PrintText("}");
            Printor.PrintLine();

            foreach (var func in funcArray)
                func(configBuilder, argList);

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

            GetRunningAssemblies()
                .ForEach(o =>
                {
                    var types = o.GetTypes().Where(o => o.IsClass && !o.IsAbstract && o.IsExtendType(typeof(Profile))).ToList();
                    allTypeInApp.AddRange(types);
                });

            if (allTypeInApp.Count() != 0)
                services.AddAutoMapper(allTypeInApp.ToArray());

            return services;
        }
    }
}
