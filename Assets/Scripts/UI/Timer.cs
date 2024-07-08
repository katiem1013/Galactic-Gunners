using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeValue;
    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        // checks to see if the time goes below 0
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        // shows a second if there is half a second less instead of 0
        else if (timeToDisplay > 0)
            timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // figures out the minutes 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // figures out the seconds

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
