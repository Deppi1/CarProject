using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndZoneScript : MonoBehaviour
{
    public float startTimeWait = 0f;
    public float _entered;
    public Timer time;
    public Text taskDescription;
    public GameObject mainPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (time.seconds > startTimeWait)
        {
            _entered++;
            if (_entered == 1)
            {
                mainPanel.SetActive(true);
                taskDescription.text = "Поздравляем! \n Вы прошли уровень за: " + time.minutes.ToString() + ":" + time.seconds.ToString("F3");
            }
        }
    }
}
