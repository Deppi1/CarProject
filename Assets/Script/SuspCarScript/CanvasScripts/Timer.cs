using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public InputSystem inputSystem;
    public bool timerStarted = false;
    public float seconds;
    public float minutes;
    public Text secondsTextTimer;
    // Start is called before the first frame update
    void Start()
    {
        secondsTextTimer.text = "Time: " + minutes.ToString() + ":" + seconds.ToString("F3");
    }

    // Update is called once per frame
    void Update()
    {

        if (inputSystem.TotalGas == 1)
        {
            timerStarted = true;
        }

        if (timerStarted)
        {
            seconds += Time.deltaTime;
            secondsTextTimer.text = "Time: " + minutes.ToString() + ":" + seconds.ToString("F3");

            if(seconds > 60f)
            {
                seconds -= 60f;
                minutes++;
            }
        }
    }
    private void OnEnable()
    {
        InputSystem.sendInputSystem.AddListener(GetInputSystem);
    }

    private void GetInputSystem(InputSystem input)
    {
        inputSystem = input;
        InputSystem.sendInputSystem.RemoveListener(GetInputSystem);
    }
}
