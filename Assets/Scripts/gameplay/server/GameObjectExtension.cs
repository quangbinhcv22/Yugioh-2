using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static List<Transform> GetAllChild(this Transform _t)
    {
        List<Transform> ts = new List<Transform>();

        foreach (Transform t in _t)
        {
            ts.Add(t);
            if (t.childCount > 0)
                ts.AddRange(GetAllChild(t));
        }

        return ts;
    }
}