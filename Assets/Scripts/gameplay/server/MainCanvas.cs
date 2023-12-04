using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(int.MinValue)]
public class MainCanvas : MonoBehaviour
{
    public static Transform Transform;
    public static GraphicRaycaster raycaster;
    
    private void OnEnable()
    {
        Transform = transform;
        raycaster = GetComponent<GraphicRaycaster>();
    }
}