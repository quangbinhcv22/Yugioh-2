using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{
    public List<GameObject> prefabs;

    private void Start()
    {
        foreach (var prefab in prefabs)
        {
            var instance = Instantiate(prefab, MainCanvas.Transform);
            instance.gameObject.SetActive(false);
        }
    }
}