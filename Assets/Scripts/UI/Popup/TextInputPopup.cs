using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextInputPopup : Popup
{
    public Button submitButton;
    public TMP_InputField inputField;

    public static TextInputPopup SpawnPopup(string promptText, Action<string> onSubmit)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Popups/TextInputPopup");
        if (prefab == null)
        {
            Debug.LogError("TextInputPopup prefab not found!");
            return null;
        }

        // Instantiate under the canvas
        GameObject instance = Instantiate(prefab, MenuManager.instance.canvas.transform);
        TextInputPopup popup = instance.GetComponent<TextInputPopup>();

        popup.Setup(promptText, onSubmit);
        return popup;
    }

    private void Setup(string promptText, Action<string> onSubmit)
    {
        promptTextObject.text = promptText;

        submitButton.onClick.AddListener(() => {
            onSubmit?.Invoke(inputField.text);
            Destroy(gameObject);
        });
    }
}
