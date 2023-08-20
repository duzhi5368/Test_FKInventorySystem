﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FKGame
{
    public class InternalTestingTools 
    {
        [MenuItem("Tools/FKGame/Internal/Delete PlayerPrefs")]
        public static void DeletePlayerPrefs() {
            PlayerPrefs.DeleteAll();
        }
        [MenuItem("Tools/FKGame/Internal/Delete EditorPrefs")]
        public static void DeleteEditorPrefs()
        {
            EditorPrefs.DeleteAll();
        }

        [MenuItem("Tools/FKGame/Internal/Recompile Scripts")]
        public static void RecompileScripts()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}