using System.Collections.Generic;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioGroupData
    {
        public string keyName = "";                                                         // 启动的key
        public string description = "";                                                     // 描述信息
        public List<MusicPlayData> fixedMusicDatas = new List<MusicPlayData>();             // 固定循环的音乐
        public List<MusicRandomLoopData> loopMusicDatas = new List<MusicRandomLoopData>();
        public List<SFXPlayData> fixedSFXDatas = new List<SFXPlayData>();                   // 固定播放一次的音效
        public List<SFXRandomLoopData> sFXRandomLoopDatas = new List<SFXRandomLoopData>();  // 随机时间触发循环音效
    }
}