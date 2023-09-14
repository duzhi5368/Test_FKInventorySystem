using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ���ƹ�����Ϣ
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

        // ��ù�����Ϣ����
        public static List<AnnouncementContent2Client> GetMessageCache()
        {
            return messageCache;
        }

        public static void AddCache(AnnouncementContent2Client e)
        {
            messageCache.Add(e);
        }

        // �������
        public static void ClearCache()
        {
            messageCache.Clear();
        }

        // ȷ�����Ķ�������Ϣ
        public static void ConfirmMessage(string id, string useTag)
        {
            AnnouncementConfirm2Server msg = new AnnouncementConfirm2Server();
            msg.id = id;
            msg.useTag = useTag;
            JsonMessageProcessingController.SendMessage(msg);
        }
    }
}