using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace GoogleHelper
{
    public class GoogleHelper
    {
        private bool _isPrintLog = false;
        private bool _isPrintErrorLog = true;

        public void SetIsPrintLog(bool isPrintLog) => _isPrintLog = isPrintLog;
        public void SetIsPrintErrorLog(bool isPrintErrorLog) => _isPrintErrorLog = isPrintErrorLog;

        public async Task<string> StartDownload(RequestURL requestURL)
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

        public async Task<string> StartPost(RequestURL requestURL)
        {
            var request = requestURL.ServiceURL;
            var result = "";

            if (!string.IsNullOrEmpty(request))
            {
                var getRequest = UnityWebRequest.Post(request, requestURL.GetPostData());

                PrintLog("[GoogleHelper:StartPost] Start post data to google.");
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
                            PrintErrorLog("GoogleHelper:StartPost] Nothing to download.");
                        }

                        PrintLog("[GoogleHelper:StartPost] Post work is done.");
                    }
                    else
                    {
                        PrintErrorLog("GoogleHelper:StartPost] Unable to access google : " + error);
                    }

                    return result;
                }
            }

            return result;
        }


        private void PrintLog(string message)
        {
            if (_isPrintLog)
                Debug.Log(message);
        }


        private void PrintErrorLog(string message)
        {
            if (_isPrintLog)
                Debug.LogError(message);
        }
    }
}