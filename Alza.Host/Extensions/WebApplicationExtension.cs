﻿using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Alza.Host.Extensions;

public static class WebApplicationExtension
{
    public static IApplicationBuilder AddOpenApi(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                var baseUrl = $"https://{httpReq.Host.Value}";
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{baseUrl}/inventory" } };
            });
        });
        app.UseSwaggerUI(options =>
        {
            var descriptions = ((WebApplication)app).DescribeApiVersions();
            var groupNames = descriptions.Select(x => x.GroupName);

            // Build a swagger endpoint for each discovered API version
            foreach (var description in groupNames)
            {
                var url = $"/inventory/swagger/{description}/swagger.json";
                var name = description.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
        return app;
    }
}
