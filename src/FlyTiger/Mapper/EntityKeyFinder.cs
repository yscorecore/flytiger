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
            var sourceAttributeKeys = FindKeyPropertyByAttribute(source);
            if (sourceAttributeKeys != null)
            {
                var targetKeys = FindPropertyByInfo(target, sourceAttributeKeys);
                if (targetKeys != null)
                {
                    // 优先使用source指定的key
                    return new KeyMap { SourceKey = sourceAttributeKeys, TargetKey = targetKeys };
                }
            }
            var targetAttributeKeys = FindKeyPropertyByAttribute(target) ?? FindKeyPropertyByName(target);
            if (targetAttributeKeys != null)
            {
                var sourceKeys = FindPropertyByInfo(source, targetAttributeKeys);
                if (sourceKeys != null)
                {
                    return new KeyMap { SourceKey = sourceKeys, TargetKey = targetAttributeKeys };
                }
            }
            return null;
        }
        public static IPropertySymbol[] GetEntityKey(ITypeSymbol type)
        {
            return FindKeyPropertyByAttribute(type) ?? FindKeyPropertyByName(type);
        }
        private static IPropertySymbol[] FindPropertyByInfo(ITypeSymbol symbol, IPropertySymbol[] propInfo)
        {
            var result = propInfo.Select(p => symbol.GetAllMembers().OfType<IPropertySymbol>().SingleOrDefault(t => t.Name == p.Name && t.Type.Equals(p.Type, SymbolEqualityComparer.Default))).ToArray();
            return result.Contains(null) ? null : result;
        }
        private static IPropertySymbol[] FindKeyPropertyByAttribute(ITypeSymbol symbol)
        {
            var keys = symbol.GetAllMembers().OfType<IPropertySymbol>().Where(p => p.HasAttribute(KeyAttributeFullName)).ToArray();
            return keys.Any() ? keys : null;
        }
        private static IPropertySymbol[] FindKeyPropertyByName(ITypeSymbol symbol)
        {
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new[] { prop };
                }
            }
            foreach (var prop in symbol.GetAllMembers().OfType<IPropertySymbol>())
            {
                if (prop.Name.Equals($"{symbol.Name}Id", StringComparison.InvariantCultureIgnoreCase))
                {
                    return new[] { prop };
                }
            }
            return null;
        }
    }
    internal class KeyMap
    {
        public IPropertySymbol[] SourceKey { get; set; }
        public IPropertySymbol[] TargetKey { get; set; }
    }
}
