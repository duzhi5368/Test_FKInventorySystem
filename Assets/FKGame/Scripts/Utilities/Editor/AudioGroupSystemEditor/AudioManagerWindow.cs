using FKGame.Macro;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AudioManagerWindow : EditorWindow
    {
        private bool changedTime = false;
        private int toolbarOption = 0;
        private string[] toolbarTexts = { "2D Player", "3D Player" };

        [MenuItem("Tools/FKGame/基础支持/声音音效管理器", priority = 1002)]
        private static void OpenWindow()
        {
            AudioManagerWindow window = GetWindow<AudioManagerWindow>();
            window.autoRepaintOnSceneChange = true;
            window.wantsMouseMove = true;
            FocusWindowIfItsOpen<AudioManagerWindow>();
            Vector2 size = new Vector2(380f, 72f);
            window.minSize = size;
            window.titleContent = new GUIContent(LanguagesMacro.AUDIO_MANAGER_WINDOW_TITLE);
            window.Init();
        }

        private void Init()
        {

        }

        private void OnGUI()
        {
            toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts, GUILayout.Width(Screen.width));
            if (!Application.isPlaying)
                return;
            switch (toolbarOption)
            {
                case 0:
                    A2DPlayerGUI();
                    break;
                case 1:
                    A3DPlayerGUI();
                    break;
            }
        }

        private void A3DPlayerGUI()
        {

        }

        private void A2DPlayerGUI()
        {
            if(AudioManager.audio2DPlayer == null)
            {
                return;
            }
            Dictionary<int, AudioAsset> bgMusicDic = AudioManager.audio2DPlayer.bgMusicDic;
            EditorGUILayout.Slider("音乐音量 : ", AudioManager.audio2DPlayer.MusicVolume, 0, 1);
            EditorGUILayout.Slider("音效音量 : ", AudioManager.audio2DPlayer.SFXVolume, 0, 1);
            EditorDrawGUIUtil.DrawFoldout(bgMusicDic, "音乐总数:" + bgMusicDic.Count, () =>
            {
                EditorDrawGUIUtil.DrawScrollView(bgMusicDic, () =>
                {
                    foreach (var item in bgMusicDic)
                    {
                        GUILayout.Label("Channel : " + item.Key);
                        ShowAudioAssetGUI(item.Value, false);
                    }
                }, "box");
            });
            List<AudioAsset> sfxList = AudioManager.audio2DPlayer.sfxList;
            EditorDrawGUIUtil.DrawFoldout(sfxList, "音效总数:" + sfxList.Count, () =>
            {
                EditorDrawGUIUtil.DrawScrollView(sfxList, () =>
                {
                    for (int i = 0; i < sfxList.Count; i++)
                    {
                        AudioAsset au = sfxList[i];
                        GUILayout.Label("Item : " + i);
                        ShowAudioAssetGUI(au, false);
                    }
                }, "box");
            });

        }
        
        private void ShowAudioAssetGUI(AudioAsset au, bool isShowAudioSource)
        {
            Color color = Color.white;
            switch (au.PlayState)
            {
                case AudioPlayState.Playing:
                    color = Color.green;
                    break;
                case AudioPlayState.Pause:
                    color = Color.yellow;
                    break;
                case AudioPlayState.Stop:
                    break;

            }
            GUI.color = color;
            GUILayout.BeginVertical("box");
            GUILayout.Label("Asset Name : " + au.AssetName);
            GUILayout.Label("Play State : " + au.PlayState);
            GUILayout.Label("Loop : " + au.audioSource.loop);
            GUILayout.Label("flag : " + au.flag);
            EditorGUILayout.Slider("VolumeScale : ", au.VolumeScale, 0, 1);
            EditorGUILayout.Slider("Volume : ", au.GetMaxRealVolume(), 0, 1f);
            changedTime = GUILayout.Toggle(changedTime, "Change Time");
            GUILayout.BeginHorizontal();
            if (au.audioSource != null && au.audioSource.clip != null)
            {
                float value = EditorGUILayout.Slider("Time:" + au.audioSource.clip.length, au.audioSource.time, 0, au.audioSource.clip.length);
                if (changedTime)
                {
                    au.audioSource.time = value;
                }
            }
            GUILayout.EndHorizontal();
            if (isShowAudioSource)
                EditorGUILayout.ObjectField("AudioSource : ", au.audioSource, typeof(AudioSource), true);
            GUILayout.EndVertical();
            GUI.color = Color.white;
        }
    }
}