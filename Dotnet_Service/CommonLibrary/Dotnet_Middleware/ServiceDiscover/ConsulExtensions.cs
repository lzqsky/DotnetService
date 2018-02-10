using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Dotnet_Middleware.ServiceDiscover
{
    public static class ConsulExtensions
    {
        /// <summary>
        /// Consul:ip               服务地址
        /// Consul:port             服务端口
        /// Consul:regist_add       请求注册的 Consul 地址
        /// Consul:check_add        健康检查地址
        /// Consul:name             服务名称
        /// Consul:tag              服务标签
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, IConfiguration configuration)
        {
            var consul = configuration["Consul:ip"];
            if (consul == null)
                return app;

            string ip = configuration["Consul:ip"];
            int port = Convert.ToInt32(configuration["Consul:port"]);
            var consulClient = new ConsulClient(x => x.Address = new Uri(configuration["Consul:regist_add"]));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{ip}:{port}/{configuration["Consul:check_add"]}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = configuration["Consul:name"],
                Address = ip,
                Port = port,
                Tags = new[] { configuration["Consul:tag"] }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
            return app;
        }
    }
}
