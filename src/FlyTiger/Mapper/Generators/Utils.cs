using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FlyTiger.Mapper.Generators
{
    internal class Utils
    {
        private static readonly ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>
            sourcePropertiesCache = new ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>(SymbolEqualityComparer.Default);
        public static Dictionary<string, IPropertySymbol> GetSourcePropertyDictionary(ITypeSymbol typeSymbol)
        {
            return sourcePropertiesCache.GetOrAdd(typeSymbol, (s) => s.GetAllMembers()
                 .OfType<IPropertySymbol>()
                 .Where(p => !p.IsWriteOnly && p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer && p.DeclaredAccessibility == Accessibility.Public)
                 .Select(p => new { p.Name, Property = p })
                 .ToLookup(p => p.Name)
                 .ToDictionary(p => p.Key, p => p.First().Property));
        }
        private static readonly ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>
           targetPropertiesCache = new ConcurrentDictionary<ITypeSymbol, Dictionary<string, IPropertySymbol>>(SymbolEqualityComparer.Default);

        public static Dictionary<string, IPropertySymbol> GetTargetPropertyDictionary(ITypeSymbol typeSymbol)
        {
            return targetPropertiesCache.GetOrAdd(typeSymbol, s => s.GetAllMembers()
                .OfType<IPropertySymbol>()
                .Where(p => !p.IsReadOnly && p.CanBeReferencedByName && !p.IsStatic && !p.IsIndexer && p.DeclaredAccessibility == Accessibility.Public)
                .Select(p => new { p.Name, Property = p })
                .ToLookup(p => p.Name)
                .ToDictionary(p => p.Key, p => p.First().Property));
        }

        public static string FormatRefrence(string refrenceName, string expression)
        {
            return string.IsNullOrEmpty(refrenceName) ? expression : $"{refrenceName}.{expression}";
        }
        public static INamedTypeSymbol GetItemType(ITypeSymbol typeSymbol)
        {
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
            {
                return arrayTypeSymbol.ElementType as INamedTypeSymbol;
            }

            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                return namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol;
            }

            return null;
        }
        public static bool IsDictionary(ITypeSymbol type)
        {
            if (type is INamedTypeSymbol namedSourcePropType)
            {
                if (namedSourcePropType.IsGenericType)
                {
                    var genericType = namedSourcePropType.ConstructUnboundGenericType();
                    if (genericType.SafeEquals(typeof(IDictionary<,>)))
                    {
                        return true;
                    }
                    if (genericType.SafeEquals(typeof(Dictionary<,>)))
                    {
                        return true;
                    }
                    if (genericType.SafeEquals(typeof(ImmutableDictionary<,>)))
                    {
                        return true;
                    }
                    if (genericType.SafeEquals(typeof(IImmutableDictionary<,>)))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        public static (ITypeSymbol, ITypeSymbol) GetKeyValueType(ITypeSymbol type)
        {
            INamedTypeSymbol namedType = type as INamedTypeSymbol;
            return (namedType.TypeArguments[0], namedType.TypeArguments[1]);
        }
        public static bool CanAssign(ITypeSymbol source, ITypeSymbol target, MapperContext context)
        {
            var conversion = context.Compilation.ClassifyConversion(source, target);
            return conversion.IsImplicit || conversion.IsBoxing;
        }

        public static bool CanMappingSubObjectProperty(ITypeSymbol sourceType, ITypeSymbol targetType,
          MapperContext convertContext)
        {
            if (convertContext.HasWalked(targetType))
            {
                return false;
            }
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
        static bool FindNavigatePaths(string targetName,
             Dictionary<string, IPropertySymbol> sourceProperties, ref List<KeyValuePair<string, IPropertySymbol>> paths)
        {
            var comparisonRule = StringComparison.InvariantCultureIgnoreCase;
            if (string.IsNullOrEmpty(targetName))
            {
                return false;
            }

            var matchProps = sourceProperties
                .Where(p => targetName.StartsWith(p.Key, comparisonRule))
                .OrderByDescending(p => p.Key.Length);


            foreach (var matchProp in matchProps)
            {
                if (matchProp.Key.Equals(targetName, comparisonRule))
                {
                    paths.Add(matchProp);
                    return true;
                }
                else
                {
                    var leftName = targetName.Substring(matchProp.Key.Length);
                    if (FindNavigatePaths(leftName, GetSourcePropertyDictionary(matchProp.Value.Type), ref paths))
                    {
                        paths.Insert(0, matchProp);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool HasSuggestionPath(KeyValuePair<string, IPropertySymbol> target,
            Dictionary<string, IPropertySymbol> sourceProperties, out List<IPropertySymbol> paths, MapperContext context)
        {
            var navPaths = new List<KeyValuePair<string, IPropertySymbol>>();
            if (FindNavigatePaths(target.Key, sourceProperties, ref navPaths))
            {
                if (CanAssign(navPaths.Last().Value.Type, target.Value.Type, context))
                {
                    paths = navPaths.Select(p => p.Value).ToList();
                    return true;
                }
            }
            paths = navPaths.Select(p => p.Value).ToList();
            return false;
        }

    }
}
