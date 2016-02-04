using UnityEngine;
using System.Collections;

public class Dimension1reset : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0.025f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player Body")
        {
            GameObject.Find("Player Body").transform.position = new Vector3(-22.49f, -4.28f, -1.943f);
            //GameObject.Find("Player Body").transform.rotation = Quaternion.Euler(-22.45f, 3.28f, -1.943f);
            transform.position = new Vector3(0, -13.4f, 0);
        }
    }
}
