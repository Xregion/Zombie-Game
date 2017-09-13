using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoBehaviour {

    Text interactionText;

    void Start()
    {
        gameObject.SetActive(false);
        interactionText = GetComponentInChildren<Text>();
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
