using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HttpImage : MonoBehaviour
{
    public Image image;
    
    public void SetData(string url)
    {
        StartCoroutine(LoadAvt(url));
    }

    IEnumerator LoadAvt(string url)
    {
        var www = new WWW(url);
        yield return www;

        image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }
}