using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public sealed class Interaction : MonoBehaviour {

    public bool examiningImage;
    
    public static GameObject player;

    public GameObject tutorialPanel1;
    public GameObject tutorialPanel2;
    public GameObject tutorialPanel3;
    public GameObject tutorialWalls;
    public GameObject torch;
    public byte tutorialProgress;

    public bool rugMoved;
    public bool doorOpened;
    public bool redOrbDown;
    public bool blueOrbDown;

    public GameObject messagePanel;
    public GameObject inventoryPanel;
    public GameObject sceneHandler;
    public GameObject examineImage;
    public GameObject examinePanel;
    public GameObject interactImage;
    public GameObject useImage;
    public GameObject canUseImage;
    private Ray playerFixation;

    public Vector3 spawnLocation = new Vector3(28.7f,6.7f,29.68f);
    public bool levelSwitched;

	
    void Start()
    {
        player = GameObject.Find("Player Body");
        levelSwitched = false;
    }

	void Update () {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 2.5f, Color.red);

        //if (tutorialProgress == 0)
        //{
        //    fadeCounter++;

        //    fadePanel.GetComponent<Image>().material.color = new Color(0, 0, 0, 1 - (fadeCounter / 255f));

        //    if (fadeCounter == 255)
        //    {
        //        fadePanel.SetActive(false);
        //        AdvanceTutorial(1);
        //    }
        //}

        if (tutorialProgress >= 3 && Input.GetKeyDown(KeyCode.F))
        {
            if (tutorialProgress == 3)
                AdvanceTutorial(4);

            torch.SetActive(!torch.activeSelf);
        }

        if(levelSwitched)
        {
            transform.parent.position = spawnLocation;
            levelSwitched = false;
        }

        playerFixation = new Ray(transform.position, transform.forward * 1.6f);
        examineImage.SetActive(false);
        interactImage.SetActive(false);
        useImage.SetActive(false);
        canUseImage.SetActive(false);

        if (examiningImage)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                ExitExamineImage();
            return;
        }

        if (sceneHandler.GetComponent<InventoryHandler>().usingItem)
        {
            useImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(sceneHandler.GetComponent<InventoryHandler>().currentSelection.iconPath);
            useImage.SetActive(true);
        }

        //Execute code if the player is looking at an object with a collider near them
        if (Physics.Raycast(playerFixation, out hit))
        {
            if (hit.collider.GetComponent<Entity>() != null)
            {
                //Allow the player to try and use the current selected object on a world object
                if (sceneHandler.GetComponent<InventoryHandler>().usingItem && Input.GetMouseButtonDown(0))
                {
                    TryUseItem(sceneHandler.GetComponent<InventoryHandler>().currentSelection, hit.collider.GetComponent<Entity>());
                    return;
                }

                //Dont show examine and interact images options to the player if they have selected an object from their inventory, but show if they can use an item on an object
                if (sceneHandler.GetComponent<InventoryHandler>().usingItem)
                {
                    canUseImage.SetActive(true);
                    return;
                }

                //Show examine image if the object is examinable
                examineImage.SetActive(true);

                //Allow the player to examine the object if they press E whilst looking at it
                if (Input.GetMouseButtonDown(1))
                {
                    if(hit.collider.GetComponent<Entity>().hasImage)
                    {
                        examinePanel.SetActive(true);
                        messagePanel.SetActive(false);
                        inventoryPanel.SetActive(false);
                        examiningImage = true;
                        GameObject.Find("Player Body").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
                        examinePanel.GetComponent<AudioSource>().Play();
                        examinePanel.transform.FindChild("Examine Large Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(hit.collider.GetComponent<Entity>().imagePath);
                    }
                    else
                        OutputMessage(hit.collider.GetComponent<Entity>().GetDescription());
                    return;
                }

                //Now check if the player is looking at a pickupable object
                if (hit.collider.GetComponent<Pickupable>() != null)
                {
                    //Show interact image if the object is interactable
                    interactImage.SetActive(true);

                    //Allow the player to pick up the object if they left click whilst looking at it
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Add the item to the players inventory if it is not full
                        if (GameObject.Find("Scene Handler").GetComponent<InventoryHandler>().AddItem(hit.collider.GetComponent<Pickupable>()))
                        {
                            OutputMessage("Picked up the " + hit.collider.GetComponent<Pickupable>().objectName + "!");
                            if(!hit.collider.GetComponent<Pickupable>().unlimited)
                                Destroy(hit.collider.gameObject);
                        }
                        else
                            OutputMessage("Cannot pick up the " + hit.collider.GetComponent<Pickupable>().objectName + ". Your inventory is too full!");
                        return;
                    }
                }

                //Now check if the player is looking at an interactable object
                if (hit.collider.GetComponent<Interactable>() != null && hit.collider.GetComponent<Interactable>().isInteractable)
                {
                    //Show interact image if the object is interactable
                    interactImage.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        OutputMessage(hit.collider.GetComponent<Interactable>().Interact());
                    }
                }
            }
        }
	}

    public static void OutputMessage(string message)
    {
        GameObject.Find("Message Text").GetComponent<ScrollingText>().UpdateText(message);
    }

    public void ExitExamineImage()
    {
        examinePanel.SetActive(false);
        messagePanel.SetActive(true);
        inventoryPanel.SetActive(true);
        GameObject.Find("Player Body").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        examiningImage = false;
    }

    private bool TryUseItem(Pickupable item, Entity target)
    {
        OutputMessage(target.UseItem(item));

        sceneHandler.GetComponent<InventoryHandler>().StopUse();
        return false;
    }

    public void AdvanceTutorial(byte stage)
    {
        tutorialPanel1.SetActive(false);
        tutorialPanel2.SetActive(false);
        tutorialPanel3.SetActive(false);

        if(stage == 1)
            tutorialPanel1.SetActive(true);
        else if (stage == 2)
            tutorialPanel2.SetActive(true);
        else if (stage == 3)
        {
            tutorialPanel3.SetActive(true);
            torch.SetActive(true);
            Destroy(GameObject.Find("Tutorial Walls"));
        }

        tutorialProgress = stage;
    }

    void OnLevelWasLoaded(int level)
    {
        if (rugMoved && level == 1)
        {
            GameObject.Find("Rug").GetComponent<Interactable>().trapdoor.SetActive(true);
            Destroy(GameObject.Find("Rug"));
        }
        if (doorOpened && level == 2)
            Destroy(GameObject.Find("Shrine Door"));
        if(redOrbDown && level == 2)
        {
            GameObject.Find("Red Pedastel").GetComponent<Interactable>().redOrb.SetActive(true);
        }
        if (blueOrbDown && level == 2)
        {
            GameObject.Find("Blue Pedastel").GetComponent<Interactable>().blueOrb.SetActive(true);
        }

        Debug.Log(level.ToString());
    }
}
