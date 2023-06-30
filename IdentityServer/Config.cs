// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using ApiScope = Duende.IdentityServer.Models.ApiScope;
using Client = Duende.IdentityServer.Models.Client;
using IdentityResource = Duende.IdentityServer.Models.IdentityResource;
using Secret = Duende.IdentityServer.Models.Secret;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>
    {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
    };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("api1", "My Api")
        };
  
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // machine to machine client
            new Client()
            {
                ClientId = "m2m.client",
                ClientName = "Machine to Machine",
                Description = "Machine to Machine",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api1" }
            },

            // interactive ASP.NET Core MVC client
            new Client()
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },
                AllowOfflineAccess = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            },
            new Client()
            {
                ClientName = "Angular-Client",
                ClientId = "angular-client",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string>{ "http://localhost:4200/signin-callback", "http://localhost:4200/assets/silent-callback.html" },
                RequirePkce = true,
                AllowAccessTokensViaBrowser = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                },
                AllowedCorsOrigins = { "http://localhost:4200" },
                RequireClientSecret = false,
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                RequireConsent = false,
                AccessTokenLifetime = 600,
                AbsoluteRefreshTokenLifetime = 10000,
            },

            new Client()
            {
                ClientId = "erebor_api_swagger",
                ClientName = "Swagger UI for erebor_api",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris = {"https://localhost:7158/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:7158"},
                AllowedScopes = {"api1"}
            }
        };
}