using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AppInfoService : CustomServiceBase
    {
        public override string FunctionName
        {
            get
            {
                return "AppInfo";
            }
        }

        private static List<ShowInfoData> infoDatas = new List<ShowInfoData>();

        public override void OnStart(){}
        protected override void OnFunctionClose(){}
        protected override void OnFunctionOpen(){}
        protected override void OnPlayerLogin(Player player)
        {
            foreach (var item in infoDatas)
            {
                Send2Client(player, item);
            }
        }

        private static void Send2Client(Player player, ShowInfoData data)
        {
            if (NetServer.NetManager != null)
            {
                AppInfoData2Client msg = new AppInfoData2Client();
                msg.data = data;
                NetServer.NetManager.Send(player.session, msg);
            }
        }

        private static void Send2AllPlayer(ShowInfoData data)
        {
            Player[] players = PlayerManager.GetAllPlayers();
            foreach (var p in players)
            {
                Send2Client(p, data);
            }
        }

        private static ShowInfoData GetShowInfoData(string typeName, string label, string key)
        {
            foreach (var item in infoDatas)
            {
                if (item.typeName == typeName &&
                    item.label == label &&
                    item.key == key)
                {
                    return item;
                }
            }
            return null;
        }

        public static void AddInfoValue(string typeName, string label, string key, object value, string description = null)
        {
            try
            {
                if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(label) || string.IsNullOrEmpty(key))
                {
                    Debug.LogError("typeName or label or key cant be null");
                    return;
                }
                if (value == null)
                {
                    Debug.LogError("value cant be null!" + " typeName:" + typeName + " label:" + label + " key:" + key);
                    return;
                }
                ShowInfoData data = GetShowInfoData(typeName, label, key);
                string valueStr = JsonSerializer.ToJson(value);
                string valueTypeStr = value.GetType().FullName;
                bool isSend = false;
                if (data == null)
                {
                    data = new ShowInfoData();
                    data.typeName = typeName;
                    data.label = label;
                    data.key = key;
                    data.value = valueStr;
                    data.valueTypeStr = valueTypeStr;
                    data.discription = description;
                    infoDatas.Add(data);
                    isSend = true;
                }
                else
                {
                    if (data.valueTypeStr != valueTypeStr)
                    {
                        Debug.LogError(" Path:" + data.GetPath() + " already have value Type:" + data.valueTypeStr + " can not set Value Type:" + valueStr);
                        return;
                    }
                    else
                    {
                        if (data.value != valueStr)
                        {
                            data.value = valueStr;
                            isSend = true;
                        }
                        if (!string.IsNullOrEmpty(description) && data.discription != description)
                        {
                            data.discription = description;
                            isSend = true;
                        }
                    }
                }
                if (isSend)
                {
                    Send2AllPlayer(data);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}