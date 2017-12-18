using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public GameObject itemBox;
    public Sprite[] icons;

    List<GameObject> itemsInInventory = new List<GameObject>();
    GameObject container;
	
	void Start () {
        container = new GameObject("container");
        container.transform.parent = transform;
        UpdateInventory();
        container.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            container.SetActive(!container.activeSelf);
    }

    public void UpdateInventory()
    {
        int currentFile = SaveManager.data.LoadedFile;
        string data = SaveManager.data.GetSaveData(currentFile);
        if (data.Equals("Empty"))
            return;

        string[] splitData = data.Split('\n');
        string[] items = splitData[8].Split(':')[1].Split(',');
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = items[i].Trim();

            for (int j = 0; j < icons.Length; j++)
            {
                if (items[i].ToLower().Equals(icons[j].name.ToLower()))
                {
                    
                    Vector3 nextPosition;
                    if (itemsInInventory.Count > 0)
                    {
                        if (itemsInInventory[i].name == items[i])
                            continue;
                        Vector3 previousPosition = itemsInInventory[itemsInInventory.Count - 1].transform.position;
                        float offset = previousPosition.y - itemBox.GetComponent<RectTransform>().rect.height;
                        nextPosition = new Vector3(previousPosition.x, offset);
                    }
                    else
                        nextPosition = transform.position;

                    GameObject newItemBox = Instantiate(itemBox, nextPosition, Quaternion.identity, container.transform);
                    itemsInInventory.Add(newItemBox);
                    newItemBox.GetComponent<Image>().sprite = icons[j];
                    newItemBox.name = items[i];
                }
            }
        }
    }
}
