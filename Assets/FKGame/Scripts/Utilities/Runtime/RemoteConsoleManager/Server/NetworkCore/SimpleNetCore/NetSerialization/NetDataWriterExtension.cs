using System.Collections.Generic;
using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class NetDataWriterExtension
    {
        public static void PutListData<T>(NetDataWriter writer, IList<T> data) where T : INetSerializable, new()
        {
            int count = 0;

            if (data != null)
            {
                count = data.Count;
            }
            writer.Put(count);
            foreach (var item in data)
            {
                item.Serialize(writer);
            }
        }

        public static void PutValue(this NetDataWriter writer, object value)
        {
            if (value == null)
            {
                Debug.LogError("PutValue is null");
                return;
            }
            Type type = value.GetType();
            if (type == typeof(bool))
            {
                writer.Put((bool)value);
                return;
            }

            if (type == typeof(bool[]))
            {
                writer.Put((bool[])value);
                return;
            }

            if (type == typeof(byte))
            {
                writer.Put((byte)value);
                return;
            }

            if (type == typeof(byte[]))
            {
                writer.Put((byte[])value);
                return;
            }

            if (type == typeof(char))
            {
                writer.Put((char)value);
                return;
            }

            if (type == typeof(double))
            {
                writer.Put((double)value);
                return;
            }

            if (type == typeof(double[]))
            {
                writer.Put((double[])value);
                return;
            }

            if (type == typeof(float))
            {
                writer.Put((float)value);
                return;
            }

            if (type == typeof(float[]))
            {
                writer.Put((float[])value);
                return;
            }

            if (type == typeof(int))
            {
                writer.Put((int)value);
                return;
            }

            if (type == typeof(int[]))
            {
                writer.Put((int[])value);
                return;
            }

            if (type == typeof(long))
            {
                writer.Put((long)value);
                return;
            }

            if (type == typeof(long[]))
            {
                writer.Put((long[])value);
                return;
            }

            if (type == typeof(sbyte))
            {
                writer.Put((sbyte)value);
                return;
            }

            if (type == typeof(short))
            {
                writer.Put((short)value);
                return;
            }

            if (type == typeof(short[]))
            {
                writer.Put((short[])value);
                return;
            }

            if (type == typeof(string))
            {
                writer.Put((string)value);
                return;
            }

            if (type == typeof(uint))
            {
                writer.Put((uint)value);
                return;
            }

            if (type == typeof(uint[]))
            {
                writer.Put((uint[])value);
                return;
            }

            if (type == typeof(ulong))
            {
                writer.Put((ulong)value);
                return;
            }

            if (type == typeof(ulong[]))
            {
                writer.Put((ulong[])value);
                return;
            }

            if (type == typeof(ushort))
            {
                writer.Put((ushort)value);
                return;
            }

            if (type == typeof(ushort[]))
            {
                writer.Put((ushort[])value);
                return;
            }

            if (type == typeof(Color))
            {
                writer.Put((Color)value);
                return;
            }

            if (type == typeof(Quaternion))
            {
                writer.Put((Quaternion)value);
                return;
            }

            if (type == typeof(Vector2))
            {
                writer.Put((Vector2)value);
                return;
            }

            if (type == typeof(Vector2Int))
            {
                writer.Put((Vector2Int)value);
                return;
            }

            if (type == typeof(Vector3))
            {
                writer.Put((Vector3)value);
                return;
            }

            if (type == typeof(Vector3Int))
            {
                writer.Put((Vector3Int)value);
                return;
            }

            if (type == typeof(Vector4))
            {
                writer.Put((Vector4)value);
                return;
            }

            if (typeof(INetSerializable).IsAssignableFrom(type))
            {
                (value as INetSerializable).Serialize(writer);
                return;
            }

            throw new ArgumentException("NetDataReader cannot write type " + value.GetType().Name);
        }

        public static void Put(this NetDataWriter writer, Color value)
        {
            byte r = (byte)(value.r * 100f);
            byte g = (byte)(value.g * 100f);
            byte b = (byte)(value.b * 100f);
            byte a = (byte)(value.a * 100f);
            writer.Put(r);
            writer.Put(g);
            writer.Put(b);
            writer.Put(a);
        }

        public static void Put(this NetDataWriter writer, Quaternion value)
        {
            writer.Put(value.eulerAngles.x);
            writer.Put(value.eulerAngles.y);
            writer.Put(value.eulerAngles.z);
        }

        public static void Put(this NetDataWriter writer, Vector2 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
        }

        public static void Put(this NetDataWriter writer, Vector2Int value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
        }

        public static void Put(this NetDataWriter writer, Vector3 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
        }

        public static void Put(this NetDataWriter writer, Vector3Int value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
        }

        public static void Put(this NetDataWriter writer, Vector4 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
            writer.Put(value.w);
        }

        public static void PutPackedUShort(this NetDataWriter writer, ushort value)
        {
            PutPackedULong(writer, value);
        }

        public static void PutPackedUInt(this NetDataWriter writer, uint value)
        {
            PutPackedULong(writer, value);
        }

        public static void PutPackedULong(this NetDataWriter writer, ulong value)
        {
            if (value <= 240)
            {
                writer.Put((byte)value);
                return;
            }
            if (value <= 2287)
            {
                writer.Put((byte)((value - 240) / 256 + 241));
                writer.Put((byte)((value - 240) % 256));
                return;
            }
            if (value <= 67823)
            {
                writer.Put((byte)249);
                writer.Put((byte)((value - 2288) / 256));
                writer.Put((byte)((value - 2288) % 256));
                return;
            }
            if (value <= 16777215)
            {
                writer.Put((byte)250);
                writer.Put((byte)(value & 0xFF));
                writer.Put((byte)((value >> 8) & 0xFF));
                writer.Put((byte)((value >> 16) & 0xFF));
                return;
            }
            if (value <= 4294967295)
            {
                writer.Put((byte)251);
                writer.Put((byte)(value & 0xFF));
                writer.Put((byte)((value >> 8) & 0xFF));
                writer.Put((byte)((value >> 16) & 0xFF));
                writer.Put((byte)((value >> 24) & 0xFF));
                return;
            }
            if (value <= 1099511627775)
            {
                writer.Put((byte)252);
                writer.Put((byte)(value & 0xFF));
                writer.Put((byte)((value >> 8) & 0xFF));
                writer.Put((byte)((value >> 16) & 0xFF));
                writer.Put((byte)((value >> 24) & 0xFF));
                writer.Put((byte)((value >> 32) & 0xFF));
                return;
            }
            if (value <= 281474976710655)
            {
                writer.Put((byte)253);
                writer.Put((byte)(value & 0xFF));
                writer.Put((byte)((value >> 8) & 0xFF));
                writer.Put((byte)((value >> 16) & 0xFF));
                writer.Put((byte)((value >> 24) & 0xFF));
                writer.Put((byte)((value >> 32) & 0xFF));
                writer.Put((byte)((value >> 40) & 0xFF));
                return;
            }
            if (value <= 72057594037927935)
            {
                writer.Put((byte)254);
                writer.Put((byte)(value & 0xFF));
                writer.Put((byte)((value >> 8) & 0xFF));
                writer.Put((byte)((value >> 16) & 0xFF));
                writer.Put((byte)((value >> 24) & 0xFF));
                writer.Put((byte)((value >> 32) & 0xFF));
                writer.Put((byte)((value >> 40) & 0xFF));
                writer.Put((byte)((value >> 48) & 0xFF));
                return;
            }
            // all others
            writer.Put((byte)255);
            writer.Put((byte)(value & 0xFF));
            writer.Put((byte)((value >> 8) & 0xFF));
            writer.Put((byte)((value >> 16) & 0xFF));
            writer.Put((byte)((value >> 24) & 0xFF));
            writer.Put((byte)((value >> 32) & 0xFF));
            writer.Put((byte)((value >> 40) & 0xFF));
            writer.Put((byte)((value >> 48) & 0xFF));
            writer.Put((byte)((value >> 56) & 0xFF));
        }
    }
}