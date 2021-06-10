using System;
using System.Text;
using CodingChainApi.Infrastructure.Services.TestsParsers;
using Domain.TestExecution.Imperative.Typescript;
using NUnit.Framework;

namespace CodingChainWorker.Domain.Tests.TestExecution
{
    public class JestTestsParserTests
    {
        private readonly string failingTestName = "test0";
        private readonly string passingTestName = "test1";

        private  string
            TestOutput =>
                $@"FAIL ./test.spec.ts  Participation tests    ✕ {failingTestName} (49ms)    ✓ {passingTestName} (3ms)  ● Participation tests › {failingTestName}    expect(received).toBeTruthy()        Expected value to be truthy, instead received      false      18 | describe(""Participation
            tests"", () => {{      19 |             test(""{failingTestName}"", () => {{    > 20 |                 expect(outputValidator0({passingTestName}(inputGenerator0()))).toBeTruthy()      21 |             }});      22 |      23 | test(""{passingTestName}"", () => {{            at Object.<anonymous> (test.spec.ts:20:68)Test Suites: 1 failed, 1 totalTests:       1 failed, 1 passed, 2 totalSnapshots:   0 totalTime:        2.495sRan all test suites matching /test/i.npm notice npm notice New minor version of npm available! 7.15.1 -> 7.16.0npm notice Changelog: <https://github.com/npm/cli/releases/tag/v7.16.0>npm notice Run `npm install -g npm@7.16.0` to update!npm notice 	
        ";

        [Test]
        public void should_return_false_on_test_failed()
        {
            var parser = new WindowsJestTestsParser();
            var hasPassed = parser.FunctionPassed(passingTestName, null, TestOutput);
            Assert.True(hasPassed);
        }
        [Test]
        public void should_return_true_on_test_failed()
        {
            var parser = new WindowsJestTestsParser();
            var hasPassed = parser.FunctionPassed(failingTestName, null, TestOutput);
            Assert.False(hasPassed);
        }
    }
}