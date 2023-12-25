using System.Collections.Generic;
using UnityEngine;

public class ChildLoader : MonoBehaviour
{
    public List<GameObject> prefabs;

    private void Start()
    {
        foreach (var prefab in prefabs)
        {
            var instance = Instantiate(prefab, transform);
            instance.gameObject.SetActive(false);
        }
    }
}