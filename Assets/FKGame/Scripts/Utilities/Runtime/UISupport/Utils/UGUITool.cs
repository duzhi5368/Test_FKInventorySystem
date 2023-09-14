using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class UGUITool
    {
        static PointerEventData eventDatas = new PointerEventData(EventSystem.current);
        static List<RaycastResult> hit = new List<RaycastResult>();

        static public bool IsHitUI()
        {
            eventDatas.position = Input.mousePosition;
            eventDatas.pressPosition = Input.mousePosition;
            EventSystem.current.RaycastAll(eventDatas, hit);
            if (hit.Count > 0)
                return true;
            if (EventSystem.current.IsPointerOverGameObject())  //������UI��
                return true;
            return false;
        }

        // �ж��Ƿ�����ĳ������
        static public bool IsClickGameObject(GameObject go)
        {
            eventDatas.position = Input.mousePosition;
            eventDatas.pressPosition = Input.mousePosition;
            EventSystem.current.RaycastAll(eventDatas, hit);
            for (int i = 0; i < hit.Count; i++)
            {
                if (hit[i].gameObject == go)
                {
                    return true;
                }
            }
            return false;
        }


        static public void SetImageSprite(Image img, string name, bool is_nativesize = false)
        {
            if (name == null)
            {
                Debug.LogError("set_icon Image name ����Ϊ null !");
                return;
            }
            if (img == null)
            {
                Debug.LogError("set_icon Image ����Ϊ null !");
                return;
            }
            try
            {
                Sprite sp = ResourceManager.Load<Sprite>(name);
                img.overrideSprite = sp;
                img.sprite = img.overrideSprite;

                if (is_nativesize)
                    img.SetNativeSize();
            }
            catch (Exception e)
            {
                Debug.LogError("SetImageSprite ����ʧ�ܣ��鿴��Դ�Ƿ���ڣ�ͼƬ��ʽ�Ƿ���ȷ:" + name + "\n" + e);
            }
        }

        static public void SetSpriteRender(GameObject go, string name)
        {
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            sprite.sprite = LoadSprite(name);
        }

        public static Sprite LoadSprite(string resName)
        {
            try
            {
                Texture2D texture = ResourceManager.Load<Texture2D>(resName);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            catch (Exception e)
            {
                Debug.LogError("����ͼƬʧ�ܣ�" + resName + "\n" + e);
                return null;
            }
        }
    }
}