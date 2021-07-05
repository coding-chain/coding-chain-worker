using Domain.TestExecution.OOP.CSharp;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public class CSharpCodeAnalyzerTests
    {
        private CsharpCodeAnalyzer _analyzer;

        [SetUp]
        public void SetUp()
        {
            _analyzer = new CsharpCodeAnalyzer();
        }

        [Test]
        public void find_method_name_should_work_on_static_expression_body()
        {
            var expectedMethodName = "Test1";
            var methodName = _analyzer.FindFunctionName($@"public static string {expectedMethodName}()=>""nothing""");
            Assert.AreEqual(expectedMethodName, methodName);
        }

        [Test]
        public void find_method_name_should_work_on_static_method()
        {
            var expectedMethodName = "Test1";
            var methodName = _analyzer.FindFunctionName($@"public static string {expectedMethodName}(){{return "";}}");
            Assert.AreEqual(expectedMethodName, methodName);
        }

        [Test]
        public void find_method_name_should_work_on_method_with_tuples()
        {
            var expectedMethodName = "Test1";
            var methodName =
                _analyzer.FindFunctionName($@"public (string, int)  {expectedMethodName}(){{return ("",1);}}");
            Assert.AreEqual(expectedMethodName, methodName);
        }

        [Test]
        public void find_method_name_should_return_null_on_mishapped_method()
        {
            var methodName = _analyzer.FindFunctionName(@"public (string, int)  (){return ("",1);}");
            Assert.Null(methodName);
        }
    }
}