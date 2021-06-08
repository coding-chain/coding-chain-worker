using System.Text.RegularExpressions;
using Domain.TestExecution.Helpers;

namespace Domain.TestExecution.OOP.CSharp
{
    public class CsharpCodeAnalyzer : ICodeAnalyzer
    {
        public string? FindFunctionName(string code)
        {
            var match = Regex.Match(code, @"{[\s\S]*}|=>");
            var openBracketCnt = 0;
            var hasChanged = false;
            int i;
            for (i = match.Index; i >= 0; i--)
            {
                if (code[i] == ')')
                {
                    hasChanged = true;
                    openBracketCnt--;
                }

                if (code[i] == '(')
                {
                    hasChanged = true;
                    openBracketCnt++;
                }

                if (openBracketCnt == 0 && hasChanged) break;
            }

            code = code[..i];
            var res = Regex.Match(code, @"\b(\w+)$").Value;
            return res.Length > 0 ? res : null;
        }
    }
}