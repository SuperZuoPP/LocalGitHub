using AutoMapper;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPFBase.Api.Context;
using WPFBase.Api.Context.Model;
using WPFBase.Api.Context.Model.BM;
using WPFBase.Api.Context.Model.SM;
using WPFBase.Api.Context.Repository;
using WPFBase.Api.Context.UnitOfWork;
using WPFBase.Api.Extensions;
using WPFBase.Api.Services.BM;

namespace WPFBase.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            XmlConfigurator.Configure(new FileInfo("log4net.config")); // 从 log4net.config 读取配置


        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<BaseContext>(options =>
            {

                //使用sqlserver
                var connectionString = Configuration.GetConnectionString("SqlServerConnection1");
                options.UseSqlServer(connectionString);

                //使用sqlite
                //var connectionString = Configuration.GetConnectionString("SqliteConnection");
                //options.UseSqlite(connectionString);
            }).AddUnitOfWork<BaseContext>()
           .AddCustomRepository<ToDo, ToDoRepository>()
           .AddCustomRepository<Memo, MemoRepository>()
           .AddCustomRepository<User, UserRepository>()
           .AddCustomRepository<Operator, OperatorRepository>()
           .AddCustomRepository<TbWeighOperator, TbWeighOperatorRepository>()
           .AddCustomRepository<TbWeighUsergroup, TbWeighUsergroupRepository>()
           .AddCustomRepository<TbWeighGroupauthorityuser, TbWeighGroupauthorityuserRepository>()
           .AddCustomRepository<TbWeighMenu, TbWeighMenuRepository>()
           .AddCustomRepository<TbWeighDatalineinfo, TbWeighDatalineinfoRepository>()
           .AddCustomRepository<TbWeighPlan, TbWeighPlanRepository>()
           .AddCustomRepository<TbWeighLittleplan, TbWeighLittleplanRepository>();

            services.AddTransient<IToDoService, ToDoService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<ITbWeighOperatorService, TbWeighOperatorService>();
            services.AddTransient<ITbWeighUsergroupService, TbWeighUsergroupService>();
            services.AddTransient<IMenuService, MenuService>(); 
            services.AddTransient<ITbWeighDatalineinfoService, TbWeighDatalineinfoService>();
            services.AddTransient<ITbWeighWeighbridgeofficeService, TbWeighWeighbridgeofficeService>();
            services.AddTransient<ITbWeighVideoService, TbWeighVideoService>();

            //添加AutoMapper
            var atuomapperConfig = new MapperConfiguration(config => {
                config.AddProfile(new AutoMapperProFile());
            });

            services.AddSingleton(atuomapperConfig.CreateMapper());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WPFBase.Api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WPFBase.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
