using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public sealed class InventoryHandler : MonoBehaviour {

    public bool usingItem;
    public Pickupable currentSelection;
    private int selectedIndex;
    private List<Pickupable> _inventory = new List<Pickupable>();

    void Start()
    {
        Object.DontDestroyOnLoad(gameObject);
        Object.DontDestroyOnLoad(GameObject.Find("Player Body"));
        Object.DontDestroyOnLoad(GameObject.Find("UI Canvas"));

    }
    void Update()
    {
        int i = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            i = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            i = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            i = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            i = 3;

        if (i != -1 && _inventory.Count >= i + 1)
        {
            if (usingItem && i != selectedIndex || !usingItem)
            {
                StopUse();
                usingItem = true;
                currentSelection = _inventory[i];
                selectedIndex = i;
                GameObject.Find("Inventory Slot " + (i + 1).ToString()).GetComponent<Image>().color = Color.yellow;
                Interaction.OutputMessage(currentSelection.description);
            }
            else
                StopUse();
        }
    }

    public List<Pickupable> Inventory
    {
        get { return _inventory; }
    }

    public bool AddItem(Pickupable itemToAdd)
    {
        if (_inventory.Count >= 4)
            return false;
        
        _inventory.Add(itemToAdd);

        GameObject.Find("Inventory Slot " + _inventory.Count.ToString()).transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(itemToAdd.iconPath);

        if (itemToAdd.objectName == "Stick")
            GameObject.Find("Player Head").GetComponent<Interaction>().AdvanceTutorial(2);

        return true;
    }

    public bool RemoveItem(Pickupable item)
    {
        int index = -1;

        for(int i = 0; i < _inventory.Count; i ++)
        {
            if (_inventory[i].objectName == item.objectName)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return false;

        _inventory.RemoveAt(index);
        GameObject.Find("Inventory Slot " + (index + 1).ToString()).transform.FindChild("Image").GetComponent<Image>().sprite = new Sprite();
        for (int i = index; i < _inventory.Count; i ++)
        {
            GameObject.Find("Inventory Slot " + (i + 1).ToString()).transform.FindChild("Image").GetComponent<Image>().sprite = GameObject.Find("Inventory Slot " + (i + 2).ToString()).transform.FindChild("Image").GetComponent<Image>().sprite;
        }
        GameObject.Find("Inventory Slot " + (_inventory.Count + 1).ToString()).transform.FindChild("Image").GetComponent<Image>().sprite = new Sprite();

        return true;
    }

    public void ExamineSelected()
    {
        Interaction.OutputMessage(currentSelection.description);
    }

    public void StopUse()
    {
        for(int i = 0; i < 4; i ++)
            GameObject.Find("Inventory Slot " + (i + 1).ToString()).GetComponent<Image>().color = Color.white;

        usingItem = false;
    }
}