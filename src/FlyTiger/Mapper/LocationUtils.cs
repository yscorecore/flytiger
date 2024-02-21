using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace FlyTiger.Mapper
{
    internal class LocationUtils
    {
        public static Location FindAttributeLocation(AttributeData attributeData)
        {
            var syntaxReference = attributeData.ApplicationSyntaxReference;
            var syntaxNode = syntaxReference.GetSyntax();
            return syntaxNode.GetLocation();
        }
        public static Location FindMemberLocation(ISymbol fieldOrProperty)
        {
            return fieldOrProperty.Locations.FirstOrDefault() ?? Location.None;
        }

    }
}
