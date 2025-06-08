using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace GoogleSpreadsheetLoader.Editor
{
	public class GoogleHelper
	{
		public virtual bool IsPrintLog { get; protected set; }

		[Preserve]
		public GoogleHelper(bool isPrintLog) =>
			SetIsPrintLog(isPrintLog);

		public virtual void SetIsPrintLog(bool isPrintLog) => IsPrintLog = isPrintLog;

		public virtual async Task<string> StartDownload(RequestURL requestURL)
		{
			var request = requestURL.GetWebDoGetURL();
			var result = string.Empty;

			if (!string.IsNullOrEmpty(request))
			{
				var getRequest = UnityWebRequest.Get(request);

				PrintLog("[GoogleHelper:StartDownload] Start download data from google.");
#if UNITY_2017_2_OR_NEWER
				await getRequest.SendWebRequest();
#else
                await getRequest.Send();
#endif

				if (getRequest.isDone)
				{
					var error = getRequest.error;

					if (string.IsNullOrEmpty(error))
					{
						result = System.Text.Encoding.UTF8.GetString(getRequest.downloadHandler.data);
						var isEmpty = string.IsNullOrEmpty(result) || result == "\"\"";

						if (isEmpty)
						{
							result = string.Empty;
							PrintErrorLog("GoogleHelper:StartDownload] Nothing to download.");
						}

						PrintLog("[GoogleHelper:StartDownload] Download is done.");
					}
					else
					{
						PrintErrorLog("GoogleHelper:StartDownload] Unable to access google : " + error);
					}

					return result;
				}
			}

			return result;
		}

		protected virtual void PrintLog(string message)
		{
			if (IsPrintLog)
				Debug.Log(message);
		}

		protected virtual void PrintErrorLog(string message)
		{
			if (IsPrintLog)
				Debug.LogError(message);
		}
	}
}