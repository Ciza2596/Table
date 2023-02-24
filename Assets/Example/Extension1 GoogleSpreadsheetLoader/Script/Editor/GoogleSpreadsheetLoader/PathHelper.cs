
namespace GoogleSpreadsheetLoader.Editor
{
    public class PathHelper
    {
        public static string GetFullPath(string assetPath, string name, string folderPath = null)
        {
            var fileName = $"{name}.asset";

            if (folderPath != null)
                folderPath += '/';

            return assetPath + '/' + folderPath + fileName;
        }
    }
}
