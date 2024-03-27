using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CsvParser
{
    public class CsvParser
    {
        public string[][] Parse(string csvData)
        {
            var context = new ParserContext();

            // Handle both Windows and Mac line endings
            var lines = Regex.Split(csvData, "\n|\r\n");


            bool isInQuotation = false;
            ParserState currentState = ParserState.LineStartState;

            foreach (var line in lines)
            {
                // Skip empty entries
                if (line.Length == 0)
                    continue;

                foreach (var c in line)
                {
                    switch (c)
                    {
                        case ParserState.COMMA_CHARACTER:
                            currentState = currentState.Comma(context);
                            break;

                        case ParserState.QUTOE_CHARACTER:
                            isInQuotation = !isInQuotation;
                            currentState = currentState.Quote(context);
                            break;

                        default:
                            currentState = currentState.AnyChar(c, context);
                            break;
                    }
                }

                if (!isInQuotation)
                    currentState = currentState.EndOfLine(context);
            }

            var allLines = context.GetAllLines();
            var length = allLines[0].Length;
            var newAllLines = new List<string[]>();
            foreach (var line in allLines)
                if (line.Length == length)
                    newAllLines.Add(line);
            return newAllLines.ToArray();
        }
    }
}