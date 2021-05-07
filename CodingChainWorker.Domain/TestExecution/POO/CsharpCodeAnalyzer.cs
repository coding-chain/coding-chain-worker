using System.Text.RegularExpressions;

namespace Domain.TestExecution.POO
{
    public class CsharpCodeAnalyzer: IPOOCodeAnalyzer
    {
        public string FindMethodName(string code)
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
            return Regex.Match(code, @"\b(\w+)$").Value;
        }
    }
}