using System.Collections;
using System.Reflection;

namespace Velora.Application.Shared.Infrastructure;

public static class ApplySeedDefaults
{



    public static void Apply<TTemplate, TDefault>(
        TTemplate template,
        TDefault defaults)
        where TTemplate : class
        where TDefault : class
    {
        if (template == null || defaults == null)
            return;

        var ignoredProperties = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase)
    {
        "Id",
        "SectionId",
        "PageId",
        "ComponentTypeId",
        "SortOrder"
    };

        var templateProperties = typeof(TTemplate)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var defaultProperties = typeof(TDefault)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var target in templateProperties)
        {
            var name = target.Name;

            if (ignoredProperties.Contains(name))
            {
                Console.WriteLine($"[SKIP] {name}: Ignored property");
                continue;
            }

            if (!target.CanRead || !target.CanWrite)
            {
                Console.WriteLine($"[SKIP] {name}: Cannot read/write");
                continue;
            }

            if (!defaultProperties.TryGetValue(name, out var source))
            {
                Console.WriteLine($"[SKIP] {name}: Source property not found");
                continue;
            }

            var sourceValue = source.GetValue(defaults);

            if (sourceValue == null)
            {
                Console.WriteLine($"[SKIP] {name}: Source value is NULL");
                continue;
            }

            if (sourceValue is string sourceText &&
                string.IsNullOrWhiteSpace(sourceText))
            {
                Console.WriteLine($"[SKIP] {name}: Source string is EMPTY");
                continue;
            }

            var targetType =
                Nullable.GetUnderlyingType(target.PropertyType)
                ?? target.PropertyType;

            var sourceType =
                Nullable.GetUnderlyingType(source.PropertyType)
                ?? source.PropertyType;

            if (!targetType.IsAssignableFrom(sourceType))
            {
                Console.WriteLine(
                    $"[SKIP] {name}: Type mismatch | " +
                    $"Source={source.PropertyType.Name} | " +
                    $"Target={target.PropertyType.Name}");

                continue;
            }

            var finalValue = sourceValue is string text
                ? text.Trim()
                : sourceValue;

            target.SetValue(template, finalValue);

        }
    }




}