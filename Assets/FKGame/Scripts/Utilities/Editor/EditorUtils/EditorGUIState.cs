using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    // 保存object的UI状态，使得可以保存折叠状态
    public static class EditorGUIState
    {
        private static int count = 0;
        private static Dictionary<object, Hasher> objectHasherDic = new Dictionary<object, Hasher>();
        private static Dictionary<int, bool> hasherDic = new Dictionary<int, bool>();
        private static Dictionary<int, Vector2> hasherPosDic = new Dictionary<int, Vector2>();

        public static bool GetState(object obj)
        {
            AddObject(obj);
            int h = objectHasherDic[obj].GetHashCode();
            return hasherDic[h];
        }

        public static void SetState(object obj, bool state)
        {
            AddObject(obj);
            int h = objectHasherDic[obj].GetHashCode();
            hasherDic[h] = state;
        }

        public static Vector2 GetVector2(object obj)
        {
            AddObject(obj);
            int h = objectHasherDic[obj].GetHashCode();
            return hasherPosDic[h];
        }

        public static void SetVector2(object obj, Vector2 pos)
        {
            AddObject(obj);
            int h = objectHasherDic[obj].GetHashCode();
            hasherPosDic[h] = pos;
        }

        private static void AddObject(object obj)
        {
            if (!objectHasherDic.ContainsKey(obj))
            {
                Hasher hasher = new Hasher(count).Hash(obj);
                objectHasherDic.Add(obj, hasher);
                int hashCode = hasher.GetHashCode();
                hasherDic.Add(hashCode, false);
                hasherPosDic.Add(hashCode, Vector2.zero);
                count++;
                if (count >= int.MaxValue)
                    count = 0;
            }
        }

        internal class Hasher
        {
            private int _hashCode;

            public Hasher()
            {
                _hashCode = 17;
            }

            public Hasher(int seed)
            {
                _hashCode = seed;
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }

            public Hasher Hash(bool obj)
            {
                _hashCode = 37 * _hashCode + obj.GetHashCode();
                return this;
            }

            public Hasher Hash(int obj)
            {
                _hashCode = 37 * _hashCode + obj.GetHashCode();
                return this;
            }

            public Hasher Hash(long obj)
            {
                _hashCode = 37 * _hashCode + obj.GetHashCode();
                return this;
            }

            public Hasher Hash<T>(Nullable<T> obj) where T : struct
            {
                _hashCode = 37 * _hashCode + ((obj == null) ? -1 : obj.Value.GetHashCode());
                return this;
            }

            public Hasher Hash(object obj)
            {
                _hashCode = 37 * _hashCode + ((obj == null) ? -1 : obj.GetHashCode());
                return this;
            }

            public Hasher HashElements(IEnumerable sequence)
            {
                if (sequence == null)
                {
                    _hashCode = 37 * _hashCode + -1;
                }
                else
                {
                    foreach (var value in sequence)
                    {
                        _hashCode = 37 * _hashCode + ((value == null) ? -1 : value.GetHashCode());
                    }
                }
                return this;
            }

            public Hasher HashStructElements<T>(IEnumerable<T> sequence) where T : struct
            {
                foreach (var value in sequence)
                {
                    _hashCode = 37 * _hashCode + value.GetHashCode();
                }
                return this;
            }
        }
    }
}
