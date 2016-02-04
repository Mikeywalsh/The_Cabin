using UnityEngine;
using UnityEngine.UI;

public sealed class ScrollingText : MonoBehaviour {

    public float speed;
    public AudioClip textSound;

    private string textToPrint;
    private int currentIndex;
    private float lastCharTime;
    private bool finished;
    private int charsSinceLastSound;
	
    void Start()
    {
        finished = true;
    }

    void Update()
    {
        if (finished)
            return;

        if(Time.time - lastCharTime >= 0.1f * (1 / speed))
        {
            GetComponent<Text>().text += textToPrint[currentIndex];
            currentIndex++;
            charsSinceLastSound++;
            if(charsSinceLastSound % 4 == 0)
                GetComponent<AudioSource>().Play();
            if (currentIndex == textToPrint.Length)
                finished = true;
            lastCharTime = Time.time;
        }
    }

	public void UpdateText (string text)
    {
        GetComponent<Text>().text = "";
        if (text.Length == 0)
            return;

        currentIndex = 0;
        charsSinceLastSound = 0;
        textToPrint = text;
        finished = false;
    }
}
