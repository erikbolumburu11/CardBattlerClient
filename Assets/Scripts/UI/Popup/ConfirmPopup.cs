using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : Popup
{
    public Button confirmButton;
    public Button rejectButton;

    public static ConfirmPopup SpawnPopup(string promptText, Action onConfirm, Action onReject = null)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Popups/ConfirmPopup");
        if (prefab == null)
        {
            Debug.LogError("ConfirmPopup prefab not found!");
            return null;
        }

        // Instantiate under the canvas
        GameObject instance = Instantiate(prefab, MenuManager.instance.canvas.transform);
        ConfirmPopup popup = instance.GetComponent<ConfirmPopup>();

        popup.Setup(promptText, onConfirm, onReject);
        return popup;
    }

    private void Setup(string promptText, Action onConfirm, Action onReject = null)
    {
        promptTextObject.text = promptText;

        confirmButton.onClick.AddListener(() => {
            onConfirm?.Invoke();
            Destroy(gameObject);
        });

        rejectButton.onClick.AddListener(() => {
            onReject?.Invoke();
            Destroy(gameObject);
        });
    }
}
