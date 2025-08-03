using UnityEngine;

namespace CizaTable
{
	public static class WebUtils
	{
		private const string GOOGLE_SCRIPT_URL = "https://www.google.com/script/start/";
		private const string GOOGLE_SPREADSHEET_URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/edit#gid=0";

		public static void OpenGoogleScriptPage() => 
			Application.OpenURL(GOOGLE_SCRIPT_URL);


		public static void OpenGoogleSpreadSheetUrl(string spreadsheetId) => 
			Application.OpenURL(string.Format(GOOGLE_SPREADSHEET_URL_FORMAT, spreadsheetId));
	}
}