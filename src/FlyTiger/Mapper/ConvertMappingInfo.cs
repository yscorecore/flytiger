using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{

    public class ConvertMappingInfo
    {
        private ConvertMappingInfo()
        {

        }
        public ITypeSymbol TargetType { get; private set; }
        public ITypeSymbol SourceType { get; private set; }
        public string TargetTypeFullDisplay { get; private set; }
        public string SourceTypeFullDisplay { get; private set; }
        public HashSet<string> IgnoreTargetProperties { get; private set; }
        public Dictionary<string, string> CustomerMappings { get; private set; }
        public bool CheckTargetPropertiesFullFilled { get; private set; }
        public bool CheckSourcePropertiesFullUsed { get; private set; }
        public string ConvertToMethodName { get; private set; }
        public AttributeData FromAttribute { get; private set; }

        public ConvertMappingInfo Fork(ITypeSymbol fromType, ITypeSymbol toType)
        {
            return new ConvertMappingInfo
            {
                SourceType = fromType,
                TargetType = toType,
                TargetTypeFullDisplay = toType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                SourceTypeFullDisplay = fromType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                CheckSourcePropertiesFullUsed = this.CheckSourcePropertiesFullUsed,
                CheckTargetPropertiesFullFilled = this.CheckTargetPropertiesFullFilled,
                CustomerMappings = new Dictionary<string, string>(),
                IgnoreTargetProperties = new HashSet<string>(),
                FromAttribute = this.FromAttribute,
            };
        }
        public static ConvertMappingInfo FromAttributeData(AttributeData attributeData)
        {
            var arguments = attributeData.ConstructorArguments;
            var fromType = arguments.First().Value as INamedTypeSymbol;
            var toType = arguments.Last().Value as INamedTypeSymbol;
            var ignoreProperties = attributeData.NamedArguments
                .Where(p => p.Key == MapperGenerator.IgnoreTargetPropertiesPropertyName)
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
            var checkType = attributeData.NamedArguments
                .Where(p => p.Key == MapperGenerator.CheckTypeName)
                .Where(p => p.Value.IsNull == false)
                .Select(p => Convert.ToInt32(p.Value.Value))
                .FirstOrDefault();
            var methodName = new string(toType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)
                .Where(ch => char.IsLetterOrDigit(ch)).ToArray());
            return new ConvertMappingInfo
            {
                SourceType = fromType,
                TargetType = toType,
                TargetTypeFullDisplay = toType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                SourceTypeFullDisplay = fromType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                IgnoreTargetProperties = new HashSet<string>(ignoreProperties),
                CustomerMappings = customMappings,
                ConvertToMethodName = $"To{methodName}",
                CheckSourcePropertiesFullUsed = (checkType & 1) == 1,
                CheckTargetPropertiesFullFilled = (checkType & 2) == 2,
                FromAttribute = attributeData
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
            if (sourceType.IsPrimitive() || targetType.IsPrimitive())
            {
                return false;
            }
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
