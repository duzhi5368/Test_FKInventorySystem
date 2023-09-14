using System;
using UnityEngine;
//------------------------------------------------------------------------
namespace FKGame
{
    public static class VectorExtends
    {
        // 向量逆时针旋转
        public static Vector3 Vector3RotateInXZ(this Vector3 dir, float angle)
        {
            angle *= Mathf.Deg2Rad;
            float l_n_dirX = dir.x * Mathf.Cos(angle) - dir.z * Mathf.Sin(angle);
            float l_n_dirZ = dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);
            Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

            return l_dir;
        }

        // 向量顺时针
        public static Vector3 Vector3RotateInXZ2(this Vector3 dir, float angle)
        {

            angle *= Mathf.Deg2Rad;
            float l_n_dirX = dir.x * Mathf.Cos(angle) + dir.z * Mathf.Sin(angle);
            float l_n_dirZ = -dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);

            Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

            return l_dir;
        }

        // 位置绕点旋转顺时针，逆时针角度乘以-1即可
        public static Vector3 PostionRotateInXZ(this Vector3 pos, Vector3 center, float angle)
        {
            angle *= -Mathf.Deg2Rad;
            float x = (pos.x - center.x) * Mathf.Cos(angle) - (pos.z - center.z) * Mathf.Sin(angle) + center.x;
            float z = (pos.x - center.x) * Mathf.Sin(angle) + (pos.z - center.z) * Mathf.Cos(angle) + center.z;

            Vector3 newPos = new Vector3(x, pos.y, z);

            return newPos;
        }

        //获取一个顺时针夹角(需先标准化向量)
        public static float GetRotationAngle(this Vector3 dir, Vector3 aimDir)
        {
            //dir = dir.normalized;
            //aimDir = aimDir.normalized;

            float angle = (float)(Math.Acos(Vector3.Dot(dir, aimDir)) * 180 / Math.PI);

            if (angle != 180 && angle != 0)
            {
                float cross = dir.x * aimDir.z - aimDir.x * dir.z;
                if (cross < 0)
                {
                    return angle;
                }
                else
                {
                    return 360 - angle;
                }
            }
            return angle;
        }
    }
}