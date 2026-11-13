using Microsoft.OpenApi;

namespace Agroflet.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;

public static class OpenApiConfiguration
{
    public static void Configure(OpenApiDocument document)
    {
        document.Info.Title = "AgroFlet Platform API";
        document.Info.Version = "v1";
        document.Info.Description = "Agricultural freight coordination in Peru. Sign in, then authorize requests with the returned JWT.";
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };
        document.Security = [new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        }];
        foreach (var path in document.Paths)
            if ((path.Key.StartsWith("/api/v1/authentication/") || path.Key.StartsWith("/health/")) && path.Value.Operations is not null)
                foreach (var operation in path.Value.Operations.Values) operation.Security = [];
    }
}
