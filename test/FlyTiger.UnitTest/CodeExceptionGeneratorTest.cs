using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FlyTiger.CodeException;
using Xunit;

namespace FlyTiger.Generator.UnitTest
{
    public class CodeExceptionGeneratorTest : BaseGeneratorTest
    {
        [Theory]
        [InlineData("CodeExceptionCases/HappyCase.xml")]
        [InlineData("CodeExceptionCases/StaticMethod.xml")]
        public void ShouldGenerateExpectSingletonPaitalClass(string testCaseFileName)
        {
            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            base.ShouldGenerateExpectCodeFile(new CodeExceptionGenerator(), testCaseFileName, assemblies);
        }
    }
}
