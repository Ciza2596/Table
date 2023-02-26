namespace CsvParser
{
    internal class ValueState : ParserState
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

        public override ParserState Quote(ParserContext context)
        {
            context.AddChar(QUTOE_CHARACTER);
            return ValueState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }
}
