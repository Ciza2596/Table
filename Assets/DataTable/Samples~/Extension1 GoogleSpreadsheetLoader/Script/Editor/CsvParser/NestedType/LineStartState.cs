namespace CsvParser
{
    internal class LineStartState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return ValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return ValueStartState;
        }

        public override ParserState Quote(ParserContext context) =>
            QuotedValueState;


        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddLine();
            return LineStartState;
        }
    }
}