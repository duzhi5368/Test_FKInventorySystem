using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SFXRandomLoopData
    {
        public int loopTimes = -1;                                      // ѭ��������-1���޴�
        public Vector2 delayRange;                                      // ����С�����ʱ���������
        public List<SFXPlayData> SFXDatas = new List<SFXPlayData>();
    }
}