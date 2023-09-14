using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
// 部分便捷方法
//------------------------------------------------------------------------
namespace FKGame
{
    public static class UnityTools
    {
        // 协程处理器单例对象
        private static ComponentCoroutineHandler m_CoroutineHandler;
        private static ComponentCoroutineHandler Handler
        {
            get
            {
                if (m_CoroutineHandler == null)
                {
                    GameObject handlerObject = new GameObject("FK Coroutine Handler");
                    m_CoroutineHandler = handlerObject.AddComponent<ComponentCoroutineHandler>();
                }
                return m_CoroutineHandler;
            }
        }

        // 播放音频，如果没有播放器则动态创建一个
        private static AudioSource audioSource;
        public static void PlaySound(AudioClip clip, float volumeScale, AudioMixerGroup audioMixerGroup=null)
        {
            if (clip == null)
            {
                return;
            }
            if (audioSource == null)
            {
                AudioListener listener = GameObject.FindObjectOfType<AudioListener>();
                if (listener != null)
                {
                    audioSource = listener.GetComponent<AudioSource>();
                    if (audioSource == null)
                    {
                        audioSource = listener.gameObject.AddComponent<AudioSource>();
                    }
                }
            }
            if (audioSource != null)
            {
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.PlayOneShot(clip, volumeScale);
            }
        }

        // 判断一个点是否在UI上
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null || EventSystem.current.currentInputModule == null)
            {
                return false;
            }
            Type type = EventSystem.current.currentInputModule.GetType();
            MethodInfo methodInfo;
            methodInfo = type.GetMethod("GetLastPointerEventData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (methodInfo == null)
            {
                return false;
            }
            PointerEventData eventData = (PointerEventData)methodInfo.Invoke(EventSystem.current.currentInputModule, new object[] { PointerInputModule.kMouseLeftId });
            if (eventData != null && eventData.pointerEnter)
            {
                return eventData.pointerEnter.layer == 5;
            }
            return false;
        }

        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static string ColorString(string value, Color color)
        {
            return "<color=#" + UnityTools.ColorToHex(color) + ">" + value + "</color>";
        }

        // 检查对象是否是数字
        public static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            double number;
            return Double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
        }

        // 检查对象是否是整型
        public static bool IsInteger(Type value)
        {
            return (value == typeof(SByte) || value == typeof(Int16) || value == typeof(Int32)
                    || value == typeof(Int64) || value == typeof(Byte) || value == typeof(UInt16)
                    || value == typeof(UInt32) || value == typeof(UInt64));
        }

        // 检查对象是否是浮点数
        public static bool IsFloat(Type value)
        {
            return (value == typeof(float) | value == typeof(double) | value == typeof(Decimal));
        }

        // 根据名字查找子节点
        public static GameObject FindChild(this GameObject target, string name, bool includeInactive)
        {
            if (target != null)
            {
                if (target.name == name && includeInactive || target.name == name && !includeInactive && target.activeInHierarchy)
                {
                    return target;
                }
                for (int i = 0; i < target.transform.childCount; ++i)
                {
                    GameObject result = target.transform.GetChild(i).gameObject.FindChild(name, includeInactive);

                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public static void IgnoreCollision(GameObject gameObject1, GameObject gameObject2)
        {
            Collider collider = gameObject2.GetComponent<Collider>();
            if (collider == null) return;
            Collider[] colliders = gameObject1.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < colliders.Length; i++)
            {
                Physics.IgnoreCollision(colliders[i], collider);
            }
        }

        public static Bounds GetBounds(GameObject obj)
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds = renderer.bounds;
                        break;
                    }
                }
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }
            }
            return bounds;
        }

        public static void Swap<QT>(ref QT t1, ref QT t2)
        {

            QT temp = t1;
            t1 = t2;
            t2 = temp;
        }

        public static string KeyToCaption(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.None: return "None";
                case KeyCode.Backspace: return "Backspace";
                case KeyCode.Tab: return "Tab";
                case KeyCode.Clear: return "Clear";
                case KeyCode.Return: return "Return";
                case KeyCode.Pause: return "Pause";
                case KeyCode.Escape: return "Esc";
                case KeyCode.Space: return "Space";
                case KeyCode.Exclaim: return "!";
                case KeyCode.DoubleQuote: return "\"";
                case KeyCode.Hash: return "#";
                case KeyCode.Dollar: return "$";
                case KeyCode.Ampersand: return "&";
                case KeyCode.Quote: return "'";
                case KeyCode.LeftParen: return "(";
                case KeyCode.RightParen: return ")";
                case KeyCode.Asterisk: return "*";
                case KeyCode.Plus: return "+";
                case KeyCode.Comma: return ",";
                case KeyCode.Minus: return "-";
                case KeyCode.Period: return ".";
                case KeyCode.Slash: return "/";
                case KeyCode.Alpha0: return "0";
                case KeyCode.Alpha1: return "1";
                case KeyCode.Alpha2: return "2";
                case KeyCode.Alpha3: return "3";
                case KeyCode.Alpha4: return "4";
                case KeyCode.Alpha5: return "5";
                case KeyCode.Alpha6: return "6";
                case KeyCode.Alpha7: return "7";
                case KeyCode.Alpha8: return "8";
                case KeyCode.Alpha9: return "9";
                case KeyCode.Colon: return ":";
                case KeyCode.Semicolon: return ";";
                case KeyCode.Less: return "<";
                case KeyCode.Equals: return "=";
                case KeyCode.Greater: return ">";
                case KeyCode.Question: return "?";
                case KeyCode.At: return "@";
                case KeyCode.LeftBracket: return "[";
                case KeyCode.Backslash: return "\\";
                case KeyCode.RightBracket: return "]";
                case KeyCode.Caret: return "^";
                case KeyCode.Underscore: return "_";
                case KeyCode.BackQuote: return "`";
                case KeyCode.A: return "A";
                case KeyCode.B: return "B";
                case KeyCode.C: return "C";
                case KeyCode.D: return "D";
                case KeyCode.E: return "E";
                case KeyCode.F: return "F";
                case KeyCode.G: return "G";
                case KeyCode.H: return "H";
                case KeyCode.I: return "I";
                case KeyCode.J: return "J";
                case KeyCode.K: return "K";
                case KeyCode.L: return "L";
                case KeyCode.M: return "M";
                case KeyCode.N: return "N";
                case KeyCode.O: return "O";
                case KeyCode.P: return "P";
                case KeyCode.Q: return "Q";
                case KeyCode.R: return "R";
                case KeyCode.S: return "S";
                case KeyCode.T: return "T";
                case KeyCode.U: return "U";
                case KeyCode.V: return "V";
                case KeyCode.W: return "W";
                case KeyCode.X: return "X";
                case KeyCode.Y: return "Y";
                case KeyCode.Z: return "Z";
                case KeyCode.Delete: return "Del";
                case KeyCode.Keypad0: return "K0";
                case KeyCode.Keypad1: return "K1";
                case KeyCode.Keypad2: return "K2";
                case KeyCode.Keypad3: return "K3";
                case KeyCode.Keypad4: return "K4";
                case KeyCode.Keypad5: return "K5";
                case KeyCode.Keypad6: return "K6";
                case KeyCode.Keypad7: return "K7";
                case KeyCode.Keypad8: return "K8";
                case KeyCode.Keypad9: return "K9";
                case KeyCode.KeypadPeriod: return ".";
                case KeyCode.KeypadDivide: return "/";
                case KeyCode.KeypadMultiply: return "*";
                case KeyCode.KeypadMinus: return "-";
                case KeyCode.KeypadPlus: return "+";
                case KeyCode.KeypadEnter: return "NT";
                case KeyCode.KeypadEquals: return "=";
                case KeyCode.UpArrow: return "UP";
                case KeyCode.DownArrow: return "DN";
                case KeyCode.RightArrow: return "LT";
                case KeyCode.LeftArrow: return "RT";
                case KeyCode.Insert: return "Ins";
                case KeyCode.Home: return "Home";
                case KeyCode.End: return "End";
                case KeyCode.PageUp: return "PU";
                case KeyCode.PageDown: return "PD";
                case KeyCode.F1: return "F1";
                case KeyCode.F2: return "F2";
                case KeyCode.F3: return "F3";
                case KeyCode.F4: return "F4";
                case KeyCode.F5: return "F5";
                case KeyCode.F6: return "F6";
                case KeyCode.F7: return "F7";
                case KeyCode.F8: return "F8";
                case KeyCode.F9: return "F9";
                case KeyCode.F10: return "F10";
                case KeyCode.F11: return "F11";
                case KeyCode.F12: return "F12";
                case KeyCode.F13: return "F13";
                case KeyCode.F14: return "F14";
                case KeyCode.F15: return "F15";
                case KeyCode.Numlock: return "Num";
                case KeyCode.CapsLock: return "Caps Lock";
                case KeyCode.ScrollLock: return "Scr";
                case KeyCode.RightShift: return "Shift";
                case KeyCode.LeftShift: return "Shift";
                case KeyCode.RightControl: return "Control";
                case KeyCode.LeftControl: return "Control";
                case KeyCode.RightAlt: return "Alt";
                case KeyCode.LeftAlt: return "Alt";
                case KeyCode.AltGr: return "Alt";
                case KeyCode.Menu: return "Menu";
                case KeyCode.Mouse0: return "Mouse 0";
                case KeyCode.Mouse1: return "Mouse 1";
                case KeyCode.Mouse2: return "M2";
                case KeyCode.Mouse3: return "M3";
                case KeyCode.Mouse4: return "M4";
                case KeyCode.Mouse5: return "M5";
                case KeyCode.Mouse6: return "M6";
                case KeyCode.JoystickButton0: return "(A)";
                case KeyCode.JoystickButton1: return "(B)";
                case KeyCode.JoystickButton2: return "(X)";
                case KeyCode.JoystickButton3: return "(Y)";
                case KeyCode.JoystickButton4: return "(RB)";
                case KeyCode.JoystickButton5: return "(LB)";
                case KeyCode.JoystickButton6: return "(Back)";
                case KeyCode.JoystickButton7: return "(Start)";
                case KeyCode.JoystickButton8: return "(LS)";
                case KeyCode.JoystickButton9: return "(RS)";
                case KeyCode.JoystickButton10: return "J10";
                case KeyCode.JoystickButton11: return "J11";
                case KeyCode.JoystickButton12: return "J12";
                case KeyCode.JoystickButton13: return "J13";
                case KeyCode.JoystickButton14: return "J14";
                case KeyCode.JoystickButton15: return "J15";
                case KeyCode.JoystickButton16: return "J16";
                case KeyCode.JoystickButton17: return "J17";
                case KeyCode.JoystickButton18: return "J18";
                case KeyCode.JoystickButton19: return "J19";
            }
            return null;
        }

        private static void CheckIsEnum<T>(bool withFlags)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        public static bool HasFlag<T>(this T value, T flag) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return Handler.StartCoroutine(routine);
        }

        public static void StopCoroutine(IEnumerator routine)
        {
            Handler.StopCoroutine(routine);
        }

        public static void StopAllCoroutines()
        {
            Handler.StopAllCoroutines();
        }
    }
}