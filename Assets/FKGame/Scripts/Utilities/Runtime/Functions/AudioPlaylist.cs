using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
// 音乐列表
//------------------------------------------------------------------------
namespace FKGame
{
    [CreateAssetMenu(fileName = "Audio Playlist", menuName = "FKGame/Utilities/Audio Playlist")]
    [System.Serializable]
    public class AudioPlaylist : ScriptableObject
    {
        [SerializeField]
        protected List<AudioClip> m_Clips;

        public AudioClip this[int index]
        {
            get { return this.m_Clips[index]; }
        }

        public int Count
        {
            get
            {
                return this.m_Clips.Count;
            }
        }

    }
}