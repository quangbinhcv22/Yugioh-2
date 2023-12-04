using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
{
    public static T Current;

    protected virtual void OnEnable()
    {
        Current = GetComponent<T>();
    }
}