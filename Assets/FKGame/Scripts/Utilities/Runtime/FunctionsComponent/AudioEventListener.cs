using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//------------------------------------------------------------------------
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioEventListener : MonoBehaviour
    {
        [SerializeField]
        private List<AudioGroup> m_AudioGroups = new List<AudioGroup>();

        private void Awake()
        {
            for (int i = 0; i < this.m_AudioGroups.Count; i++) {
                AudioGroup group = this.m_AudioGroups[i];
                group.audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void PlayAudio(AnimationEvent evt) {
            AudioGroup group = this.m_AudioGroups.First(x => x.name == evt.stringParameter);
            group.PlayOneShot(evt.objectReferenceParameter as AudioClip, evt.floatParameter);
        }
    }
}