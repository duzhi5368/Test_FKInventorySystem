using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class AnimManager : MonoBehaviour
    {
        static AnimManager instance;
        public static AnimManager GetInstance()
        {
            if (instance == null)
            {
                GameObject animGameObject = GameObject.Find("AnimManager");
                if (animGameObject == null)
                {
                    animGameObject = new GameObject();
                    animGameObject.name = "AnimManager";
                    instance = animGameObject.AddComponent<AnimManager>();
                }
                if (instance == null)
                    instance = animGameObject.GetComponent<AnimManager>();
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
                else
                {
#if UNITY_EDITOR
                    // ���ӱ�������ᵼ�´��ʧ��
                    EditorApplication.update += instance.Update;
#endif
                }
            }
            return instance;
        }

        public List<AnimData> animList = new List<AnimData>();
        public List<AnimData> removeList = new List<AnimData>();

        public void Update()
        {
            for (int i = 0; i < animList.Count; i++)
            {
                //ִ��Update
                bool isError = animList[i].ExecuteUpdate();
                if (isError)
                {
                    removeList.Add(animList[i]);
                    continue;
                }
                if (animList[i].m_isDone == true)
                {
                    AnimData animTmp = animList[i];
                    animTmp.ExecuteCallBack();      // ִ�лص�
                    if (!animTmp.AnimReplayLogic())
                    {
                        removeList.Add(animTmp);
                    }
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                animList.Remove(removeList[i]);
            }
            removeList.Clear();
        }

        /// <summary>
        /// ֹͣһ���������ϵ����ж���
        /// </summary>
        /// <param name="animGameObject">Ҫֹͣ�����Ķ���</param>
        /// <param name="isCallBack">�Ƿ񴥷��ص�</param>
        public static void StopAnim(GameObject animGameObject, bool isCallBack = false)
        {
            for (int i = 0; i < GetInstance().animList.Count; i++)
            {
                if (GetInstance().animList[i].m_animGameObejct == animGameObject)
                {
                    AnimData animData = GetInstance().animList[i];
                    if (isCallBack)
                    {
                        animData.ExecuteCallBack();
                    }
                    GetInstance().removeList.Add(animData);
                }
            }
            for (int i = 0; i < GetInstance().removeList.Count; i++)
            {
                GetInstance().animList.Remove(GetInstance().removeList[i]);
            }
            GetInstance().removeList.Clear();
        }

        /// <summary>
        /// ֹͣһ������
        /// </summary>
        /// <param name="animGameObject">Ҫֹͣ�Ķ���</param>
        /// <param name="isCallBack">�Ƿ񴥷��ص�</param>
        public static void StopAnim(AnimData animData, bool isCallBack = false)
        {
            if (GetInstance().animList.Contains(animData))
            {
                if (isCallBack)
                {
                    animData.ExecuteCallBack();
                }
                GetInstance().animList.Remove(animData);
            }
        }

        /// <summary>
        /// �������һ������
        /// </summary>
        /// <param name="animGameObject">Ҫ��ɵ�</param>
        public static void FinishAnim(AnimData animData)
        {
            animData.m_currentTime = animData.m_totalTime;
            animData.ExecuteUpdate();
            animData.ExecuteCallBack();
            GetInstance().animList.Remove(animData);
        }

        public static void ClearAllAnim(bool isCallBack = false)
        {
            for (int i = 0; i < GetInstance().animList.Count; i++)
            {
                AnimData animTmp = GetInstance().animList[i];
                if (isCallBack)
                {
                    animTmp.ExecuteCallBack();
                }
            }
            GetInstance().animList.Clear();
        }


        #region UGUI_Color
        /// <summary>
        /// �������ȵ�Ŀ����ɫ
        /// </summary>
        /// <param name="animObject">��������</param>
        /// <param name="from">��ʼ��ɫ(�ɿ�)</param>
        /// <param name="to">Ŀ����ɫ</param>
        /// <param name="time">����ʱ��</param>
        /// <param name="isChild">�Ƿ�Ӱ���ӽڵ�</param>
        /// <param name="interp">��ֵ����</param>
        /// <param name="IsIgnoreTimeScale">�Ƿ����ʱ������</param>
        /// <param name="repeatType">�ظ�����</param>
        /// <param name="repeatCount">�ظ�����</param>
        /// <param name="callBack">������ɻص�����</param>
        /// <param name="parameter">������ɻص���������</param>
        /// <returns></returns>
        public static AnimData UguiColor(GameObject animObject, Color? from, Color to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool isChild = true,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            Color fromTmp = from ?? Color.white;

            if (from == null)
            {
                if (animObject.GetComponent<Graphic>() != null)
                {
                    fromTmp = from ?? animObject.GetComponent<Graphic>().color;
                }
            }

            AnimData l_tmp = new AnimData();

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.UGUI_Color;
            l_tmp.m_fromColor = fromTmp;
            l_tmp.m_toColor = to;
            l_tmp.m_isChild = isChild;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region UGUI Alpha

        /// <summary>
        /// �������ȵ�Ŀ��alpha
        /// </summary>
        /// <param name="animObject">��������</param>
        /// <param name="from">��ʼalpha(�ɿ�)</param>
        /// <param name="to">Ŀ��alpha</param>
        /// <param name="time">����ʱ��</param>
        /// <param name="isChild">�Ƿ�Ӱ���ӽڵ�</param>
        /// <param name="interp">��ֵ����</param>
        /// <param name="IsIgnoreTimeScale">�Ƿ����ʱ������</param>
        /// <param name="repeatType">�ظ�����</param>
        /// <param name="repeatCount">�ظ�����</param>
        /// <param name="callBack">������ɻص�����</param>
        /// <param name="parameter">������ɻص���������</param>
        /// <returns></returns>
        public static AnimData UguiAlpha(GameObject animObject, float? from, float to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool isChild = true,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            float fromTmp = from ?? 1;

            if (from == null)
            {
                if (animObject.GetComponent<Graphic>() != null)
                {
                    fromTmp = from ?? animObject.GetComponent<Graphic>().color.a;
                }
            }

            AnimData l_tmp = new AnimData();

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.UGUI_Alpha;
            l_tmp.m_fromFloat = fromTmp;
            l_tmp.m_toFloat = to;
            l_tmp.m_isChild = isChild;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region UGUI Move

        public static AnimData UguiMove(GameObject animObject, Vector3? from, Vector3 to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null,
            object[] parameter = null)
        {

            Vector3 fromTmp = from ?? animObject.GetComponent<RectTransform>().anchoredPosition;

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.UGUI_AnchoredPosition;
            l_tmp.m_fromV3 = fromTmp;
            l_tmp.m_toV3 = to;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region UGUI_SizeDelta
        public static AnimData UguiSizeDelta(GameObject animObject, Vector2? from, Vector2 to,

            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null,
            object[] parameter = null)
        {
            Vector2 fromTmp = from ?? animObject.GetComponent<RectTransform>().sizeDelta;

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.UGUI_SizeDetal;
            l_tmp.m_fromV2 = fromTmp;
            l_tmp.m_toV2 = to;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region Color

        public static AnimData ColorTo(GameObject animObject, Color from, Color to,

            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool isChild = true,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null,
            object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.Color;
            l_tmp.m_fromColor = from;
            l_tmp.m_toColor = to;
            l_tmp.m_isChild = isChild;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData AlphaTo(GameObject animObject, float from, float to,

          float time = 0.5f,
          float delayTime = 0,
          InterpType interp = InterpType.Default,
          bool IsIgnoreTimeScale = false,
          RepeatType repeatType = RepeatType.Once,
          int repeatCount = -1,
          bool isChild = true,
          AnimCallBack callBack = null, object[] parameter = null)
        {

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.Alpha;
            l_tmp.m_fromFloat = from;
            l_tmp.m_toFloat = to;
            l_tmp.m_isChild = isChild;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region Move
        /// <summary>
        /// �����ƶ���ĳλ��
        /// </summary>
        /// <param name="animObject">��������</param>
        /// <param name="from">���λ��(�ɿգ���Ϊ����ӵ�ǰλ�ÿ�ʼ)</param>
        /// <param name="to">�յ�λ��</param>
        /// <param name="time">����ʱ��</param>
        /// <param name="isLocal">�Ƿ��������λ��</param>
        /// <param name="interp">��ֵ����</param>
        /// <param name="IsIgnoreTimeScale">�Ƿ����ʱ������</param>
        /// <param name="repeatType">�ظ�����</param>
        /// <param name="repeatCount">�ظ�����</param>
        /// <param name="callBack">������ɻص�����</param>
        /// <param name="parameter">������ɻص���������</param>
        /// <returns></returns>
        public static AnimData Move(GameObject animObject, Vector3? from, Vector3 to,
            float delayTime = 0,
            float time = 0.5f,
            bool isLocal = true,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            Transform toTransform = null,
            AnimCallBack callBack = null,
            object[] parameter = null)
        {

            Vector3 fromTmp;
            AnimType animType;
            if (isLocal)
            {
                fromTmp = from ?? animObject.transform.localPosition;
                animType = AnimType.LocalPosition;
            }
            else
            {
                fromTmp = from ?? animObject.transform.position;
                animType = AnimType.Position;
            }

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = animType;
            l_tmp.m_fromV3 = fromTmp;
            l_tmp.m_toV3 = to;
            l_tmp.m_isLocal = isLocal;
            l_tmp.m_toTransform = toTransform;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region Rotate

        public static AnimData Rotate(GameObject animObject, Vector3? from, Vector3 to,

            float time = 0.5f,
            float delayTime = 0,
            bool isLocal = true,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,

            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimType animType;
            Vector3 fromTmp;

            if (isLocal)
            {
                fromTmp = from ?? animObject.transform.localEulerAngles;
                animType = AnimType.LocalRotate;
            }
            else
            {
                fromTmp = from ?? animObject.transform.eulerAngles;
                animType = AnimType.Rotate;
            }

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = animType;
            l_tmp.m_fromV3 = fromTmp;
            l_tmp.m_toV3 = to;

            l_tmp.m_isLocal = isLocal;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;

        }


        #endregion

        #region Rotate_Quaternion

        public static AnimData Rotation(GameObject animObject, Quaternion? from, Quaternion to,

          float time = 0.5f,
          float delayTime = 0,
          bool isLocal = true,
          InterpType interp = InterpType.Default,
          bool IsIgnoreTimeScale = false,
          RepeatType repeatType = RepeatType.Once,
          int repeatCount = -1,

          AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimType animType;
            Quaternion fromTmp;

            if (isLocal)
            {
                fromTmp = from ?? animObject.transform.localRotation;
                animType = AnimType.LocalRotation;
            }
            else
            {
                fromTmp = from ?? animObject.transform.rotation;
                animType = AnimType.Rotation;
            }

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = animType;
            l_tmp.m_fromQ4 = fromTmp;
            l_tmp.m_toQ4 = to;

            l_tmp.m_isLocal = isLocal;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;

        }


        public static AnimData Rotation(GameObject animObject, Vector3? from, Vector3 to,

         float time = 0.5f,
         float delayTime = 0,
         bool isLocal = true,
         InterpType interp = InterpType.Default,
         bool IsIgnoreTimeScale = false,
         RepeatType repeatType = RepeatType.Once,
         int repeatCount = -1,

         AnimCallBack callBack = null, object[] parameter = null)
        {

            Quaternion? quaternion = null;
            if (from != null)
            {
                quaternion = Quaternion.Euler((Vector3)from);
            }
            return Rotation(animObject, quaternion, Quaternion.Euler(to),

              time,
              delayTime,
              isLocal,
              interp,
              IsIgnoreTimeScale,
              repeatType,
              repeatCount,

              callBack, parameter);

        }
        #endregion

        #region Scale
        public static AnimData Scale(GameObject animObject, Vector3? from, Vector3 to,
        float time = 0.5f,
        InterpType interp = InterpType.Default,
        bool IsIgnoreTimeScale = false,
        RepeatType repeatType = RepeatType.Once,
        int repeatCount = -1,
        float delayTime = 0,
        AnimCallBack callBack = null, object[] parameter = null)
        {

            Vector3 fromTmp = from ?? animObject.transform.localScale;

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_animType = AnimType.LocalScale;
            l_tmp.m_fromV3 = fromTmp;
            l_tmp.m_toV3 = to;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }
        #endregion

        #region CustomMethod

        public static AnimData CustomMethodToFloat(AnimCustomMethodFloat method, float from, float to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animType = AnimType.Custom_Float;
            l_tmp.m_fromFloat = from;
            l_tmp.m_toFloat = to;
            l_tmp.m_customMethodFloat = method;
            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();
            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData CustomMethodToVector2(AnimCustomMethodVector2 method, Vector2 from, Vector2 to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animType = AnimType.Custom_Vector2;
            l_tmp.m_fromV2 = from;
            l_tmp.m_toV2 = to;
            l_tmp.m_customMethodV2 = method;
            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();
            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData CustomMethodToVector3(AnimCustomMethodVector3 method, Vector3 from, Vector3 to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animType = AnimType.Custom_Vector3;
            l_tmp.m_fromV3 = from;
            l_tmp.m_toV3 = to;
            l_tmp.m_customMethodV3 = method;
            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData CustomMethodToVector4(AnimCustomMethodVector4 method, Vector4 from, Vector4 to,
            float time = 0.5f,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animType = AnimType.Custom_Vector4;
            l_tmp.m_fromV4 = from;
            l_tmp.m_toV4 = to;
            l_tmp.m_customMethodV4 = method;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        #endregion

        #region ������
        public static AnimData BezierMove(GameObject animObject, Vector3? from, Vector3 to,
            Vector3[] bezier_contral,
            float time = 0.5f,
            float delayTime = 0,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            InterpType interp = InterpType.Default,
            bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,

            AnimCallBack callBack = null, object[] parameter = null)
        {

            AnimData l_tmp = new AnimData(); ;
            if (isLocal)
            {
                l_tmp.m_animType = AnimType.LocalPosition;
                l_tmp.m_fromV3 = from ?? animObject.transform.localPosition;
            }
            else
            {
                l_tmp.m_animType = AnimType.Position;
                l_tmp.m_fromV3 = from ?? animObject.transform.position;
            }
            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_toV3 = to;
            l_tmp.m_isLocal = isLocal;
            l_tmp.m_pathType = bezierMoveType;
            l_tmp.m_v3Contral = bezier_contral;
            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();
            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData BezierMove(GameObject animObject, Vector3? from, Vector3 to,
            Vector3[] t_Bezier_contral,
            float time = 0.5f,
            InterpType interp = InterpType.Default,
            bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            return BezierMove(animObject, from, to, t_Bezier_contral, time, 0, RepeatType.Once, -1, interp, isLocal, bezierMoveType, callBack, parameter);
        }

        //����From����׼ȷ���Ƶ�
        public static AnimData BezierMove(GameObject animObject, Vector3 to,
            Vector3[] t_Bezier_contral,
            float time = 0.5f,
            InterpType interp = InterpType.Default,
            RepeatType repeatType = RepeatType.Once,
            bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            Vector3 from;
            if (isLocal)
            {
                from = animObject.transform.localPosition;
            }
            else
            {
                from = animObject.transform.position;
            }
            return BezierMove(animObject, from, to, t_Bezier_contral, time, 0, repeatType, -1, interp, isLocal, bezierMoveType, callBack, parameter);
        }

        //��From����׼ȷ���Ƶ������Χ
        public static AnimData BezierMove(GameObject animObject, Vector3? from, Vector3 to, float time,
            float[] t_Bezier_contralRadius,
            RepeatType repeatType,
            int repeatCount = -1,
            float delayTime = 0,
            InterpType interp = InterpType.Default,
            bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            AnimData l_tmp = new AnimData(); ;
            if (isLocal)
            {
                l_tmp.m_animType = AnimType.LocalPosition;
                l_tmp.m_fromV3 = from ?? animObject.transform.localPosition;
            }
            else
            {
                l_tmp.m_animType = AnimType.Position;
                l_tmp.m_fromV3 = from ?? animObject.transform.position;
            }
            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_toV3 = to;
            l_tmp.m_isLocal = isLocal;
            l_tmp.m_pathType = bezierMoveType;
            l_tmp.m_floatContral = t_Bezier_contralRadius;
            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_interpolationType = interp;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;

            l_tmp.Init();
            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public static AnimData BezierMove(GameObject animObject, Vector3 from, Vector3 to, float time,
            float[] t_Bezier_contralRadius,
            float delayTime = 0,
            InterpType interp = InterpType.Default, bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            return BezierMove(animObject, from, to, time, t_Bezier_contralRadius, RepeatType.Once, 1, delayTime, interp, isLocal, bezierMoveType, callBack, parameter);
        }


        //����From����׼ȷ���Ƶ������Χ
        public static AnimData BezierMove(GameObject animObject, Vector3 to, float time, RepeatType repeatType,
            float[] t_Bezier_contralRadius,
            InterpType interp = InterpType.Default,
            float delayTime = 0,
            bool isLocal = true,
            PathType bezierMoveType = PathType.Bezier2,
            AnimCallBack callBack = null, object[] parameter = null)
        {
            Vector3 from;
            if (isLocal)
            {
                from = animObject.transform.localPosition;
            }
            else
            {
                from = animObject.transform.position;
            }
            return BezierMove(animObject, from, to, time, t_Bezier_contralRadius, repeatType, 1, delayTime, interp, isLocal, bezierMoveType, callBack, parameter);
        }

        #endregion

        #region ��˸
        public static AnimData Blink(GameObject animObject, float space,

            float time = 0.5f,
            float delayTime = 0,
            bool IsIgnoreTimeScale = false,
            RepeatType repeatType = RepeatType.Once,
            int repeatCount = -1,
            AnimCallBack callBack = null,
            object[] parameter = null)
        {

            AnimData l_tmp = new AnimData(); ;

            l_tmp.m_animType = AnimType.Blink;
            l_tmp.m_animGameObejct = animObject;
            l_tmp.m_space = space;

            l_tmp.m_delayTime = delayTime;
            l_tmp.m_totalTime = time;
            l_tmp.m_repeatType = repeatType;
            l_tmp.m_repeatCount = repeatCount;
            l_tmp.m_callBack = callBack;
            l_tmp.m_parameter = parameter;
            l_tmp.m_ignoreTimeScale = IsIgnoreTimeScale;

            l_tmp.Init();

            GetInstance().animList.Add(l_tmp);
            return l_tmp;

        }

        #endregion

        #region ValueTo

        public static AnimData ValueTo(AnimParamHash l_animHash)
        {
            AnimData l_tmp = l_animHash.GetAnimData();
            l_tmp.Init();
            GetInstance().animList.Add(l_tmp);
            return l_tmp;
        }

        public class AnimParamHash : Dictionary<AnimParamType, object>
        {
            public AnimParamHash(params object[] l_params)
            {
                for (int i = 0; i < l_params.Length; i += 2)
                {
                    this[(AnimParamType)l_params[i]] = l_params[i + 1];
                }
            }

            public AnimParamHash SetData(params object[] l_params)
            {
                Clear();
                for (int i = 0; i < l_params.Length; i += 2)
                {
                    this[(AnimParamType)l_params[i]] = l_params[i + 1];
                }
                return this;
            }

            public AnimData GetAnimData()
            {
                AnimData DataTmp = HeapObjectPool<AnimData>.GetObject();
                foreach (var hash in this)
                {
                    AnimParamType l_ParamType = hash.Key;
                    switch (l_ParamType)
                    {
                        //��������
                        case AnimParamType.GameObj: DataTmp.m_animGameObejct = (GameObject)hash.Value; break;
                        case AnimParamType.AnimType: DataTmp.m_animType = (AnimType)hash.Value; break;
                        case AnimParamType.Time: DataTmp.m_totalTime = (float)hash.Value; break;
                        case AnimParamType.InteType: DataTmp.m_interpolationType = (InterpType)hash.Value; break;
                        case AnimParamType.RepeatType: DataTmp.m_repeatType = (RepeatType)hash.Value; break;
                        case AnimParamType.RepeatCount: DataTmp.m_repeatCount = (int)hash.Value; break;
                        case AnimParamType.DelayTime: DataTmp.m_delayTime = (float)hash.Value; break;

                        //From
                        case AnimParamType.FromV3: DataTmp.m_fromV3 = (Vector3)hash.Value; break;
                        case AnimParamType.FromV2: DataTmp.m_fromV2 = (Vector2)hash.Value; break;
                        case AnimParamType.FromColor: DataTmp.m_fromColor = (Color)hash.Value; break;
                        case AnimParamType.FromFloat: DataTmp.m_fromFloat = (float)hash.Value; break;

                        //To
                        case AnimParamType.ToV3: DataTmp.m_toV3 = (Vector3)hash.Value; break;
                        case AnimParamType.ToV2: DataTmp.m_toV2 = (Vector2)hash.Value; break;
                        case AnimParamType.ToColor: DataTmp.m_toColor = (Color)hash.Value; break;
                        case AnimParamType.ToFloat: DataTmp.m_toFloat = (float)hash.Value; break;

                        //�����ص�
                        case AnimParamType.CallBack: DataTmp.m_callBack = (AnimCallBack)hash.Value; break;
                        case AnimParamType.CallBackParams: DataTmp.m_parameter = (object[])hash.Value; break;

                        //���ƺ���
                        case AnimParamType.CustomMethodV3: DataTmp.m_customMethodV3 = (AnimCustomMethodVector3)hash.Value; break;
                        case AnimParamType.CustomMethodV2: DataTmp.m_customMethodV2 = (AnimCustomMethodVector2)hash.Value; break;
                        case AnimParamType.CustomMethodFloat: DataTmp.m_customMethodFloat = (AnimCustomMethodFloat)hash.Value; break;

                        //��˸
                        case AnimParamType.Space: DataTmp.m_space = (float)hash.Value; break;

                        //���������Ƶ�
                        case AnimParamType.PathType: DataTmp.m_pathType = (PathType)hash.Value; break;
                        case AnimParamType.V3Control: DataTmp.m_v3Contral = (Vector3[])hash.Value; break;
                        case AnimParamType.floatControl: DataTmp.m_floatContral = (float[])hash.Value; break;

                        //��������
                        case AnimParamType.IsIncludeChild: DataTmp.m_isChild = (bool)hash.Value; break;
                        case AnimParamType.IsLocal: DataTmp.m_isLocal = (bool)hash.Value; break;
                        case AnimParamType.IsIgnoreTimeScale: DataTmp.m_ignoreTimeScale = (bool)hash.Value; break;
                    }
                }
                return DataTmp;
            }
        }

        #endregion
    }
}