using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace GoogleHelper
{
    public class GoogleHelper
    {
        public async Task<string> StartDownload(RequestURL requestURL)
        {
            var request = requestURL.GetWebDoGetURL();
            var result = string.Empty;

            if (!string.IsNullOrEmpty(request))
            {
                var getRequest = UnityWebRequest.Get(request);

                Debug.Log("[GoogleHelper:StartDownload] Start download data from google.");
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
                            Debug.LogError("GoogleHelper:StartDownload] Nothing to download.");
                        }
                        Debug.Log("[GoogleHelper:StartDownload] Download is done.");
                    }
                    else
                    {
                        Debug.Log("GoogleHelper:StartDownload] Unable to access google : " + error);
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

                Debug.Log("[GoogleHelper:StartPost] Start post data to google.");
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
                            Debug.LogError("GoogleHelper:StartPost] Nothing to download.");
                        }
                        Debug.Log("[GoogleHelper:StartPost] Post work is done.");
                    }
                    else
                    {
                        Debug.Log("GoogleHelper:StartPost] Unable to access google : " + error);
                    }
                    return result;
                }
            }
            return result;
        }
    }
}