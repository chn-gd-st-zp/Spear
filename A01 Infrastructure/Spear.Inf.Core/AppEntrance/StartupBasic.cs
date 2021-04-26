using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Autofac;
using Newtonsoft.Json;

using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.AppEntrance
{
    public abstract class StartupBasic<T> where T : AppSettingsBasic
    {
        public StartupBasic(IConfiguration configuration)
        {
            Configuration = configuration;
            CurConfig = Configuration.GetSetting<T>();
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
        public T CurConfig { get; private set; }

        /// <summary>
        /// JSON设置
        /// </summary>
        protected JsonSerializerSettings JsonSerializerSettings { get; private set; }

        #region 基础方法

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Extend_ConfigureServices(services);
        }

        /// <summary>
        /// 配置容器
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            List<string> typeIgnore = new List<string>();
            List<string> typeRegis = new List<string>();

            var runningType = this.GetRunningType();

            containerBuilder.Register(runningType, Configuration, typeIgnore, typeRegis);
            containerBuilder.Register(runningType, typeIgnore, typeRegis);

            containerBuilder.Register(o => Configuration).As<IConfiguration>().SingleInstance();
            containerBuilder.Register(o => JsonSerializerSettings).AsSelf().SingleInstance();

            Extend_ConfigureContainer(containerBuilder);
        }

        /// <summary>
        /// 配置构造器
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            Extend_Configure(app, env, lifetime, loggerFactory);
        }

        #endregion

        #region 子类实现

        /// <summary>
        /// 设置序列化参数
        /// </summary>
        /// <returns></returns>
        protected abstract JsonSerializerSettings SetJsonSerializerSettings();

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
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        /// <param name="loggerFactory"></param>
        protected abstract void Extend_Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory);

        #endregion
    }
}
