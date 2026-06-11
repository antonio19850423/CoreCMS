using HotChocolate.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Microsoft.Extensions.Hosting;

namespace Velora.Application.Services
{
    public class ComponentRuleCacheService : IComponentRuleCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        private const string _cacheKey = "cms_component_rules";

        public ComponentRuleCacheService(
            IMemoryCache cache,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _cache = cache;
            _env = env;
            _configuration = configuration;
        }

        public async Task<ResultDto<ComponentRulesRootView>> GetComponentRulesAsync()
        {
            if (!_cache.TryGetValue(_cacheKey, out ComponentRulesRootView data)
                || _env.IsDevelopment())
            {
                var assembly = typeof(SeedJsonModel).Assembly;

                using var stream = assembly.GetManifestResourceStream(
                    "Velora.Application.Shared.Resources.ComponentRules.json");

                if (stream == null)
                    throw new FileNotFoundException("ComponentRules.json not found");

                using var document = await JsonDocument.ParseAsync(stream);

                var root = new ComponentRulesRootView
                {
                    Components = new Dictionary<string, ComponentRuleView>()
                };

                foreach (var component in document.RootElement.EnumerateObject())
                {
                    var rules = component.Value.GetProperty("rules");

                    var section = new Dictionary<string, bool>();
                    var sectionItem = new Dictionary<string, bool>();

                    foreach (var rule in rules.EnumerateObject())
                    {
                        // اگر object بود => items (sectionItem)
                        if (rule.Value.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var itemRule in rule.Value.EnumerateObject())
                            {
                                sectionItem[itemRule.Name] = itemRule.Value.GetBoolean();
                            }
                        }
                        else
                        {
                            section[rule.Name] = rule.Value.GetBoolean();
                        }
                    }

                    root.Components[component.Name] = new ComponentRuleView
                    {
                        Section = section,
                        SectionItem = sectionItem
                    };
                }

                data = root;

                if (!_env.IsDevelopment())
                {
                    _cache.Set(_cacheKey, data, TimeSpan.FromMinutes(10));
                }
            }

            return new ResultDto<ComponentRulesRootView>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Data = data
            };
        }
    }
}
