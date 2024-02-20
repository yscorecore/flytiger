using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{
    internal class DiagnosticDescriptors
    {
        private const string Category = "FlyTiger.Mapper";

        private static ConcurrentDictionary<string, LocalizableString> ResourceCache = new ConcurrentDictionary<string, LocalizableString>();
        private static LocalizableString GetLocalizableString(string key)
        {
            return ResourceCache.GetOrAdd(key, (k) => new LocalizableResourceString(k, Resources.ResourceManager, typeof(Resources)));
        }

        public static DiagnosticDescriptor MapperNotUsed =
           new DiagnosticDescriptor(
               "FT50000",
           GetLocalizableString(nameof(Resources.MapperNotUsed_Title)),
           GetLocalizableString(nameof(Resources.MapperNotUsed_MessageFormat)),
           Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
           description: GetLocalizableString(nameof(Resources.MapperNotUsed_Description)));

        public static DiagnosticDescriptor MissingMapper =
           new DiagnosticDescriptor(
               "FT50001",
           GetLocalizableString(nameof(Resources.MissingMapper_Title)),
           GetLocalizableString(nameof(Resources.MissingMapper_MessageFormat)),
           Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
           description: GetLocalizableString(nameof(Resources.MissingMapper_Description)));

        public static DiagnosticDescriptor DuplicateMapper =
           new DiagnosticDescriptor(
               "FT50002",
           GetLocalizableString(nameof(Resources.DuplicateMapper_Title)),
           GetLocalizableString(nameof(Resources.DuplicateMapper_MessageFormat)),
           Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
           description: GetLocalizableString(nameof(Resources.DuplicateMapper_Description)));

    }
}
