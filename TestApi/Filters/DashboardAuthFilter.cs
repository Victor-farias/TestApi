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
        private readonly IAuthorizationService _authorizationService;

        public DashboardAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("HangfireConnection"));
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            _authorizationService = new AuthorizationService(new UserRepository(dbContext));
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var userName = "Victor_Farias";

            return _authorizationService.HasAdminRole(userName);
        }
    }
}
