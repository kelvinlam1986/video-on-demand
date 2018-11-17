using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoOnDemand.UI.Services;
using VideoOnDemand.Data.Data;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Repositories;
using VideoOnDemand.UI.Models.DTOModels;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.UI
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
            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<VODContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IReadRepository, SqlReadRepository>();
            services.AddTransient<IDbReadService, DbReadService>();
            
            services.AddMvc();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>();
                cfg.CreateMap<Download, DownloadDTO>()
                    .ForMember(des => des.DownloadTitle, src => src.MapFrom(s => s.Title))
                    .ForMember(des => des.DownloadUrl, src => src.MapFrom(s => s.Url));
                cfg.CreateMap<Instructor, InstructorDTO>()
                    .ForMember(des => des.InstructorName, src => src.MapFrom(s => s.Name))
                    .ForMember(des => des.InstructorDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(des => des.InstructorAvatar, src => src.MapFrom(s => s.Thumbnail));
                cfg.CreateMap<Course, CourseDTO>()
                    .ForMember(des => des.CourseId, src => src.MapFrom(s => s.Id))
                    .ForMember(des => des.CourseTitle, src => src.MapFrom(s => s.Title))
                    .ForMember(des => des.CourseDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(des => des.MarqueeImageUrl, src => src.MapFrom(s => s.MarqueeImageUrl))
                    .ForMember(des => des.CourseImageUrl, src => src.MapFrom(s => s.ImageUrl));
                cfg.CreateMap<Module, ModuleDTO>()
                    .ForMember(des => des.ModuleTitle, src => src.MapFrom(s => s.Title));
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
