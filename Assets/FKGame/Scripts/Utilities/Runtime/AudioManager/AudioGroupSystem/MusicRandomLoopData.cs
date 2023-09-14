using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class MusicRandomLoopData
    {
        public int loopTimes = -1;                                          // 循环次数，-1无限次(顺序播放时起作用)
        public bool isRandom = false;                                       // 是否随机播放，false：顺序播放
        public List<MusicPlayData> musicDatas = new List<MusicPlayData>();
    }
}