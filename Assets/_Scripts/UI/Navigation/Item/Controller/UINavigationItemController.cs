using UnityEngine;
using UnityEngine.UI;

public class UINavigationItemController : MonoBehaviour
{
    public static event OnItemClickedDelegate OnItemClicked;
    public delegate void OnItemClickedDelegate(Transform self);

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        Debug.Log("Clicked Navigation");

        OnItemClicked?.Invoke(transform);
    }
}
