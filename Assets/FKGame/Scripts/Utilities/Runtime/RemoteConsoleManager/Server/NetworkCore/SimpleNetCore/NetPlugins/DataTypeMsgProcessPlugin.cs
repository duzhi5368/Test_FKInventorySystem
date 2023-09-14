using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class DataTypeMsgProcessPlugin : INetMsgProcessPluginBase
    {
        private EndianBitConverter bitConverter = null;

        public override byte GetNetProperty()
        {
            return (byte)NetProperty.Data;
        }

        public override void Release() {}
        protected override void OnInit() {}

        public override void ReceveProcess(MsgPackest packest)
        {
            Session session = packest.session;
            if (packest.isEncryption == 1)
            {
                IMsgEncryptionBase encryption = networkCommon.Configuration.GetMsgEncryption();
                if (encryption == null)
                {
                    NetDebug.LogError("��֧����Ϣ���ܣ�" + packest);
                    return;
                }
                else
                {
                    try
                    {
                        packest.contents = encryption.Decryption(packest.session, packest.contents);
                    }
                    catch (System.Exception e)
                    {
                        NetDebug.LogError("��Ϣ���ܴ���" + packest + " \n" + e);
                        return;
                    }
                }
            }
            if (packest.isCompress == 1)
            {
                INetworkCoreMsgCompressBase compress = networkCommon.Configuration.GetCompressFunction(packest.compressType);
                if (compress == null)
                {
                    NetDebug.LogError("��֧�ֵ�ѹ����ʽ��" + packest.compressType);
                    return;
                }
                else
                {
                    packest.contents = compress.Decompress(packest.contents);
                }
            }
            if (bitConverter == null || bitConverter.byteOrder != packest.byteOrder)
            {
                bitConverter = EndianBitConverter.GetBitConverter(packest.byteOrder);
            }
            // �շ���Ϣ��������������Ϣ���кţ�
            uint counter = bitConverter.ToUInt32(packest.contents, 0);
            byte[] dataArray = new byte[packest.contents.Length - 4];
            Array.Copy(packest.contents, 4, dataArray, 0, dataArray.Length);
            packest.contents = dataArray;
            networkCommon.ReceiveMsgPackest(packest);
        }

        public override byte[] SendProcess(Session session, byte msgProperty, byte[] datas)
        {
            ByteOrder byteOrder = networkCommon.Configuration.byteOrder;
            byte compressType = 0;
            byte isEncryption = 0;
            byte isCompress = 0;

            if (bitConverter == null || bitConverter.byteOrder != byteOrder)
            {
                bitConverter = EndianBitConverter.GetBitConverter(byteOrder);
            }
            byte[] pBytes = bitConverter.GetBytes(session.AddSendCounter());
            byte[] allDatas = new byte[pBytes.Length + datas.Length];
            pBytes.CopyTo(allDatas, 0);
            datas.CopyTo(allDatas, pBytes.Length);
            datas = allDatas;

            INetworkCoreMsgCompressBase compress = networkCommon.Configuration.GetSendCompressFunction();
            if (compress != null)
            {
                isCompress = 1;
                try
                {
                    datas = compress.Compress(datas);
                }
                catch (Exception e)
                {
                    NetDebug.LogError("ѹ������:" + e);
                    return null;
                }
                compressType = compress.CompressType;
            }
            else
            {
                isCompress = 0;
            }

            IMsgEncryptionBase encryption = networkCommon.Configuration.GetMsgEncryption();
            if (networkCommon.Configuration.IsEncryption && encryption != null)
            {
                isEncryption = 1;
                try
                {
                    datas = encryption.Encryption(session, datas);
                }
                catch (Exception e)
                {
                    NetDebug.LogError("���ܴ���" + e);
                    return null;
                }
            }
            else
            {
                isEncryption = 0;
            }

            byte[] res = MsgPackest.Write2Bytes(networkCommon.Configuration.byteOrder, isEncryption, isCompress, compressType, msgProperty, datas);
            return res;
        }
    }
}