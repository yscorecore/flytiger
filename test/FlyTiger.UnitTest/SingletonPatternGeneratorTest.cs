﻿using System;
using System.Reflection;
using FlyTiger.SingletonPattern;
using Xunit;

namespace FlyTiger.Generator.UnitTest
{

    public class SingletonPatternGeneratorTest : BaseGeneratorTest
    {
        [Theory]
        [InlineData("SingletonPatternCases/HappyCase.xml")]
        [InlineData("SingletonPatternCases/NestedType.xml")]
        [InlineData("SingletonPatternCases/GeneicType.xml")]
        [InlineData("SingletonPatternCases/CustomerInstanceName.xml")]
        public void ShouldGenerateExpectSingletonPaitalClass(string testCaseFileName)
        {
            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            base.ShouldGenerateExpectCodeFile(new SingletonPatternGenerator(), testCaseFileName, assemblies);
        }

        //[Theory]
        //[InlineData("SingletonPatternCases/Error.AlreadyExistCtor.xml")]
        //[Theory]
        private void ShouldReportDigError(string testCaseFileName)
        {
            var assemblies = new[]
            {
                typeof(Binder).GetTypeInfo().Assembly,
                Assembly.GetExecutingAssembly()
            };
            base.ShouldReportDiagnostic(new SingletonPatternGenerator(), testCaseFileName, assemblies);
        }
    }
}
