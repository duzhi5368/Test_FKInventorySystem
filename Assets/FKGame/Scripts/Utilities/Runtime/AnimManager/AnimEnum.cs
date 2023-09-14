using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public delegate void AnimCallBack(params object[] arg);
    public delegate void AnimCustomMethodVector4(Vector4 data);
    public delegate void AnimCustomMethodVector3(Vector3 data);
    public delegate void AnimCustomMethodVector2(Vector2 data);
    public delegate void AnimCustomMethodFloat(float data);

    //动画类型
    public enum AnimType
    {
        LocalPosition,
        Position,
        LocalScale,
        LocalRotate,
        Rotate,
        LocalRotation,
        Rotation,

        Color,
        Alpha,

        UGUI_Color,
        UGUI_Alpha,
        UGUI_AnchoredPosition,
        UGUI_SizeDetal,

        Custom_Vector4,
        Custom_Vector3,
        Custom_Vector2,
        Custom_Float,

        Blink,
    }

    // 动画插值算法类型
    public enum InterpType
    {
        Default,
        Linear,
        InBack,
        OutBack,
        InOutBack,
        OutInBack,
        InQuad,
        OutQuad,
        InoutQuad,
        InCubic,
        OutCubic,
        InoutCubic,
        OutInCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        OutInQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        OutInQuint,
        InSine,
        OutSine,
        InOutSine,
        OutInSine,

        InExpo,
        OutExpo,
        InOutExpo,
        OutInExpo,

        OutBounce,
        InBounce,
        InOutBounce,
        OutInBounce,
    }

    // 动画参数类型
    public enum AnimParamType
    {
        GameObj,

        FromV3,
        FromV2,
        FromFloat,
        FromColor,

        ToV3,
        ToV2,
        ToFloat,
        ToColor,

        DelayTime,

        AnimType,
        Time,
        InteType,

        IsIgnoreTimeScale,

        PathType,
        V3Control,
        floatControl,

        IsIncludeChild,
        IsLocal,

        RepeatType,
        RepeatCount,

        CustomMethodV3,
        CustomMethodV2,
        CustomMethodFloat,

        Space,

        CallBack,
        CallBackParams
    }

    // 动画路径类型
    public enum PathType
    {
        Line,
        Bezier2,
        Bezier3,
    }

    // 动画重复类型
    public enum RepeatType
    {
        Once,
        Loop,
        PingPang,
    }
}