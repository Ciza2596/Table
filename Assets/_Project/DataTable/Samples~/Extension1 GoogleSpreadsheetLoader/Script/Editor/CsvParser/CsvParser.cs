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

			ParserState currentState = ParserState.LineStartState;
			foreach (var line in lines)
			{
				// Skip empty entries
				if (line.Length == 0)
					continue;
				
				foreach (var c in line)
				{
					switch (c) {
						case ParserState.COMMA_CHARACTER:
							currentState = currentState.Comma(context);
							break;
						
						case ParserState.QUTOE_CHARACTER:
							currentState = currentState.Quote(context);
							break;
						
						default:
							currentState = currentState.AnyChar(c, context);
							break;
					}
				}
				currentState = currentState.EndOfLine(context);
			}

			var allLines = context.GetAllLines();
			return allLines.ToArray();
		}
    }
}
