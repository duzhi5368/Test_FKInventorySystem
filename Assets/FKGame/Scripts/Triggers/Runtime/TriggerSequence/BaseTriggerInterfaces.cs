using UnityEngine;
using UnityEngine.EventSystems;
//------------------------------------------------------------------------
namespace FKGame {
    // 基本触发器事件
    public interface ITriggerEventHandler
    {
    }

    // 进行使用 的触发
    public interface ITriggerUsedHandler : ITriggerEventHandler{
        void OnTriggerUsed(GameObject player);
    }

    // 取消使用 的触发
    public interface ITriggerUnUsedHandler : ITriggerEventHandler{
        void OnTriggerUnUsed(GameObject player);
    }

    // 进入触发区域 的触发
    public interface ITriggerCameInRange : ITriggerEventHandler {
        void OnCameInRange(GameObject player);
    }

    // 离开触发区域 的触发
    public interface ITriggerWentOutOfRange : ITriggerEventHandler{
        void OnWentOutOfRange(GameObject player);
    }

    // 鼠标悬浮到触发区域  的触发
    public interface ITriggerPointerEnter : ITriggerEventHandler {
        void OnPointerEnter(PointerEventData eventData);
    }

    // 鼠标离开触发区域  的触发
    public interface ITriggerPointerExit : ITriggerEventHandler{
        void OnPointerExit(PointerEventData eventData);
    }
}