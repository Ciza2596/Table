using System.Collections.Generic;
using System.Text;


namespace CsvParser
{
    internal class ParserContext 
    {
        private readonly StringBuilder _currentValue = new StringBuilder();
        private readonly List<string[]> _lines = new List<string[]>();
        private readonly List<string> _currentLine = new List<string>();

        public void AddChar(char ch)
        {
            _currentValue.Append(ch);
        }

        public void AddValue()
        {
            _currentLine.Add(_currentValue.ToString());
            _currentValue.Remove(0, _currentValue.Length);
        }

        public void AddLine()
        {
            _lines.Add(_currentLine.ToArray());
            _currentLine.Clear();
        }

        public List<string[]> GetAllLines()
        {
            if (_currentValue.Length > 0)
            {
                AddValue();
            }
            if (_currentLine.Count > 0)
            {
                AddLine();
            }
            return _lines;
        }
    }
}
