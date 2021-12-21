using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestApi.Data;
using TestApi.Models;
using TestApi.Services.Interfaces;

namespace TestApi.Auth
{
    public class DashboardAuthFilter : IDashboardAuthorizationFilter
    {
        private IAuthorizationService authorizationService;

        public DashboardAuthFilter(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = "Victor_Farias";

            return this.authorizationService.HasAdminRole(httpContext);
        }
    }
}
