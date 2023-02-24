
namespace CsvParser
{
    internal class ValueStartState : LineStartState
    {
        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }
}
