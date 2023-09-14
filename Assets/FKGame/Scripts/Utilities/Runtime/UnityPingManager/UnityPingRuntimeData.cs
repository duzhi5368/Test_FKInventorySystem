using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UnityPingRuntimeData
    {
        public string host;
        public string ip;
        public float s_timeout = 2;
        public bool isFinish = false;

        public float currentUseTime = 0;
        public float delayStartTime = 0;
        public ErrorReason errorReason = ErrorReason.None;
        public Ping ping;
    }
}