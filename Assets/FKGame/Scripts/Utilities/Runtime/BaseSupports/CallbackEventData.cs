using System.Collections.Generic;
//------------------------------------------------------------------------
// 回调事件的参数
//------------------------------------------------------------------------
namespace FKGame
{
	public class CallbackEventData {
		private Dictionary<string, object> properties;

		public CallbackEventData() {
			properties = new Dictionary<string, object>();
		}

		public void AddData(string key, object value) {
			if (properties.ContainsKey(key))
			{
				properties[key] = value;
			}else {
				properties.Add(key, value);
			}
		}

		public object GetData(string key) {
			if (properties.ContainsKey(key))
			{
				return properties[key];
			}else {
				return null;
			}
		}
	}
}