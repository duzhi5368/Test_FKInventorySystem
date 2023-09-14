using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 控制公告消息
    public static class AnnouncementController
    {
        public static CallBack<AnnouncementContent2Client> OnAnnouncementMessage;
        private static List<AnnouncementContent2Client> messageCache = new List<AnnouncementContent2Client>();

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            GlobalEvent.AddTypeEvent<AnnouncementContent2Client>(OnAnnouncement);
        }

        private static void OnAnnouncement(AnnouncementContent2Client e, object[] args)
        {
            if (OnAnnouncementMessage != null)
                OnAnnouncementMessage(e);
        }

        // 获得公告信息缓存
        public static List<AnnouncementContent2Client> GetMessageCache()
        {
            return messageCache;
        }

        public static void AddCache(AnnouncementContent2Client e)
        {
            messageCache.Add(e);
        }

        // 清除缓存
        public static void ClearCache()
        {
            messageCache.Clear();
        }

        // 确认已阅读公告信息
        public static void ConfirmMessage(string id, string useTag)
        {
            AnnouncementConfirm2Server msg = new AnnouncementConfirm2Server();
            msg.id = id;
            msg.useTag = useTag;
            JsonMessageProcessingController.SendMessage(msg);
        }
    }
}