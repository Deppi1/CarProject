using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public InputSystem inputSystem;
    void Update()
    {
        if (inputSystem.KeyI == 1)
        {
            mainPanel.SetActive(true);
        }

        if (mainPanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
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
