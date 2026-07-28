namespace CizaTable.Editor
{
    internal class ValueState : ParserState
    {
        // PUBLIC METHOD: ----------------------------------------------------------------------

        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return VALUE_STATE;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return VALUE_START_STATE;
        }

        public override ParserState Quote(ParserContext context)
        {
            context.AddChar(QUTOE_CHARACTER);
            return VALUE_STATE;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LINE_START_STATE;
        }
    }
}
