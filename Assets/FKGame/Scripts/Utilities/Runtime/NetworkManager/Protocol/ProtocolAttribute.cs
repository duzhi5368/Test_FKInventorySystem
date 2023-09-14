using System;
//------------------------------------------------------------------------
namespace FKGame
{

    /// <summary>
    /// ��Protocol��Int16������ֶ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Int16Attribute : System.Attribute { }

    /// <summary>
    /// ��Protocol��Int8������ֶ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Int8Attribute : System.Attribute { }

    /// <summary>
    /// ��Protocol��Int32������ֶ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class Int32Attribute : System.Attribute { }

    /// <summary>
    /// ģ������ģ����Ϣ����
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ModuleAttribute : System.Attribute
    {
        public int MessageCode;
        public string ModuleName;

        public ModuleAttribute(int messageCode, string moduleName)
        {
            MessageCode = messageCode;
            ModuleName = moduleName;
        }
    }

    /// <summary>
    /// ��Ϣ����ģʽ���������Ĭ��ΪBoth
    /// ���� protocol ʱ ToClient �� ToServer ���Ͳ����Զ��Ӻ�׺��Ҫ��֤����������_s ���� _c��׺
    /// Both�Զ���� _s �� _c ��׺
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageModeAttribute : System.Attribute
    {
        public SendMode Mode;

        public MessageModeAttribute(SendMode mode)
        {
            Mode = mode;
        }
    }

    public enum SendMode
    {
        ToClient,
        ToServer,
        Both,
    }

    /// <summary>
    /// �Զ���Protocol�����Ļ���
    /// </summary>
    public interface IProtocolMessageInterface { }

    /// <summary>
    /// �Զ���Protocol�����Ľṹ
    /// </summary>
    public interface IProtocolStructInterface { }

    /// <summary>
    /// 
    /// </summary>
    public interface CsharpProtocolInterface : IProtocolMessageInterface { }
}