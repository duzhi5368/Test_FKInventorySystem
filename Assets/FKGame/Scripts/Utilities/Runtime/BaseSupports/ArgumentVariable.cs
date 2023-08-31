using FKGame.Macro;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public enum ArgumentType
    {
        None,
        Bool,
        Int,
        Float,
        String,
        Vector2,
        Vector3,
        Color,
        Object
    }

    [System.Serializable]
    public class ArgumentVariable
    {
        [InspectorLabel(LanguagesMacro.BOOL_VALUE)]
        [SerializeField]
        private bool m_BoolValue = false;
        [InspectorLabel(LanguagesMacro.INT_VALUE)]
        [SerializeField]
        private int m_IntValue = 0;
        [InspectorLabel(LanguagesMacro.FLOAT_VALUE)]
        [SerializeField]
        private float m_FloatValue = 0f;
        [InspectorLabel(LanguagesMacro.STRING_VALUE)]
        [SerializeField]
        private string m_StringValue = string.Empty;
        [InspectorLabel(LanguagesMacro.VECTOR2_VALUE)]
        [SerializeField]
        private Vector2 m_Vector2Value = Vector2.zero;
        [InspectorLabel(LanguagesMacro.VECTOR3_VALUE)]
        [SerializeField]
        private Vector3 m_Vector3Value= Vector3.zero;
        [InspectorLabel(LanguagesMacro.COLOR_VALUE)]
        [SerializeField]
        private Color m_ColorValue=Color.white;
        [InspectorLabel(LanguagesMacro.OBJECT_VALUE)]
        [SerializeField]
        private Object m_ObjectValue = null;

        [SerializeField]
        private ArgumentType m_ArgumentType = ArgumentType.None;

        public ArgumentType ArgumentType {
            get { return this.m_ArgumentType; }
            set { this.m_ArgumentType = value; }
        }

        public bool IsNone {
            get { return this.m_ArgumentType == ArgumentType.None; }
        }

        public object GetValue() {
            switch (this.m_ArgumentType) {
                case ArgumentType.Bool:
                    return this.m_BoolValue;
                case ArgumentType.Int:
                    return this.m_IntValue;
                case ArgumentType.Float:
                    return this.m_FloatValue;
                case ArgumentType.String:
                    return this.m_StringValue;
                case ArgumentType.Vector2:
                    return this.m_Vector2Value;
                case ArgumentType.Vector3:
                    return this.m_Vector3Value;
                case ArgumentType.Color:
                    return this.m_ColorValue;
                case ArgumentType.Object:
                    return this.m_ObjectValue;
                default:
                    return null;
            }
        }

        public static string GetPropertyValuePath(ArgumentType argumentType) {
            switch (argumentType)
            {
                case ArgumentType.Bool:
                    return "m_BoolValue";
                case ArgumentType.Int:
                    return "m_IntValue";
                case ArgumentType.Float:
                    return "m_FloatValue";
                case ArgumentType.String:
                    return "m_StringValue";
                case ArgumentType.Vector2:
                    return "m_Vector2Value";
                case ArgumentType.Vector3:
                    return "m_Vector3Value";
                case ArgumentType.Color:
                    return "m_ColorValue";
                case ArgumentType.Object:
                    return "m_ObjectValue";
                default:
                    return string.Empty;
            }
        }
    }
}