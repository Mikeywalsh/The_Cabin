using UnityEngine;
using System.Collections.Generic;

public class Interactable : Entity {

    public GameObject key;
    public GameObject purplePort;
    public GameObject redOrb;
    public GameObject blueOrb;

    public AudioClip doorCantOpen;
    public AudioClip doorOpen;
    public AudioClip doorSlam;
    public AudioClip mixPotion;

    public GameObject trapdoor;

    public bool isInteractable;
    public bool locked;

    private List<string> mixerContents = new List<string>();

    public override string UseItem(Pickupable item)
    {
        if(objectName == "Campfire" && item.objectName == "Stick")
        {
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().AddItem(GameObject.Find("Torch Pickupable").GetComponent<Pickupable>());
            GameObject.Find("Player Head").GetComponent<Interaction>().AdvanceTutorial(3);
            return "You light the torch on the campfire!";
        }
        else if (objectName == "Door" && item.objectName == "Key")
        {
            transform.Translate(0, 100, 0);
            isInteractable = false;
            locked = false;
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            return "You open the door with the key!";
            Destroy(gameObject);
        }
        else if (objectName == "Bin" && item.objectName.Contains("Potion"))
        {
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            return "You throw away the potion.";
        }
        else if (objectName == "Bin" && item.objectName.Contains("Vial"))
        {
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            return "You throw away the vial.";
        }
        else if (objectName == "Bin")
        {
            return "You don't want to throw this away...";
        }
        else if(objectName == "Red Pedestal" && item.objectName == "Red Orb")
        {
            redOrb.SetActive(true);
            if (GameObject.Find("Blue Orb") != null && GameObject.Find("Red Orb") != null)
            {
                purplePort.SetActive(true);
            }
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            GameObject.Find("Player Head").GetComponent<Interaction>().redOrbDown = true;
            return "You place the orb on the pedestal";
        }
        else if (objectName == "Blue Pedestal" && item.objectName == "Blue Orb")
        {
            blueOrb.SetActive(true);
            if (GameObject.Find("Blue Orb") != null && GameObject.Find("Red Orb") != null)
            {
                purplePort.SetActive(true);
            }
            GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
            GameObject.Find("Player Head").GetComponent<Interaction>().blueOrbDown = true;
            return "You place the orb on the pedestal";
        }
        else if (objectName == "Mould" && item.objectName.Contains("Potion"))
        {
            if (mixerContents.Count < 2)
            {
                GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
                mixerContents.Add(item.objectName);
                GetComponent<AudioSource>().clip = mixPotion;
                GetComponent<AudioSource>().Play();
                return "You pour the potion into the mould.";
            }
            else if (mixerContents.Count == 2)
            {
                GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().RemoveItem(item);
                mixerContents.Add(item.objectName);
                GetComponent<AudioSource>().clip = mixPotion;
                GetComponent<AudioSource>().Play();
                if (mixerContents[0] == "Red Potion" && mixerContents[1] == "Green Potion" && mixerContents[2] == "Blue Potion")
                {
                    key.SetActive(true);
                    gameObject.SetActive(false);
                    return "You pour the potion into the mould. It hardens into a key.";
                }
                else
                {
                    return "The mould is now full, but it isn't hardening.";
                }
            }
            else if (mixerContents.Count == 2)
            {
                return "The mould is full, you cannot add any more potions.";
            }
        }
        return "Nothing Happens...";
    }

    public override string GetDescription()
    {
        if (objectName == "Door" && locked)
            return description + " It's Locked.";
        else if (objectName == "Door" && !locked)
            return description + " It's Open!";

        return base.GetDescription();        
    }

    public string Interact()
    {
        if (objectName == "Door")
        {
            //Play locked door sound
            GetComponent<AudioSource>().clip = doorCantOpen;
            GetComponent<AudioSource>().Play();
            return "You try to open the door but it does not budge...";
        }
        else if(objectName == "Mould")
        {
            mixerContents.Clear();
            return "You empty the Mould!";
        }
        else if(objectName == "Rucksack")
        {
            return "You search the bag but find nothing.";
        }
        else if(objectName == "Rug")
        {
            transform.localPosition = new Vector3(-15, -1,37);
            trapdoor.SetActive(true);
            isInteractable = false;
            Destroy(gameObject);
            GameObject.Find("Player Head").GetComponent<Interaction>().rugMoved = true;
            return "You move the rug and discover a trapdoor!";
        }
        else if(objectName == "Trap Door")
        {
            GameObject.Find("Player Head").GetComponent<Interaction>().levelSwitched = true;
            GameObject.Find("Player Head").GetComponent<Interaction>().spawnLocation = new Vector3(-16, -0.5f, 0);

            Application.LoadLevel(2);
            return "You open the trapdoor and climb down...";
        }
        else if(objectName == "Ladder")
        {
            GameObject.Find("Player Head").GetComponent<Interaction>().levelSwitched = true;
            GameObject.Find("Player Head").GetComponent<Interaction>().spawnLocation = new Vector3(-111.27f,7.61f,24.1f);
            Application.LoadLevel(1);
            return "You climb back up the ladder...";
        }
        else if(objectName == "Red Portal")
        {
            GameObject.Find("Player Head").GetComponent<Interaction>().levelSwitched = true;
            GameObject.Find("Player Head").GetComponent<Interaction>().spawnLocation = new Vector3(-22.49f,-4.28f,-1.943f);
            Application.LoadLevel(3);
            return "You step throught the portal";

        }
        else if(objectName.Contains("Exit Portal"))
        {
            GameObject.Find("Player Head").GetComponent<Interaction>().levelSwitched = true;
            GameObject.Find("Player Head").GetComponent<Interaction>().spawnLocation = new Vector3(-7.75f,1.75f,55.4f);
            GameObject.Find("Player Head").GetComponent<Interaction>().doorOpened = true;
            Application.LoadLevel(2);
            return "You step throught the portal";
        }
        else if(objectName == "Blue Portal")
        {
            GameObject.Find("Player Head").GetComponent<Interaction>().levelSwitched = true;
            GameObject.Find("Player Head").GetComponent<Interaction>().spawnLocation = new Vector3(0,0,0);
            Application.LoadLevel(4);
            return "You step throught the portal";
        }
        else if(objectName == "Purple Portal")
        {
            Application.LoadLevel(5);
            return "You step throught the portal";
        }

        return "Error 10110010010000100";
    }
}
