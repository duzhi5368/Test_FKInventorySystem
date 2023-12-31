using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class EffectStopParticle : MonoBehaviour
    {
        public ParticleSystem particle;

        private void Awake()
        {
            if (particle == null)
            {
                particle = GetComponentInChildren<ParticleSystem>();
            }
        }


        private void OnEnable()
        {
            particle.Play(true);
        }

        private void OnDisable()
        {
            StopParticle();
        }

        public void StopParticle()
        {
            if (particle != null)
            {
                particle.Stop(true);
            }
        }
    }
}