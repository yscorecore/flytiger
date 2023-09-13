using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{
    internal class EntityKeyFinder
    {
        const string KeyAttributeFullName = "System.ComponentModel.DataAnnotations.KeyAttribute";

        public static KeyMap GetEntityKeyMaps(ITypeSymbol source, ITypeSymbol target)
        {
            var targetKey = FindKeyProperty(target);
            if (targetKey == null)
            {
                return null;
            }
            else
            {
                foreach (var prop in source.GetAllMembers().OfType<IPropertySymbol>())
                {
                    if (prop.Name == targetKey.Name && prop.Type.Equals(targetKey.Type, SymbolEqualityComparer.Default))
                    {
                        return new KeyMap { SourceKey = prop.Name, TargetKey = targetKey.Name };
                    }
                }
                return null;
            }

        }
        public static IPropertySymbol FindKeyProperty(ITypeSymbol symbol)
        {
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.HasAttribute(KeyAttributeFullName))
                {
                    return prop;
                }
            }
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    return prop;
                }
            }
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.Name.Equals($"{symbol.Name}Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    return prop;
                }
            }
            return null;
        }
    }
    public class KeyMap
    {
        public string SourceKey { get; set; }
        public string TargetKey { get; set; }
    }
}
