

namespace CsvParser
{
    internal class QuotedValueState : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            context.AddChar(ch);
            return QuotedValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddChar(COMMA_CHARACTER);
            return QuotedValueState;
        }

        public override ParserState Quote(ParserContext context)
        {
            return QuoteState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddChar('\r');
            context.AddChar('\n');
            return QuotedValueState;
        }
    }
}
