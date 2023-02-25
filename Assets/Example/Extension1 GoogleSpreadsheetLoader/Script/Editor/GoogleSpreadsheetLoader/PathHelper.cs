namespace GoogleSpreadsheetLoader.Editor
{
    public class PathHelper
    {
        public static string GetFullPath(string assetPath, string name, string folderName = null)
        {
            var fileName = $"{name}.asset";

            var folderPath = GetFolderPath(assetPath, folderName);

            return folderPath + '/' + fileName;
        }

        public static string GetFolderPath(string assetPath, string folderName = null)
        {
            if (folderName != null)
                folderName = '/' + folderName;
            
            return assetPath + folderName;
        }
    }
}