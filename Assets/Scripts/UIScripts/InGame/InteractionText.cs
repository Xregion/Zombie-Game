using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour {

    Text interactionText;
    Image border;

    void Start()
    {
        interactionText = GetComponentInChildren<Text>();
        border = GetComponentInChildren<Image>();
        EnableDialogue(false);
    }

    public void SetText(string newText)
    {
        interactionText.text = newText;
    }

    public void EnableDialogue(bool enabled)
    {
        interactionText.gameObject.SetActive(enabled);
        border.gameObject.SetActive(enabled);
    }
}
