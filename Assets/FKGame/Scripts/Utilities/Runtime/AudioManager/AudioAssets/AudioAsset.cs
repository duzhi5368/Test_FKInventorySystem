using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioAsset
    {
        [NotJsonSerialized]
        public AudioSource audioSource;
        public AudioSourceType sourceType;
        public string flag = "";
        private string assetName = "";
        public int musicChannel = 0;
        private bool isCallPreStop;

        private float totleVolume = 1;      // 总音量
        public float TotleVolume
        {
            get
            {
                return totleVolume;
            }
            set
            {
                totleVolume = value;
                Volume = TotleVolume * volumeScale;
            }
        }

        // 当前AudioSource 实际音量
        public float Volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }

        // 实际音量恢复到当前的最大
        public void ResetVolume()
        {
            Volume = TotleVolume * volumeScale;
        }

        public float GetMaxRealVolume()
        {
            return TotleVolume * volumeScale;
        }

        // 相对于总音量当前当前AudioSource的音量缩放. Volume= TotleVolume * volumeScale
        private float volumeScale = 1f;
        public float VolumeScale
        {
            get { return volumeScale; }
            set
            {
                volumeScale = Mathf.Clamp01(value);
                ResetVolume();
            }
        }
        public bool IsPlay
        {
            get { return audioSource.isPlaying; }
        }

        private AudioPlayState playState = AudioPlayState.Stop;
        public AudioPlayState PlayState
        {
            get
            {
                return playState;
            }
        }

        public string AssetName
        {
            get
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    if (string.IsNullOrEmpty(assetName))
                    {
                        assetName = audioSource.clip.name;
                    }
                }
                return assetName;
            }
            set
            {
                assetName = value;
            }
        }

        public void SetPlayState(AudioPlayState state)
        {
            playState = state;
        }

        // 检查音频是否播放完成
        public void CheckState()
        {
            if (playState == AudioPlayState.Stop)
                return;
            if (audioSource.clip.length > 1 && audioSource.time >= (audioSource.clip.length - 1) && !isCallPreStop)
            {
                isCallPreStop = true;
                if (AudioManager.OnMusicPreStopCallBack != null)
                    AudioManager.OnMusicPreStopCallBack(AssetName, musicChannel, flag);
            }
            if (audioSource == null || (!audioSource.isPlaying && playState != AudioPlayState.Pause))
            {
                Stop();
            }
        }

        public void Play(float delay = 0f)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                isCallPreStop = false;
                audioSource.time = 0;
                audioSource.PlayDelayed(delay);
                playState = AudioPlayState.Playing;
            }
        }

        public void Pause()
        {
            if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
            {
                audioSource.Pause();
                playState = AudioPlayState.Pause;
            }
        }

        public void Stop()
        {
            if (audioSource)
                audioSource.Stop();
            playState = AudioPlayState.Stop;

            if (sourceType == AudioSourceType.Music)
            {
                if (!isCallPreStop)
                {
                    isCallPreStop = true;
                    if (AudioManager.OnMusicPreStopCallBack != null)
                        AudioManager.OnMusicPreStopCallBack(AssetName, musicChannel, flag);
                }

                if (AudioManager.OnMusicStopCallBack != null)
                    AudioManager.OnMusicStopCallBack(AssetName, musicChannel, flag);
            }
            else
            {
                if (AudioManager.OnSFXStopCallBack != null)
                    AudioManager.OnSFXStopCallBack(AssetName, flag);
            }
        }

        // 重置某些参数，防止回收后再使用参数不对
        public void ResetData()
        {
            AssetName = "";
            audioSource.pitch = 1;
            flag = "";
        }
    }
}