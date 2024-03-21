using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;

namespace FlyTiger.AutoConstructor
{
    internal class DiagnosticDescriptors
    {
        private const string Category = "FlyTiger.AutoCtor";

        private static ConcurrentDictionary<string, LocalizableString> ResourceCache = new ConcurrentDictionary<string, LocalizableString>();
        private static LocalizableString GetLocalizableString(string key)
        {
            return ResourceCache.GetOrAdd(key, (k) => new LocalizableResourceString(k, Resources.ResourceManager, typeof(Resources)));
        }

        public static DiagnosticDescriptor InitializeMethodShouldNotBeStatic
            = new DiagnosticDescriptor("FT10001",
           GetLocalizableString(nameof(Resources.InitializeMethodShouldNotBeStatic_Title)),
           GetLocalizableString(nameof(Resources.InitializeMethodShouldNotBeStatic_MessageFormat)),
           Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
           description: GetLocalizableString(nameof(Resources.InitializeMethodShouldNotBeStatic_Description)));

        public static DiagnosticDescriptor InitializeMethodShouldReturnVoid
            = new DiagnosticDescriptor("FT10002",
            GetLocalizableString(nameof(Resources.InitializeMethodShouldReturnVoid_Title)),
            GetLocalizableString(nameof(Resources.InitializeMethodShouldReturnVoid_MessageFormat)),
            Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
            description: GetLocalizableString(nameof(Resources.InitializeMethodShouldReturnVoid_Description)));

        public static DiagnosticDescriptor InitializeMethodShouldOnlyOne
            = new DiagnosticDescriptor("FT10003",
           GetLocalizableString(nameof(Resources.InitializeMethodShouldOnlyOne_Title)),
           GetLocalizableString(nameof(Resources.InitializeMethodShouldOnlyOne_MessageFormat)),
           Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
           description: GetLocalizableString(nameof(Resources.InitializeMethodShouldOnlyOne_Description)));

        public static DiagnosticDescriptor InitializeMethodShouldHasNoneArguments
        = new DiagnosticDescriptor("FT10004",
       GetLocalizableString(nameof(Resources.InitializeMethodShouldHasNoneArguments_Title)),
       GetLocalizableString(nameof(Resources.InitializeMethodShouldHasNoneArguments_MessageFormat)),
       Category, DiagnosticSeverity.Warning, isEnabledByDefault: true,
       description: GetLocalizableString(nameof(Resources.InitializeMethodShouldHasNoneArguments_Description)));
    }

}
