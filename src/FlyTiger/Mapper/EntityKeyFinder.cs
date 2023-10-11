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
            var sourceAttributeKey = FindKeyPropertyByAttribute(source);
            if (sourceAttributeKey != null)
            {
                var targetKey = FindPropertyByInfo(target, sourceAttributeKey);
                if (targetKey != null)
                {
                    // 优先使用source指定的key
                    return new KeyMap { SourceKey = sourceAttributeKey.Name, TargetKey = targetKey.Name };
                }
            }
            var targetAttributeKey = FindKeyPropertyByAttribute(target) ?? FindKeyPropertyByName(target);
            if (targetAttributeKey != null)
            {
                var sourceKey = FindPropertyByInfo(source, targetAttributeKey);
                if (sourceKey != null)
                {
                    return new KeyMap { SourceKey = sourceKey.Name, TargetKey = targetAttributeKey.Name };
                }
            }
            return null;
        }
        public static IPropertySymbol GetEntityKey(ITypeSymbol type)
        {
            return FindKeyPropertyByAttribute(type) ?? FindKeyPropertyByName(type);
        }
        private static IPropertySymbol FindPropertyByInfo(ITypeSymbol symbol, IPropertySymbol propInfo)
        {
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.Name == propInfo.Name && prop.Type.Equals(propInfo.Type, SymbolEqualityComparer.Default))
                {
                    return prop;
                }
            }
            return null;
        }
        private static IPropertySymbol FindKeyPropertyByAttribute(ITypeSymbol symbol)
        {
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.HasAttribute(KeyAttributeFullName))
                {
                    return prop;
                }
            }
            return null;
        }
        private static IPropertySymbol FindKeyPropertyByName(ITypeSymbol symbol)
        {
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
