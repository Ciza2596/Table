using System.Collections.Generic;
using UnityEngine;

namespace GoogleHelper
{
    public struct RequestURL
    {
        //private variable
        private string _requestAction;
        private Dictionary<string, string> _parameters;
        
        private const string _devToken = "";
        
        
        //public variable
        public string ServiceURL { get; }

        
        //constructor
        public RequestURL(string service, string requestAction, Dictionary<string, string> requestParameters = null)
        {
            ServiceURL = service;
            _requestAction = requestAction;
            _parameters = requestParameters;
        }

        
        //public method
        public string GetWebDoGetURL()
        {
            string request = "";

            if (string.IsNullOrEmpty(ServiceURL) || string.IsNullOrEmpty(_requestAction))
                return request;

            request =
                $"{ServiceURL}?action={_requestAction}{GetParamString()}{(!string.IsNullOrEmpty(_devToken) ? $"&access_token={_devToken}" : "")}";
            return request;
        }

        public WWWForm GetPostData()
        {
            WWWForm form = new WWWForm();
            form.AddField("action", _requestAction);
            foreach (var item in _parameters)
            {
                form.AddField(item.Key, item.Value);
            }

            return form;
        }

        private string GetParamString()
        {
            string result = "";
            if (_parameters != null)
            {
                foreach (var item in _parameters)
                {
                    result += $"&{item.Key}={item.Value}";
                }
            }

            return result;
        }
    }
}