using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button_ClosePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Close);
    }

    private void Close()
    {
        panel.SetActive(false);
    }
}