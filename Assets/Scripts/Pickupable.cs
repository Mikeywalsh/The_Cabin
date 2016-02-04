using UnityEngine;
using System.Collections;

public sealed class Pickupable : Entity {

    public string iconPath;
    public string pickedUpDescription;
    public bool unlimited;

    public string GetPickedUpDescription()
    {
        if (pickedUpDescription == "")
            return base.GetDescription();

        return pickedUpDescription;
    }
}
