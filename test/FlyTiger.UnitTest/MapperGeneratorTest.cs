using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlyTiger.Mapper;
using Xunit;
namespace FlyTiger.Generator.UnitTest
{
    public class MapperGeneratorTest : BaseGeneratorTest
    {

        //[Theory]
        //[InlineData("MapperCases/DicToDic.xml")]
        //[InlineData("MapperCases/GlobalNamespace.xml")]
        //[InlineData("MapperCases/ClassifyConversion.xml")]
        //[InlineData("MapperCases/ClassToStruct.xml")]
        //[InlineData("MapperCases/StructToClass.xml")]
        //[InlineData("MapperCases/StructToStruct.xml")]
        //[InlineData("MapperCases/IgnoreTargetProperty.xml")]
        //[InlineData("MapperCases/IgnoreNullTargetProperty.xml")]
        //[InlineData("MapperCases/IgnoreEmptyTargetProperty.xml")]
        //[InlineData("MapperCases/IgnoreNotExistingTargetProperty.xml")]
        //[InlineData("MapperCases/CustomerMappings.xml")]
        //[InlineData("MapperCases/CustomerMappingsInSubObject.xml")]
        //[InlineData("MapperCases/NavigateProperty.xml")]
        //[InlineData("MapperCases/NavigateComplexObject.xml")]
        //[InlineData("MapperCases/SubClassToClass.xml")]
        //[InlineData("MapperCases/SubStructToStruct.xml")]
        //[InlineData("MapperCases/SubStructToClass.xml")]
        //[InlineData("MapperCases/SubClassToStruct.xml")]
        //[InlineData("MapperCases/CircleRefrence.xml")]
        //[InlineData("MapperCases/ListToList.xml")]
        //[InlineData("MapperCases/ListToArray.xml")]
        //[InlineData("MapperCases/ListToIEnumerable.xml")]
        //[InlineData("MapperCases/ListToIQueryable.xml")]
        //[InlineData("MapperCases/ListToIList.xml")]
        //[InlineData("MapperCases/ListToICollection.xml")]
        //[InlineData("MapperCases/ArrayToList.xml")]
        //[InlineData("MapperCases/ArrayToArray.xml")]
        //[InlineData("MapperCases/ArrayToIEnumerable.xml")]
        //[InlineData("MapperCases/ArrayToIQueryable.xml")]
        //[InlineData("MapperCases/ArrayToIList.xml")]
        //[InlineData("MapperCases/ArrayToICollection.xml")]

        //[InlineData("MapperCases/ArrayClassToListStruct.xml")]
        //[InlineData("MapperCases/ArrayClassToArrayStruct.xml")]
        //[InlineData("MapperCases/ArrayClassToIEnumerableStruct.xml")]
        //[InlineData("MapperCases/ArrayClassToIQueryableStruct.xml")]
        //[InlineData("MapperCases/ArrayClassToIListStruct.xml")]
        //[InlineData("MapperCases/ArrayClassToICollectionStruct.xml")]


        //[InlineData("MapperCases/ArrayStructToListClass.xml")]
        //[InlineData("MapperCases/ArrayStructToArrayClass.xml")]
        //[InlineData("MapperCases/ArrayStructToIEnumerableClass.xml")]
        //[InlineData("MapperCases/ArrayStructToIQueryableClass.xml")]
        //[InlineData("MapperCases/ArrayStructToIListClass.xml")]
        //[InlineData("MapperCases/ArrayStructToICollectionClass.xml")]

        //[InlineData("MapperCases/SourceParentClassProperty.xml")]
        //[InlineData("MapperCases/TargetParentClassProperty.xml")]
        //[InlineData("MapperCases/OverwriteParentClassProperty.xml")]

        //[InlineData("MapperCases/AllInOne.xml")]
        //[InlineData("MapperCases/ManySourceTypesToOneTargetType.xml")]

        public void ShouldGenerateConverterClass(string testCaseFileName)
        {

            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                typeof(IQueryable<>).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            base.ShouldGenerateExpectCodeFile(new MapperGenerator(), testCaseFileName, assemblies);
        }
    }

}


