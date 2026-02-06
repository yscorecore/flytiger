using System.ComponentModel;
using System.Reflection;
using FlyTiger.AutoNotify;
using Xunit;

namespace FlyTiger.Generator.UnitTest
{

    public class AutoNotifyGeneratorTest : BaseGeneratorTest
    {
        [Theory]
        [InlineData("AutoNotifyCases/HappyCase.xml")]
        [InlineData("AutoNotifyCases/CustomPropertyName.xml")]
        [InlineData("AutoNotifyCases/GenericType.xml")]
        [InlineData("AutoNotifyCases/CombinAllPartials.xml")]
        [InlineData("AutoNotifyCases/NotifyPropertyChangedDefined.xml")]
        [InlineData("AutoNotifyCases/NotifyPropertyChangedInherited.xml")]
        [InlineData("AutoNotifyCases/NotifyPropertyChangedInheritedFromGenerator.xml")]
        [InlineData("AutoNotifyCases/NestedType.xml")]
        [InlineData("AutoNotifyCases/EmptyNamespace.xml")]
        [InlineData("AutoNotifyCases/SameNameInMultipleNamespace.xml")]
        [InlineData("AutoNotifyCases/NotifyPropertyChangedInheritedFromOtherAssembly.xml")]
        [InlineData("AutoNotifyCases/ComplexTypeFromCurrentSource.xml")]
        [InlineData("AutoNotifyCases/ComplexTypeFromCurrentSourceAndOtherNamespace.xml")]
        [InlineData("AutoNotifyCases/ComplexTypeFromOtherAssembly.xml")]
        public void ShouldGenerateExpectPartailClass(string testCaseFileName)
        {
            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                typeof(INotifyPropertyChanged).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            //base.UpdateTestOutput(new AutoNotifyGenerator(), testCaseFileName, assemblies);
            base.ShouldGenerateExpectCodeFile(new AutoNotifyGenerator(), testCaseFileName, assemblies);
        }

        //[Theory]
        //[InlineData("AutoNotifyCases/Error.EmptyPropertyName.xml")]
        //[InlineData("AutoNotifyCases/Error.InvalidPropertyName.xml")]
        //[InlineData("AutoNotifyCases/Error.PropertyNameEqualFieldName.xml")]
        // [Theory]
        private void ShouldReportDigError(string testCaseFileName)
        {
            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                typeof(INotifyPropertyChanged).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            base.ShouldReportDiagnostic(new AutoNotifyGenerator(), testCaseFileName, assemblies);
        }
#pragma warning disable CS0067
        public class BaseClass : INotifyPropertyChanged
        {

            public event PropertyChangedEventHandler PropertyChanged;


        }
#pragma warning restore CS0067
    }
}
