using System.Collections.Generic;
using UnityEngine;

namespace GoogleHelper
{
	public struct RequestURL
	{
		//private variable
		private readonly string                     _action;
		private readonly Dictionary<string, string> _parameters;

		private const string DEV_TOKEN = "";

		//public variable
		public string ServiceURL { get; }

		//constructor
		public RequestURL(string service, string action, Dictionary<string, string> parameters = null)
		{
			ServiceURL  = service;
			_action     = action;
			_parameters = parameters;
		}

		//public method
		public string GetWebDoGetURL()
		{
			string request = "";

			if (string.IsNullOrEmpty(ServiceURL) || string.IsNullOrEmpty(_action))
				return request;

			request =
				$"{ServiceURL}?action={_action}{GetParamString()}{(!string.IsNullOrEmpty(DEV_TOKEN) ? $"&access_token={DEV_TOKEN}" : "")}";
			return request;
		}

		public WWWForm GetPostData()
		{
			WWWForm form = new WWWForm();
			form.AddField("action", _action);
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
