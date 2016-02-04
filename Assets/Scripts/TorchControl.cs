using UnityEngine;
using System.Collections;

public class TorchControl : MonoBehaviour {

    float colorLerpTime;
    float intensityLerpTime;

    float lastFlicker;
    float flickermodifier;

	void Start () {
	
	}
	
	void Update () {
        if(Time.time - lastFlicker > 0.1f + flickermodifier)
        {
            GetComponent<Light>().intensity = Random.Range(3.5f, 7);
            lastFlicker = Time.time;
            flickermodifier = Random.Range(-0.05f, 0.05f);
        }

	}
}
