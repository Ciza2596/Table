
namespace CizaTable.Editor
{
    internal class ValueStartState : LineStartState
    {
        // PUBLIC METHOD: ----------------------------------------------------------------------

        public override ParserState EndOfLine(ParserContext context)
        {
            context.AddValue();
            context.AddLine();
            return LINE_START_STATE;
        }
    }
}
