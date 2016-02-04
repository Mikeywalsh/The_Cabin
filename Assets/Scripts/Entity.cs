using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public string objectName;
    public string description;
    public bool hasImage;
    public string imagePath;

    public virtual string UseItem(Pickupable item)
    {
        return "Nothing Happens";
    }

    public virtual string GetDescription()
    {
        return description;
    }
}