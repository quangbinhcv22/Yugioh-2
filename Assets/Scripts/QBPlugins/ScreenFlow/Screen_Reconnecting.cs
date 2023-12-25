using UnityEngine;

public class Screen_Reconnecting : MonoBehaviour
{
    public static Screen_Reconnecting singleton;
    
    private void Awake()
    {
        singleton = this;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}