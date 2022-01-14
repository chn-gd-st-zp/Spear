using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Newtonsoft.Json;

using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.Tool;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.Core.AppEntrance
{
    public abstract class StartupBase<TSettings, TConfigures>
        where TSettings : AppSettingsBase
        where TConfigures : AppConfiguresBase
    {
        public StartupBase(IConfiguration configuration)
        {
            Configuration = configuration;
            CurConfig = Configuration.GetSetting<TSettings>();
            JsonSerializerSettings = SetJsonSerializerSettings();

            Printor.PrintText(CurConfig.ToJson());
            Printor.PrintLine();
        }

        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 配置
        /// </summary>
        public TSettings CurConfig { get; private set; }

        /// <summary>
        /// JSON设置
        /// </summary>
        protected JsonSerializerSettings JsonSerializerSettings { get; private set; }

        /// <summary>
        /// 设置序列化参数
        /// </summary>
        /// <returns></returns>
        protected abstract JsonSerializerSettings SetJsonSerializerSettings();

        /// <summary>
        /// 配置容器
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            List<string> typeIgnore = new List<string>();
            List<string> typeRegis = new List<string>();

            var runningType = AppInitHelper.GetAllType(CurConfig.AutoFacSettings.Patterns, CurConfig.AutoFacSettings.Dlls.ToArray());

            containerBuilder.Register(runningType, Configuration, typeIgnore, typeRegis);
            containerBuilder.Register(runningType, typeIgnore, typeRegis);

            containerBuilder.Register(o => Configuration).As<IConfiguration>().SingleInstance();
            containerBuilder.Register(o => CurConfig).AsSelf().SingleInstance();
            containerBuilder.Register(o => JsonSerializerSettings).AsSelf().SingleInstance();

            Extend_ConfigureContainer(containerBuilder);
        }

        /// <summary>
        /// 配置程序
        /// </summary>
        /// <param name="configures"></param>
        protected void Configure(TConfigures configures)
        {
            ServiceContext.InitServiceProvider(configures.App.ApplicationServices);

            Extend_Configure(configures);
        }

        /// <summary>
        /// 拓展 ConfigureServices 方法
        /// </summary>
        /// <param name="services"></param>
        protected abstract void Extend_ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 拓展 ConfigureContainer
        /// </summary>
        /// <param name="containerBuilder"></param>
        protected abstract void Extend_ConfigureContainer(ContainerBuilder containerBuilder);

        /// <summary>
        /// 拓展 Configure
        /// 给Web继承的
        /// </summary>
        /// <param name="configures"></param>
        protected abstract void Extend_Configure(TConfigures configures);
    }
}
