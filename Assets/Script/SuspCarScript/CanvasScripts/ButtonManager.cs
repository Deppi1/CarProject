using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public Timer time;

    public GameObject mainPanel;
    public GameObject bodyPanel;
    public GameObject footerPanel;
    public GameObject[] _childBody;
    public GameObject[] _childButton;
    public GameObject levels;
    public GameObject[] _level;
    public int _windowsNow;

    public GameObject car;

    private void Start()
    {
        _childBody = new GameObject[bodyPanel.transform.childCount];
        _childButton = new GameObject[footerPanel.transform.childCount];
        _level = new GameObject[levels.transform.childCount];


        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            _childBody[i] = bodyPanel.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < footerPanel.transform.childCount; i++)
        {
            _childButton[i] = footerPanel.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < _level.Length; i++)
        {
            _level[i] = levels.transform.GetChild(i).gameObject;
        }


    }

    public void ButtonBack()
    {
        if(_windowsNow == 1)
        {
            for (int i = 0; i < bodyPanel.transform.childCount; i++)
            {
                _childButton[i].SetActive(true);
            }
            _childButton[1].SetActive(false);
            _childButton[2].SetActive(false);
            _childButton[4].SetActive(true);
        }

        if(_windowsNow != 0)
        {
            for(int i = 0; i < bodyPanel.transform.childCount; i++)
            {
                _childBody[i].SetActive(false);
            }
            _windowsNow--;
            _childBody[_windowsNow].SetActive(true);
            _childButton[1].SetActive(false);
        }
    }

    public void ButtonLevelChoice()
    {
        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            for (int y = 0; y < bodyPanel.transform.childCount; y++)
            {
                _childBody[y].SetActive(false);
            }
            _windowsNow = 1;
            _childBody[1].SetActive(true);

            _childButton[i].SetActive(true);
        }
        _childButton[0].SetActive(false);
        _childButton[1].SetActive(false);
        _childButton[4].SetActive(false);

    }

    public void ButtonClose()
    {
        _windowsNow = 0;
            
        time.seconds = 0;
        time.minutes = 0;
        time.timerStarted = false;

        for (int i = 1; i < _level.Length; i++)
        {
            _level[i].transform.GetChild(0).GetComponent<EndZoneScript>()._entered = 0;
        }

        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            _childBody[i].SetActive(false);
            _childButton[i].SetActive(false);
        }

        _childBody[0].SetActive(true);

        _childButton[0].SetActive(true);
        _childButton[3].SetActive(true);
        _childButton[4].SetActive(true);

        mainPanel.SetActive(false);

        for(int i = 0; i < _level.Length; i++)
        {
            _level[i].SetActive(false);
        }
        _level[0].SetActive(true);
        car.transform.position = _level[0].transform.position;
        car.transform.rotation = _level[0].transform.rotation;

        car.GetComponent<Rigidbody>().isKinematic = true;
        car.GetComponent<Rigidbody>().isKinematic = false;
        car.GetComponent<GearBox>().gear = 1;

    }

    public void ButtonCar()
    {
        _windowsNow = 1;
        
        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            _childBody[i].SetActive(false);
        }
        _childBody[3].SetActive(true);
        _childButton[0].SetActive(false);
        _childButton[1].SetActive(false);
        _childButton[2].SetActive(true);
        _childButton[3].SetActive(false);
        _childButton[4].SetActive(false);
    }

    public void ButtonLevel()
    {
        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            _childBody[i].SetActive(false);
            _childButton[i].SetActive(true);
        }
        _windowsNow = 2;
        _childBody[2].SetActive(true);

        _childButton[0].SetActive(false);
    }

    public void ButtonStart()
    {
        _windowsNow = 0;

        time.seconds = 0;
        time.minutes = 0;
        time.timerStarted = false;

        for(int i = 1; i < _level.Length; i++)
        {
            _level[i].transform.GetChild(0).GetComponent<EndZoneScript>()._entered = 0;
        }

        for (int i = 0; i < bodyPanel.transform.childCount; i++)
        {
            _childBody[i].SetActive(false);
            _childButton[i].SetActive(false);
        }

        _childBody[0].SetActive(true);

        _childButton[0].SetActive(true);
        _childButton[3].SetActive(true);

        mainPanel.SetActive(false);

        car.GetComponent<Rigidbody>().isKinematic = true;
        car.GetComponent<Rigidbody>().isKinematic = false;
        car.GetComponent<GearBox>().gear = 1;
    }

    

    

}
