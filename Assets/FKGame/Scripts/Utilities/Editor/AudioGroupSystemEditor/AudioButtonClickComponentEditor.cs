using UnityEditor;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    [CustomEditor(typeof(ComponentAudioButton))]
    public class AudioButtonClickComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ComponentAudioButton component = (ComponentAudioButton)target;
            if (string.IsNullOrEmpty(component.audioName))
            {
                EditorGUILayout.HelpBox("不能为空!!!", MessageType.Error);
                return;
            }
            if (!ResourcesConfigManager.IsResourceExist(component.audioName))
            {
                EditorGUILayout.HelpBox("没有资源!!!", MessageType.Error);
                return;
            }
            if (GUILayout.Button("Play", GUILayout.Height(60)))
            {
                AudioClip clip = ResourceManager.Load<AudioClip>(component.audioName);
                AudioEditorUtils.PlayClip(clip);
            }
        }
    }
}