using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // ʵ���Ƽ��Ľ��
    public class RealNameLimitEvent
    {
        public RealNameLimitType realNameLimitType;     // ��������
        public RealNameData realNameData;               // ʵ������Ϣ
        public string describ = "";                     // ����

        public RealNameLimitEvent(RealNameData l_realNameData, string l_describ = "")
        {
            realNameData = l_realNameData;
            describ = l_describ;
            realNameLimitType = GetLimitResult(realNameData);
        }

        private RealNameLimitType GetLimitResult(RealNameData l_realNameData)
        {
            if (!l_realNameData.canPlay) // ��ֹ��������
            {
                if (l_realNameData.realNameStatus == RealNameStatus.NotRealName)
                {
                    if (string.IsNullOrEmpty(describ))
                    {
                        describ = "������ط��ɷ��棬δʵ�������ÿ��ֻ������1��СʱŶ����ǰ��ʵ����֤��";
                    }
                    // δʵ���ƣ���Ϸ��������1Сʱ
                    return RealNameLimitType.NoRealNameMaxTimeLimit;
                }
                else if (!l_realNameData.isAdult)
                {
                    if (l_realNameData.isNight)
                    {
                        if (string.IsNullOrEmpty(describ))
                        {
                            describ = "����10����˾�Ҫ��Ϣ��������������8��֮��������";
                        }
                        // ��ҹ�� 22ʱ������8ʱ ����Ϊδ�������ṩ��Ϸ����
                        return RealNameLimitType.ChildNightLimit;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(describ))
                        {
                            describ = "�����������ʱ���Ѿ�������1.5СʱŶ���ڼ���3Сʱ����������������";
                        }
                        // δ�����ˣ�ÿ������ʱ�����ó���xСʱ�������ڼ���3Сʱ����������1.5Сʱ��
                        return RealNameLimitType.ChildTimeLimit;
                    }
                }
                else
                {
                    Debug.LogError("GetLimitResult error�� adult:" + l_realNameData.isAdult);
                    return RealNameLimitType.NoLimit;
                }
            }
            else
            {
                return RealNameLimitType.NoLimit;// �����棬��ʾδ������
            }
        }

        static public void Dispatch(int l_onlineTime, bool l_isNight, bool l_canPlay, RealNameStatus l_realNameStatus, bool l_isAdult)
        {
            RealNameData realNameData = new RealNameData(l_canPlay, l_realNameStatus, l_isAdult, l_onlineTime, l_isNight);
            GlobalEvent.DispatchTypeEvent(new RealNameLimitEvent(realNameData));
        }
    }
}