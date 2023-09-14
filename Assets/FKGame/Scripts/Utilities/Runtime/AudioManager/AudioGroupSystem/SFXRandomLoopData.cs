using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class SFXRandomLoopData
    {
        public int loopTimes = -1;                                      // 循环次数，-1无限次
        public Vector2 delayRange;                                      // 在最小到最大时间间隔后随机
        public List<SFXPlayData> SFXDatas = new List<SFXPlayData>();
    }
}