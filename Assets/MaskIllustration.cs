using UnityEngine;

public class MaskIllustration : MonoBehaviour
{
    public Transform root;
    public Transform content;


    private void Update()
    {
        if (transform.eulerAngles != Vector3.zero)
        {
            transform.eulerAngles = Vector3.zero;
            content.eulerAngles = root.eulerAngles;
        }
    }
}