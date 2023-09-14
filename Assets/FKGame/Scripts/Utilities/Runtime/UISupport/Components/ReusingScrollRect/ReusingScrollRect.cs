using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//------------------------------------------------------------------------
namespace FKGame
{
    public class ReusingScrollRect : ScrollRectInput
    {
        class ReusingData
        {
            public int index;
            public ReusingStatus status;
        }

        public string m_ItemName = "";
        public bool m_isInversion = false;      // Ĭ���Ǵ��ϵ��£������ң����ϴ�ѡ������
        public bool m_isReceiveControl = true;  // �Ƿ���ܲ���
        public List<Dictionary<string, object>> m_datas = new List<Dictionary<string, object>>();
        public List<ReusingScrollItemBase> m_items = new List<ReusingScrollItemBase>();
        public List<ReusingScrollItemBase> m_itemCaches = new List<ReusingScrollItemBase>();
        private Bounds m_viewBounds;
        public GameObject m_itemPrefab;
        public Vector3 m_itemSize;
        private bool m_rebuild = false;
        private List<ReusingData> m_indexList = new List<ReusingData>();

        public void SetItem(string itemName)
        {
            m_ItemName = itemName;
            Rebuild(CanvasUpdate.Layout);
            UpdateBound();
            SetLayout();
            m_itemPrefab = GameObjectManager.CreateGameObjectByPool(m_ItemName);
            m_itemSize = m_itemPrefab.GetComponent<RectTransform>().sizeDelta;
        }

        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < m_items.Count; i++)
            {
                GameObjectManager.DestroyGameObjectByPool(m_items[i].gameObject);
            }
            m_items.Clear();
            for (int i = 0; i < m_itemCaches.Count; i++)
            {
                GameObjectManager.DestroyGameObjectByPool(m_itemCaches[i].gameObject);
            }
            m_itemCaches.Clear();
        }

        public void Refresh()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].SetContent(m_items[i].m_index, m_datas[m_items[i].m_index]);
            }
            SetItemDisplay();
        }

        public void SetData(List<Dictionary<string, object>> data)
        {
            m_datas = data;
            UpdateIndexList(data.Count);
            UpdateConetntSize(data.Count);
            SetItemDisplay();
        }

        public ReusingScrollItemBase GetItem(int index)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i].m_index == index)
                {
                    return m_items[i];
                }
            }
            return null;
        }

        public Vector3 GetItemAnchorPos(int index)
        {
            if (content == null)
            {
                throw new Exception("SetItemDisplay Exception: content is null !");
            }
            return GetItemPos(index) + GetRealItemOffset() + content.localPosition;
        }

        public void SetPos(Vector3 pos)
        {
            if (content == null)
            {
                throw new Exception("SetItemDisplay Exception: content is null !");
            }
            content.anchoredPosition3D = pos;
            SetItemDisplay();
        }

        public void Update()
        {
            if (m_rebuild)
            {
                m_rebuild = false;
                SetItemDisplay();
            }
        }

        protected override void OnSetContentAnchoredPosition(InputUIOnScrollEvent e)
        {
            base.OnSetContentAnchoredPosition(e);
            SetItemDisplay();
        }

        protected override void Start()
        {
            base.Start();
            SetItemDisplay();
        }

        public override void Rebuild(CanvasUpdate executing)
        {
            base.Rebuild(executing);
            UpdateBound();
            m_rebuild = true;
        }

        void SetLayout()
        {
            content.anchorMin = GetMinAchors();
            content.anchorMax = GetMaxAchors();
            content.pivot = GetPivot();
            content.anchoredPosition3D = Vector3.zero;
        }

        void UpdateBound()
        {
            m_viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
        }

        void UpdateConetntSize(int count)
        {
            Vector3 size = m_itemSize;
            if (horizontal)
            {
                size.x *= count;
                size.y = 0;
            }
            if (vertical)
            {
                size.y *= count;
                size.x = 0;
            }
            content.sizeDelta = size;
        }

        void UpdateIndexList(int count)
        {
            m_indexList = new List<ReusingData>();
            for (int i = 0; i < count; i++)
            {
                ReusingData reusingTmp = null;
                if (m_indexList.Count > i)
                {
                    reusingTmp = m_indexList[i];
                }
                else
                {
                    reusingTmp = new ReusingData();
                    m_indexList.Add(reusingTmp);
                }
                reusingTmp.index = i;
                reusingTmp.status = ReusingStatus.Hide;
            }
        }

        void SetItemDisplay(bool isRebuild = false)
        {
            if (content == null)
            {
                throw new Exception("SetItemDisplay Exception: content is null !");
            }
            // ��������ʾ����Щ��Ҫ����
            for (int i = 0; i < m_items.Count; i++)
            {
                m_items[i].OnDrag();
                // �Ѿ���ȫ�뿪����ʾ����
                if (!m_viewBounds.Intersects(GetItemBounds(m_items[i].m_index)))
                {
                    ReusingScrollItemBase itemTmp = m_items[i];
                    m_items.Remove(itemTmp);
                    itemTmp.OnHide();
                    if (!isRebuild)
                    {
                        // ���ز��Ƶ�����
                        itemTmp.gameObject.SetActive(false);
                    }
                    m_itemCaches.Add(itemTmp);
                    m_indexList[itemTmp.m_index].status = ReusingStatus.Hide;
                }
            }
            // �������Щ��Ҫ��ʾ
            // �����δ��ʾ������ʾ�������Ӷ����ȡ������
            bool m_clip = false;
            for (int i = 0; i < m_indexList.Count; i++)
            {
                if (m_indexList[i].status == ReusingStatus.Hide)
                {
                    if (m_viewBounds.Intersects(GetItemBounds(m_indexList[i].index)))
                    {
                        ShowItem(i, isRebuild, m_datas[i]);
                        m_indexList[i].status = ReusingStatus.Show;
                        m_clip = true;
                    }
                    else
                    {
                        if (m_clip)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    m_clip = true;
                }
            }
        }

        void ShowItem(int index, bool isRebuild, Dictionary<string, object> data)
        {
            ReusingScrollItemBase itemTmp = GetItem();
            itemTmp.transform.SetParent(content);
            itemTmp.transform.localScale = Vector3.one;
            if (!isRebuild)
            {
                itemTmp.SetContent(index, data);
            }
            itemTmp.RectTransform.pivot = GetPivot();
            itemTmp.RectTransform.anchorMin = GetMinAchors();
            itemTmp.RectTransform.anchorMax = GetMaxAchors();
            itemTmp.RectTransform.sizeDelta = GetItemSize();
            itemTmp.RectTransform.anchoredPosition3D = GetItemPos(index);
            itemTmp.m_index = index;
        }

        ReusingScrollItemBase GetItem()
        {
            ReusingScrollItemBase result = null;
            if (m_itemCaches.Count > 0)
            {
                result = m_itemCaches[0];
                result.gameObject.SetActive(true);
                result.OnShow();
                m_itemCaches.RemoveAt(0);
                m_items.Add(result);
                return result;
            }
            result = GameObjectManager.CreateGameObjectByPool(m_itemPrefab).GetComponent<ReusingScrollItemBase>();
            result.Init(m_UIEventKey, m_items.Count);
            m_items.Add(result);
            return result;
        }

        Bounds GetItemBounds(int index)
        {
            return new Bounds(GetItemPos(index) + GetRealItemOffset() + content.localPosition, m_itemSize);
        }

        Vector3 GetItemPos(int index)
        {
            Vector3 offset = Vector3.zero;
            if (vertical)
            {
                offset = new Vector3(0, -m_itemSize.y, 0);
            }
            else
            {
                offset = new Vector3(m_itemSize.x, 0, 0);
            }
            if (m_isInversion)
            {
                offset *= -1;
            }
            offset *= index;
            return offset;
        }

        Vector3 GetRealItemOffset()
        {
            Vector3 offset;
            if (vertical)
            {
                offset = new Vector3(0, -m_itemSize.y / 2, 0);
            }
            else
            {
                offset = new Vector3(m_itemSize.x / 2, 0, 0);
            }
            if (m_isInversion)
            {
                offset *= -1;
            }
            return offset;
        }

        Vector2 GetPivot()
        {
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            if (horizontal)
            {
                if (!m_isInversion)
                {
                    pivot.x = 0;
                }
                else
                {
                    pivot.x = 1;
                }
            }
            if (vertical)
            {
                if (!m_isInversion)
                {
                    pivot.y = 1;
                }
                else
                {
                    pivot.y = 0;
                }
            }
            return pivot;
        }

        Vector2 GetMinAchors()
        {
            Vector2 minAchors = new Vector2(0.5f, 0.5f);
            if (horizontal)
            {
                if (!m_isInversion)
                {
                    minAchors.x = 0;
                }
                else
                {
                    minAchors.x = 1;
                }
                minAchors.y = 0;
            }
            if (vertical)
            {
                if (!m_isInversion)
                {
                    minAchors.y = 1;
                }
                else
                {
                    minAchors.y = 0;
                }
                minAchors.x = 0;
            }
            return minAchors;
        }

        Vector2 GetMaxAchors()
        {
            Vector2 maxAchors = new Vector2(0.5f, 0.5f);
            if (horizontal)
            {
                if (!m_isInversion)
                {
                    maxAchors.x = 0;
                }
                else
                {
                    maxAchors.x = 1;
                }
                maxAchors.y = 1;
            }
            if (vertical)
            {
                if (!m_isInversion)
                {
                    maxAchors.y = 1;
                }
                else
                {
                    maxAchors.y = 0;
                }
                maxAchors.x = 1;
            }
            return maxAchors;
        }

        Vector2 GetItemSize()
        {
            Vector3 size = m_itemSize;
            if (horizontal)
            {
                size.y = 0;
            }
            if (vertical)
            {
                size.x = 0;
            }
            return size;
        }

        public void StartEnterAnim()
        {
            m_isReceiveControl = false;
            StartCoroutine(EnterAnim());
        }

        public void StartExitAnim()
        {
            m_isReceiveControl = false;
            StartCoroutine(ExitAnim());
        }

        void EndEnterAnim()
        {
            m_isReceiveControl = true;
        }

        void EndExitAnim()
        {
            m_isReceiveControl = true;
        }

        public virtual IEnumerator EnterAnim()
        {
            return null;
        }

        public virtual IEnumerator ExitAnim()
        {
            return null;
        }
    }
}