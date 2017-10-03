using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour {

    Text interactionText;

    void Start()
    {
        interactionText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void SetText(string newText)
    {
        interactionText.text = newText;
    }

    public void EnableDialogue(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}
