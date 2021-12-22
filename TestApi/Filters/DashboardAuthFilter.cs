using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using TestApi.Data;
using TestApi.Enums;
using TestApi.Models;
using TestApi.Repositories;
using TestApi.Services;
using TestApi.Services.Interfaces;

namespace TestApi.Auth
{
    public class DashboardAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;

        public DashboardAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;

            this.optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("HangfireConnection"));
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            if (context.IsReadOnly)
                return true;

            var userName = "Victor_Farias";

            // Utilizing using statements to dispose non dependecy injected objects
            using (var dbContext = new ApplicationDbContext(optionsBuilder.Options))
            {
                using (var userRepository = new UserRepository(dbContext))
                {
                    using (var authorizationService = new AuthorizationService(userRepository))
                    {
                        return authorizationService.HasAdminRole(userName);
                    } 
                }
            }
        }
    }
}
