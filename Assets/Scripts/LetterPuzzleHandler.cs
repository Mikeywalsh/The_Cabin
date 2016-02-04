using UnityEngine;
using System.Collections;

public class LetterPuzzleHandler : MonoBehaviour
{

    private static GameObject puzzleHolder;
    public GameObject bridge;
    public int order;

    static int currentStep;
    static float failTime;
    static bool failed;

    void Start()
    {
        puzzleHolder = GameObject.Find("Letter Puzzle Platforms");
    }

    void Update()
    {
        if (Time.time - failTime > 1 && failed)
        {
            failed = false;
            ResetPuzzle();
        }
    }

    private static void ResetPuzzle()
    {
        currentStep = 0;
        for (int i = 0; i < puzzleHolder.transform.childCount - 1; i++)
        {
            puzzleHolder.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
            //puzzleHolder.transform.GetChild(i).GetComponent<LetterPuzzleHandler>().completetime = Time.time;
            //puzzleHolder.transform.GetChild(i).GetComponent<LetterPuzzleHandler>().fallTime = Random.Range(0.1f, 4f);
            //puzzleHolder.transform.GetChild(i).GetComponent<LetterPuzzleHandler>().falling = true;
        }
        GameObject.Find("Player Body").transform.position = Vector3.zero;
        GameObject.Find("Player Body").transform.rotation = Quaternion.Euler(0, 90, 0);
        GameObject.Find("Player Head").transform.rotation = Quaternion.Euler(12, 0, 0);

    }

    void OnTriggerEnter(Collider other)
    {
        if(order == -1)
        {
            ResetPuzzle();
            return;
        }

        if (other.gameObject.name == "Player Body")
        {
            if (order == currentStep + 1)
            {
                currentStep++;
                GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                currentStep = 0;
                GetComponent<Renderer>().material.color = Color.red;
                failTime = Time.time;
                failed = true;

            }

            if (currentStep == 7)
                bridge.SetActive(true);
        }
    }
}
