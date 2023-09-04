namespace CsvParser
{
	internal abstract class ParserState
	{
		public const char COMMA_CHARACTER = ',';
		public const char QUTOE_CHARACTER = '"';

		public static readonly LineStartState   LineStartState   = new LineStartState();
		public static readonly ValueStartState  ValueStartState  = new ValueStartState();
		public static readonly ValueState       ValueState       = new ValueState();
		public static readonly QuotedValueState QuotedValueState = new QuotedValueState();
		public static readonly QuoteState       QuoteState       = new QuoteState();

		public abstract ParserState AnyChar(char            ch, ParserContext context);
		public abstract ParserState Comma(ParserContext     context);
		public abstract ParserState Quote(ParserContext     context);
		public abstract ParserState EndOfLine(ParserContext context);
	}
}
