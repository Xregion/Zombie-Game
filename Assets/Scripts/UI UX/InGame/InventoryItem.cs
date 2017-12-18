using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

    public GameObject tooltip;

    Bounds bounds;
    Image image;
    float timer;
    bool isTooltipShown;

    void Start () {
        //tooltip = GetComponentInChildren<GameObject>();
        image = GetComponentsInChildren<Image>()[1];
        Text text = tooltip.GetComponent<Text>();
        text.text = name;
        image.gameObject.SetActive(false);
        RectTransform rectTransform = GetComponent<RectTransform>();
        timer = 0;
        bounds = new Bounds(transform.position, new Vector3(rectTransform.rect.width, rectTransform.rect.height, 0));
	}
	
	void Update () {
        if (bounds.Contains(Input.mousePosition) && !isTooltipShown)
            timer += Time.deltaTime;
        else
            timer = 0;

        if (timer > 0.5 && !isTooltipShown)
        {
            tooltip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            image.gameObject.SetActive(true);
            isTooltipShown = true;
        }

        if (isTooltipShown && !bounds.Contains(Input.mousePosition))
        {
            image.gameObject.SetActive(false);
            isTooltipShown = false;
        }
    }
}
