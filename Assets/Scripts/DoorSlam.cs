using UnityEngine;
using System.Collections;

public class DoorSlam : MonoBehaviour {


    public GameObject door;
    public bool slammed;
	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player Body" && !slammed)
        {
            GetComponent<AudioSource>().Play();
            slammed = true;
        }
    }
}
