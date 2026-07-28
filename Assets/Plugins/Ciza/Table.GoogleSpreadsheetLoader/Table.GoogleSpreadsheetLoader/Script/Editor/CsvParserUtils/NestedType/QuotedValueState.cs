namespace CizaTable.Editor
{
	internal class QuotedValueState : ParserState
	{
		// PUBLIC METHOD: ----------------------------------------------------------------------

		public override ParserState AnyChar(char ch, ParserContext context)
		{
			context.AddChar(ch);
			return QUOTED_VALUE_STATE;
		}

		public override ParserState Comma(ParserContext context)
		{
			context.AddChar(COMMA_CHARACTER);
			return QUOTED_VALUE_STATE;
		}

		public override ParserState Quote(ParserContext context) =>
			QUOTE_STATE;

		public override ParserState EndOfLine(ParserContext context)
		{
			context.AddChar('\r');
			context.AddChar('\n');
			return QUOTED_VALUE_STATE;
		}
	}
}