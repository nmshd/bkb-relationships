﻿using Microsoft.Extensions.Hosting;

namespace Relationships.API.Extensions
{
    public static class IHostEnvironmentExtensions
    {
        public static bool IsLocal(this IHostEnvironment env)
        {
            return env.EnvironmentName == "Local";
        }
    }
}