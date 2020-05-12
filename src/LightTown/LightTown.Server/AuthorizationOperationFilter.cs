using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LightTown.Server
{
    /// <summary>
    /// Authorization filter for swagger that shows a pad-lock and required permissions for endpoints that require authentication.
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        private readonly AppendAuthorizeToSummaryOperationFilter<AuthorizationAttribute> _filter;

        public AuthorizationOperationFilter()
        {
            var permissionsSelector = new PolicySelectorWithLabel<AuthorizationAttribute>
            {
                Label = "Permissions",
                Selector = authAttributes =>
                    authAttributes
                        .Where(a => a.Arguments?.Length > 0)
                        .Select(a => string.Join(",", a.Arguments.Select(e => e.ToString())))
            };

            _filter = new AppendAuthorizeToSummaryOperationFilter<AuthorizationAttribute>(new[]
            {
                permissionsSelector
            }.AsEnumerable());
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterDescriptor = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterDescriptor.Select(filterInfo => filterInfo.Filter)
                .Any(filterMetadata => filterMetadata is AuthorizationAttribute);
            var allowAnonymous = filterDescriptor.Select(filterInfo => filterInfo.Filter)
                .Any(filterMetadata => filterMetadata is IAllowAnonymousFilter);

            if (!isAuthorized || allowAnonymous)
            {
                return;
            }
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, Id = "Identity"
                            }
                        },
                        new string[0]
                    }
                }
            };

            _filter.Apply(operation, context);
        }
    }
}