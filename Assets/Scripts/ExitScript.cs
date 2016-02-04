using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {

    public bool menu;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !menu)
            Application.Quit();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
