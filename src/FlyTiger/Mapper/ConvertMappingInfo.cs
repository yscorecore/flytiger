using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger
{

    class ConvertMappingInfo
    {
        public ITypeSymbol TargetType { get; internal set; }
        public ITypeSymbol SourceType { get; internal set; }
        public HashSet<string> IgnoreTargetProperties { get; internal set; }
        public Dictionary<string, string> CustomerMappings { get; internal set; }
        public string ConvertToMethodName { get; internal set; }

        public static ConvertMappingInfo FromAttributeData(AttributeData attributeData)
        {
            var arguments = attributeData.ConstructorArguments;
            var fromType = arguments.First().Value as INamedTypeSymbol;
            var toType = arguments.Last().Value as INamedTypeSymbol;
            var ignoreProperties = attributeData.NamedArguments
                .Where(p => p.Key == MapperGenerator. IgnoreTargetPropertiesPropertyName)
                .Where(p => p.Value.IsNull == false)
                .SelectMany(p => p.Value.Values.Select(t => (string)t.Value))
                .Where(p => !string.IsNullOrWhiteSpace(p));
            var customMappings = attributeData.NamedArguments
                .Where(p => p.Key == MapperGenerator.CustomMappingsPropertyName)
                .Where(p => p.Value.IsNull == false)
                .SelectMany(p => p.Value.Values.Select(t => (string)t.Value))
                .Select(ParseCustomMapping)
                .Where(item => item != null)
                .Select(item => item.Value)
                .ToLookup(item => item.Key, item => item.Value)
                .ToDictionary(p => p.Key, p => p.Last());
            var methodName = new string(toType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return new ConvertMappingInfo
            {
                SourceType = fromType,
                TargetType = toType,
                IgnoreTargetProperties = new HashSet<string>(ignoreProperties),
                CustomerMappings = customMappings,
                ConvertToMethodName = $"To{methodName}",
            };
        }


        static KeyValuePair<string, string>? ParseCustomMapping(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }

            var equalIndex = expression.IndexOf('=');
            if (equalIndex > 0)
            {
                var targetExpression = expression.Substring(0, equalIndex).Trim();
                var sourceExpression = expression.Substring(equalIndex + 1).Trim();
                return new KeyValuePair<string, string>(targetExpression, sourceExpression);
            }
            else
            {
                return null;
            }
        }

        public static bool CanMappingSubObject(ITypeSymbol sourceType, ITypeSymbol targetType)
        {
            if (targetType is INamedTypeSymbol namedTargetType)
            {
                if (sourceType.TypeKind == TypeKind.Class || sourceType.TypeKind == TypeKind.Struct)
                {
                    if (targetType.TypeKind == TypeKind.Struct) return true;
                    return targetType.TypeKind == TypeKind.Class && !targetType.IsAbstract &&
                           namedTargetType.HasEmptyCtor();
                }
            }

            return false;
        }
    }

}
