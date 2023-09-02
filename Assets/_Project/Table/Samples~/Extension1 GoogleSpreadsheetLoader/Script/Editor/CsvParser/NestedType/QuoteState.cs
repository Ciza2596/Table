
namespace CsvParser
{
    internal class QuoteState  : ParserState
    {
        public override ParserState AnyChar(char ch, ParserContext context)
        {
            //undefined, ignore "
            context.AddChar(ch);
            return QuotedValueState;
        }

        public override ParserState Comma(ParserContext context)
        {
            context.AddValue();
            return ValueStartState;
        }

        public override ParserState Quote(ParserContext context)
        {
            context.AddChar(QUTOE_CHARACTER);
            return QuotedValueState;
        }

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LineStartState;
        }
    }
}
