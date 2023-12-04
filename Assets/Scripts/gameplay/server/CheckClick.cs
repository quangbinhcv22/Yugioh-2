using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckClick : MonoBehaviour
{
    public static Action<List<RaycastResult>> OnClick;
    private static readonly List<object> subscriber = new List<object>();

    private static readonly CheckClick Main;

    public GraphicRaycaster raycaster;
    

    static CheckClick()
    {
        Main = new GameObject("ClickCheck").AddComponent<CheckClick>();
        DontDestroyOnLoad(Main.gameObject);
    }


    public static void Subscribe(object source)
    {
        if (subscriber.Contains(source)) return;
        subscriber.Add(source);

        Main.gameObject.SetActive(subscriber.Any());
    }

    public static void UnSubscribe(object source)
    {
        subscriber.Remove(source);

        try
        {
            Main.gameObject.SetActive(subscriber.Any());
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var m_PointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            MainCanvas.raycaster.Raycast(m_PointerEventData, results);

            OnClick?.Invoke(results);
        }
    }
}