namespace CizaTable.Editor
{
	internal abstract class ParserState
	{
		// CONST & STATIC: -----------------------------------------------------------------------

		public const char COMMA_CHARACTER = ',';
		public const char QUTOE_CHARACTER = '"';

		public static readonly LineStartState LINE_START_STATE = new LineStartState();
		public static readonly ValueStartState VALUE_START_STATE = new ValueStartState();
		public static readonly ValueState VALUE_STATE = new ValueState();
		public static readonly QuotedValueState QUOTED_VALUE_STATE = new QuotedValueState();
		public static readonly QuoteState QUOTE_STATE = new QuoteState();

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public abstract ParserState AnyChar(char ch, ParserContext context);
		public abstract ParserState Comma(ParserContext context);
		public abstract ParserState Quote(ParserContext context);
		public abstract ParserState EndOfLine(ParserContext context);
	}
}