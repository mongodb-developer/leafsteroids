using UnityEngine;

namespace _6_Main
{
    public static class Helper
    {
        public static Vector3 InvertY(Vector3 vector)
        {
            return new Vector3(vector.x, -vector.y, vector.z);
        }
    }
}