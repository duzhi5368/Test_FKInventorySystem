using System.Net;
using System.Text;
using System;
//------------------------------------------------------------------------
namespace FKGame
{
    public class NetDataWriter
    {
        protected byte[] _data;
        protected int _position;
        private const int InitialSize = 64;
        private readonly bool _autoResize;
        private ByteOrder byteOrder;

        public int Position() { return _position; }
        public int Capacity { get { return _data.Length; }}

        public NetDataWriter(ByteOrder byteOrder) : this(true, InitialSize, byteOrder){}

        public NetDataWriter(bool autoResize, ByteOrder byteOrder) : this(autoResize, InitialSize, byteOrder){}
        
        public NetDataWriter(bool autoResize, int initialSize, ByteOrder byteOrder)
        {
            this.byteOrder = byteOrder;
            _data = new byte[initialSize];
            _autoResize = autoResize;
        }

        public void ResizeIfNeed(int newSize)
        {
            int len = _data.Length;
            if (len < newSize)
            {
                while (len < newSize)
                    len *= 2;
                Array.Resize(ref _data, len);
            }
        }

        public void Reset(int size)
        {
            ResizeIfNeed(size);
            _position = 0;
        }

        public void Reset()
        {
            _position = 0;
        }

        public byte[] CopyData()
        {
            byte[] resultData = new byte[_position];
            Buffer.BlockCopy(_data, 0, resultData, 0, _position);
            return resultData;
        }

        public byte[] Data
        {
            get { return _data; }
        }

        public int Length
        {
            get { return _position; }
        }

        public void Put(float value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 4;
        }

        public void Put(double value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 8;
        }

        public void Put(long value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 8;
        }

        public void Put(ulong value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 8);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 8;
        }

        public void Put(int value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 4;
        }

        public void Put(uint value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 4;
        }

        public void Put(char value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 2;
        }

        public void Put(ushort value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 2;
        }

        public void Put(short value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 2);
            FastBitConverter.GetBytes(byteOrder, _data, _position, value);
            _position += 2;
        }

        public void Put(sbyte value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = (byte)value;
            _position++;
        }

        public void Put(byte value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = value;
            _position++;
        }
        public void Put(bool value)
        {
            if (_autoResize)
                ResizeIfNeed(_position + 1);
            _data[_position] = (byte)(value ? 1 : 0);
            _position++;
        }

        public void Put(byte[] data, int offset, int length)
        {
            if (_autoResize)
                ResizeIfNeed(_position + length);
            Buffer.BlockCopy(data, offset, _data, _position, length);
            _position += length;
        }

        // byte[]不加入长度
        public void PutByteSource(byte[] data)
        {
            if (_autoResize)
                ResizeIfNeed(_position + data.Length);
            Buffer.BlockCopy(data, 0, _data, _position, data.Length);
            _position += data.Length;
        }

        public void PutSBytesWithLength(sbyte[] data, int offset, int length)
        {
            if (_autoResize)
                ResizeIfNeed(_position + length + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, length);
            Buffer.BlockCopy(data, offset, _data, _position + 4, length);
            _position += length + 4;
        }

        // 加入长度
        public void Put(sbyte[] data)
        {
            if (_autoResize)
                ResizeIfNeed(_position + data.Length + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, data.Length);
            Buffer.BlockCopy(data, 0, _data, _position + 4, data.Length);
            _position += data.Length + 4;
        }

        public void PutBytesWithLength(byte[] data, int offset, int length)
        {
            if (_autoResize)
                ResizeIfNeed(_position + length + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, length);
            Buffer.BlockCopy(data, offset, _data, _position + 4, length);
            _position += length + 4;
        }

        // 加入长度
        public void Put(byte[] data)
        {
            if (_autoResize)
                ResizeIfNeed(_position + data.Length + 4);
            FastBitConverter.GetBytes(byteOrder, _data, _position, data.Length);
            Buffer.BlockCopy(data, 0, _data, _position + 4, data.Length);
            _position += data.Length + 4;
        }

        public void Put(float[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(double[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(long[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(ulong[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 8 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(int[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(uint[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 4 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(ushort[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 2 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(short[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len * 2 + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(bool[] value)
        {
            int len = value == null ? 0 : value.Length;
            if (_autoResize)
                ResizeIfNeed(_position + len + 4);
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(string[] value)
        {
            int len = value == null ? 0 : value.Length;
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i]);
        }

        public void Put(string[] value, int maxLength)
        {
            int len = value == null ? 0 : value.Length;
            Put(len);
            for (int i = 0; i < len; i++)
                Put(value[i], maxLength);
        }

        public void Put(IPEndPoint endPoint)
        {
            Put(endPoint.Address.ToString());
            Put(endPoint.Port);
        }

        public void Put(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Put(0);
                return;
            }
            int bytesCount = Encoding.UTF8.GetByteCount(value);
            if (_autoResize)
                ResizeIfNeed(_position + bytesCount + 4);
            Put(bytesCount);
            Encoding.UTF8.GetBytes(value, 0, value.Length, _data, _position);
            _position += bytesCount;
        }

        public void Put(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                Put(0);
                return;
            }
            int length = value.Length > maxLength ? maxLength : value.Length;
            int bytesCount = Encoding.UTF8.GetByteCount(value);
            if (_autoResize)
                ResizeIfNeed(_position + bytesCount + 4);
            Put(bytesCount);
            Encoding.UTF8.GetBytes(value, 0, length, _data, _position);
            _position += bytesCount;
        }
    }
}