namespace CizaTable.Editor
{
	internal class QuoteState : ParserState
	{
		// PUBLIC METHOD: ----------------------------------------------------------------------
		
		public override ParserState AnyChar(char ch, ParserContext context)
		{
			//undefined, ignore "
			context.AddChar(ch);
			return QUOTED_VALUE_STATE;
		}

		public override ParserState Comma(ParserContext context)
		{
			context.AddValue();
			return VALUE_START_STATE;
		}

		public override ParserState Quote(ParserContext context)
		{
			context.AddChar(QUTOE_CHARACTER);
			return QUOTED_VALUE_STATE;
		}

		public override ParserState EndOfLine(ParserContext context)
		{
			context.AddValue();
			context.AddLine();
			return LINE_START_STATE;
		}
	}
}
