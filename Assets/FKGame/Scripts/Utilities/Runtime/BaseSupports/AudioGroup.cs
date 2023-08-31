using UnityEngine;
using UnityEngine.Audio;
//------------------------------------------------------------------------
//------------------------------------------------------------------------
namespace FKGame
{
    [System.Serializable]
    public class AudioGroup
    {
        public string name = "SFX";
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;
        private AudioSource m_AudioSource;

        public AudioSource audioSource
        {
            get { return this.m_AudioSource; }
            set
            {
                this.m_AudioSource = value;
                this.m_AudioSource.outputAudioMixerGroup = this.m_AudioMixerGroup;
                this.m_AudioSource.spatialBlend = 1f;
            }
        }

        public void PlayOneShot(AudioClip clip, float volumeScale)
        {
            audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}
