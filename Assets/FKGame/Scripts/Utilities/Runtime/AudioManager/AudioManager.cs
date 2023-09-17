using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioManager : MonoBehaviour
    {
        public static Audio2DPlayer audio2DPlayer;
        public static Audio3DPlayer audio3DPlayer;
        // 音乐播放完成 回调（参数 ：资源名，channel，flag（标识：用于在多个相同音频名称时分辨））
        public static CallBack<string, int, string> OnMusicStopCallBack;
        // 音乐播放将要完成 回调，提前1秒回调，当clip时长不足1秒则在OnMusicStopCallBack前回调（参数 ：资源名，channel，flag（标识：用于在多个相同音频名称时分辨））
        public static CallBack<string, int, string> OnMusicPreStopCallBack;
        // SFX播放完成 回调（参数 ：资源名，flag（标识：用于在多个相同音频名称时分辨））
        public static CallBack<string, string> OnSFXStopCallBack;

        public static void Init()
        {
            GameObject obj = new GameObject("[AudioManager]");
            AudioManager audioManager = obj.AddComponent<AudioManager>();
            DontDestroyOnLoad(obj);

            audio2DPlayer = new Audio2DPlayer(audioManager);
            audio3DPlayer = new Audio3DPlayer(audioManager);
            TotleVolume = RecordManager.GetFloatRecord("GameSettingData", "TotleVolume", 1f);
            MusicVolume = RecordManager.GetFloatRecord("GameSettingData", "MusicVolume", 1f);
            SFXVolume = RecordManager.GetFloatRecord("GameSettingData", "SFXVolume", 1f);
        }


        private static float totleVolume = 1f;
        public static float TotleVolume
        {
            get { return totleVolume; }
            set
            {
                totleVolume = Mathf.Clamp01(value);
                SetMusicVolume();
                SetSFXVolume();

            }
        }

        private static float musicVolume = 1f;
        public static float MusicVolume
        {
            get{return musicVolume;}
            set
            {
                musicVolume = Mathf.Clamp01(value);
                SetMusicVolume();
            }
        }

        private static float sfxVolume = 1f;
        public static float SFXVolume
        {
            get{return sfxVolume;}
            set
            {
                sfxVolume = Mathf.Clamp01(value);
                SetSFXVolume();
            }
        }

        private static void SetMusicVolume()
        {
            audio2DPlayer.SetMusicVolume(totleVolume * musicVolume);
            audio3DPlayer.SetMusicVolume(totleVolume * musicVolume);
        }
        private static void SetSFXVolume()
        {
            audio2DPlayer.SetSFXVolume(totleVolume * sfxVolume);
            audio3DPlayer.SetSFXVolume(totleVolume * sfxVolume);
        }

        public static void SaveVolume()
        {
            RecordManager.SaveRecord("GameSettingData", "TotleVolume", TotleVolume);
            RecordManager.SaveRecord("GameSettingData", "MusicVolume", MusicVolume);
            RecordManager.SaveRecord("GameSettingData", "SFXVolume", SFXVolume);
        }

        public static AudioAsset PlayMusic2D(string name, int channel, float volumeScale = 1, bool isLoop = true, float fadeTime = 0.5f, float delay = 0f, string flag = "")
        {
            return audio2DPlayer.PlayMusic(channel, name, isLoop, volumeScale, delay, fadeTime, flag);
        }

        public static void PauseMusic2D(int channel, bool isPause, float fadeTime = 0.5f)
        {
            audio2DPlayer.PauseMusic(channel, isPause, fadeTime);
        }

        public static void PauseMusicAll2D(bool isPause, float fadeTime = 0.5f)
        {
            audio2DPlayer.PauseMusicAll(isPause, fadeTime);
        }

        public static void StopMusic2D(int channel, float fadeTime = 0.5f)
        {

            audio2DPlayer.StopMusic(channel, fadeTime);
        }

        public static void StopMusicAll2D()
        {
            audio2DPlayer.StopMusicAll();
        }

        public static void PlaySFX2D(string name, float volumeScale = 1f, float delay = 0f, float pitch = 1, string flag = "")
        {
            audio2DPlayer.PlaySFX(name, volumeScale, delay, pitch, flag);
        }
        public static void PauseSFXAll2D(bool isPause)
        {
            audio2DPlayer.PauseSFXAll(isPause);
        }

        public static AudioAsset PlayMusic3D(GameObject owner, string audioName, int channel = 0, float volumeScale = 1, 
            bool isLoop = true, float fadeTime = 0.5f, float delay = 0f, string flag = "")
        {
            return audio3DPlayer.PlayMusic(owner, audioName, channel, isLoop, volumeScale, delay, fadeTime, flag);
        }

        public static void PauseMusic3D(GameObject owner, int channel, bool isPause, float fadeTime = 0.5f)
        {
            audio3DPlayer.PauseMusic(owner, channel, isPause, fadeTime);
        }

        public static void PauseMusicAll3D(bool isPause, float fadeTime = 0.5f)
        {
            audio3DPlayer.PauseMusicAll(isPause, fadeTime);
        }

        public static void StopMusic3D(GameObject owner, int channel, float fadeTime = 0.5f)
        {
            audio3DPlayer.StopMusic(owner, channel, fadeTime);

        }
        public static void StopMusicOneAll3D(GameObject owner)
        {
            audio3DPlayer.StopMusicOneAll(owner);
        }
        public static void StopMusicAll3D()
        {
            audio3DPlayer.StopMusicAll();
        }
        public static void ReleaseMusic3D(GameObject owner)
        {
            audio3DPlayer.ReleaseMusic(owner);
        }
        public static void ReleaseMusicAll3D()
        {
            audio3DPlayer.ReleaseMusicAll();
        }

        public static void PlaySFX3D(GameObject owner, string name, float delay = 0f, float volumeScale = 1f)
        {
            audio3DPlayer.PlaySFX(owner, name, volumeScale, delay);
        }
        public static void PlaySFX3D(Vector3 position, string name, float delay = 0f, float volumeScale = 1)
        {
            audio3DPlayer.PlaySFX(position, name, volumeScale, delay);
        }

        public static void PauseSFXAll3D(bool isPause)
        {
            audio3DPlayer.PauseSFXAll(isPause);
        }
        public static void ReleaseSFX3D(GameObject owner)
        {
            audio3DPlayer.ReleaseSFX(owner);
        }
        public static void ReleaseSFXAll3D()
        {
            audio3DPlayer.ReleaseSFXAll();
        }

        void Update()
        {
            audio3DPlayer.ClearDestroyObjectData();
            audio2DPlayer.ClearMoreAudioAsset();

            audio2DPlayer.UpdateFade();
            audio3DPlayer.UpdateFade();
        }
    }

}