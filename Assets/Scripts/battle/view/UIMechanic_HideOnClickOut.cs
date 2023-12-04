using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMechanic_HideOnClickOut : MonoBehaviour
{
    private List<Transform> allObjects;

    protected void OnEnable()
    {
        allObjects = new List<Transform>
        {
            transform,
        };

        allObjects = transform.GetAllChild();

        CheckClick.Subscribe(gameObject);
    }

    private void OnDisable()
    {
        CheckClick.UnSubscribe(gameObject);
    }

    private void OnClick(List<RaycastResult> raycastResults)
    {
        var clickMe = allObjects.Any(o => raycastResults.Any(r => o.gameObject == r.gameObject));
        if (!clickMe) gameObject.SetActive(false);
    }
}