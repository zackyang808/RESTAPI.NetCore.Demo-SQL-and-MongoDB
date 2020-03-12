using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RESTAPI.NetCore.Demo.Common.Contracts;
using RESTAPI.NetCore.Demo.Common.MongoDB;
using RESTAPI.NetCore.Demo.Common.Services;
using RESTAPI.NetCore.Demo.Web.Domain.Contracts;
using RESTAPI.NetCore.Demo.Web.Domain.Services;

namespace RESTAPI.NetCore.Demo.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpContextAccessor();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();

            //services.AddScoped(typeof(IRepository<>), typeof(Common.MSSQL.Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Common.MongoDB.Repository<>));

            //services.AddDbContext<Common.MSSQL.DataContext>(option => option.UseSqlServer(Configuration.GetConnectionString("SQLConStr")));
            services.AddMongoDB(Configuration);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
