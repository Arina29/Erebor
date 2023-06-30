﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace IdentityServer.Models
{
    public class ExternalProviderModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? AuthenticationScheme { get; set; } = String.Empty;
    }
}