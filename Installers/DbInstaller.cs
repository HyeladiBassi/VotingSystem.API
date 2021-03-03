using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VotingSystem.API.DataAccess;
using VotingSystem.API.Models;
using VotingSystem.API.Services;
using System;

namespace VotingSystem.API.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            
            var connectString = Configuration["connectionString:VotingConnect"];
            services.AddDbContext<VotingContext>(options => options.UseSqlite(connectString));

            services.AddDbContext<VotingContext>(
                item => item.UseSqlite(connectString, b => b.MigrationsAssembly("VotingSystem.API")))
                .AddDefaultIdentity<SystemUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                }).AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<VotingContext>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IVotingService, VotingService>();
            services.AddTransient<SystemRole>();
        }
    }
}