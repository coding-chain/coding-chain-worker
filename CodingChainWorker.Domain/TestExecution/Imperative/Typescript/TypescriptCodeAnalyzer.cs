using System;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.Imperative.Typescript
{
    public class TypescriptCodeAnalyzer : ICodeAnalyzer

    {
        public string? FindFunctionName(string code)
        {
            var funcEndIdx = code.IndexOf("function", StringComparison.Ordinal) + "function".Length;
            var signatureWithoutStart = code[funcEndIdx..];
            var startParamsIdx = signatureWithoutStart.IndexOf('(');
            return signatureWithoutStart[..startParamsIdx].Trim();
        }
    }
}