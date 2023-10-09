using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI timer;

    public float timeRemaining = 180; // 3 minutes in seconds
    private bool timerIsRunning = false;

    private void Start()
    {
        // Start the countdown timer as soon as the game starts
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                // Time has run out!
                timeRemaining = 0;
                timerIsRunning = false;

                // Perform any actions needed when the timer runs out
                TimerFinished();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Calculate minutes and seconds from the total time in seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Update the TextMeshProUGUI text
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        SceneManager.LoadScene(3);
    }
}
