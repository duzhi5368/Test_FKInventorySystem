using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class MusicRandomLoopData
    {
        public int loopTimes = -1;                                          // ѭ��������-1���޴�(˳�򲥷�ʱ������)
        public bool isRandom = false;                                       // �Ƿ�������ţ�false��˳�򲥷�
        public List<MusicPlayData> musicDatas = new List<MusicPlayData>();
    }
}