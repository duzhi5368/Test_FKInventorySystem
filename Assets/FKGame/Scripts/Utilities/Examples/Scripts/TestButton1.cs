using UnityEngine;
using FKGame;
using System.Collections.Generic;
//------------------------------------------------------------------------
public class TestButton1 : MonoBehaviour
{
    [System.Serializable]
    public class MyTestObj : ScriptableObject, INameable, IJsonSerializable
    {
        private string m_MyTestObjName = string.Empty;
        private int m_MyTestObjID = 0;
        public string Name
        {
            get { return this.m_MyTestObjName; }
            set { this.m_MyTestObjName = value; }
        }
        public MyTestObj(string str, int id)
        {
            m_MyTestObjName = str;
            m_MyTestObjID = id;
        }
        public virtual void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", this.Name);
            data.Add("ObjID", (int)this.m_MyTestObjID);
        }
        public virtual void SetObjectData(Dictionary<string, object> data)
        {
            string name = (string)data["Name"];
            this.m_MyTestObjID = (int)data["ObjID"];
        }
    }

    public UtilityBehavior utilityBehavior;
    private List<MyTestObj> m_MyTestObjList;

    void Start()
    {
        m_MyTestObjList = new List<MyTestObj>();
        m_MyTestObjList.Add(new MyTestObj("ÄãºÃ",1));
        m_MyTestObjList.Add(new MyTestObj("Äã²»ºÃ", 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnButton1Click()
    {
        //utilityBehavior.QuitApplication();
        CameraEffects.Shake();

        string testObjStr = JsonSerializer.Serialize(m_MyTestObjList.ToArray());
        Debug.Log(testObjStr);
    }
    public void OnSelected()
    {
        Debug.Log("OnSelected");
    }

    public void OnDeselected()
    {
        Debug.Log("OnDeselected");
    }
}
