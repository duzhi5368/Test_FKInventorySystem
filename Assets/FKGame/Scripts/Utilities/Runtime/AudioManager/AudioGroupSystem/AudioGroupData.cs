using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioGroupData
    {
        public string keyName = "";                                                         // ������key
        public string description = "";                                                     // ������Ϣ
        public List<MusicPlayData> fixedMusicDatas = new List<MusicPlayData>();             // �̶�ѭ��������
        public List<MusicRandomLoopData> loopMusicDatas = new List<MusicRandomLoopData>();
        public List<SFXPlayData> fixedSFXDatas = new List<SFXPlayData>();                   // �̶�����һ�ε���Ч
        public List<SFXRandomLoopData> sFXRandomLoopDatas = new List<SFXRandomLoopData>();  // ���ʱ�䴥��ѭ����Ч
    }
}