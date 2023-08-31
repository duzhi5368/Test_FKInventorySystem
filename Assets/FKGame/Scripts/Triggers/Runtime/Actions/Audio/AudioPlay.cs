using UnityEngine;
using UnityEngine.Audio;
//------------------------------------------------------------------------
namespace FKGame
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [ComponentMenu("Audio/Play")]
    public class AudioPlay : Action
    {
        [SerializeField]
        private AudioClip m_Clip = null;
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;
        [SerializeField]
        private float m_Volume = 0.4f;

        public override ActionStatus OnUpdate()
        {
            UnityTools.PlaySound(this.m_Clip, this.m_Volume,this.m_AudioMixerGroup);
            return ActionStatus.Success;
        }
    }
}
