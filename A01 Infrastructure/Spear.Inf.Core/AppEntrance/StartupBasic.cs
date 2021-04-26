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
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        public T CurConfig { get; private set; }

        /// <summary>
        /// JSON����
        /// </summary>
        protected JsonSerializerSettings JsonSerializerSettings { get; private set; }

        #region ��������

        /// <summary>
        /// ���÷���
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            Extend_ConfigureServices(services);
        }

        /// <summary>
        /// ��������
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
        /// ���ù�����
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

        #region ����ʵ��

        /// <summary>
        /// �������л�����
        /// </summary>
        /// <returns></returns>
        protected abstract JsonSerializerSettings SetJsonSerializerSettings();

        /// <summary>
        /// ��չ ConfigureServices ����
        /// </summary>
        /// <param name="services"></param>
        protected abstract void Extend_ConfigureServices(IServiceCollection services);

        /// <summary>
        /// ��չ ConfigureContainer
        /// </summary>
        /// <param name="containerBuilder"></param>
        protected abstract void Extend_ConfigureContainer(ContainerBuilder containerBuilder);

        /// <summary>
        /// ��չ Configure
        /// ��Web�̳е�
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        /// <param name="loggerFactory"></param>
        protected abstract void Extend_Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory);

        #endregion
    }
}
