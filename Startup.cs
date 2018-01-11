using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KTBDay.Hubs;
using KTBDay.Models;
using KTBDay.Mq;

namespace KTBDay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mqConfig = GetMqConfiguration();

            services.AddSignalR();
            services.AddMvc();

            services.AddSingleton(s => new Client(mqConfig));
            services.AddSingleton(s => new Receiver(s.GetService<Client>(), s.GetService<MessengerHub>()));
            services.AddScoped(s => new Sender(s.GetService<Client>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<MessengerHub>("messengerHub");
            });
        }

        private MqConnectionViewModel GetMqConfiguration()
        {
            return Configuration.GetSection("RabbitMq").Get<MqConnectionViewModel>();
        }
    }
}
