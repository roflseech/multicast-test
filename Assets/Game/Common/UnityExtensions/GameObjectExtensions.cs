using UnityEngine;

namespace Game.Common.UnityExtensions
{
    public static class GameObjectExtensions
    {
        public static void ClearObjectsUnderTransform(this Transform transform)
        {
            foreach (Transform t in transform)
            {
                if (t != transform) GameObject.Destroy(t.gameObject);
            }
        }
    }
}