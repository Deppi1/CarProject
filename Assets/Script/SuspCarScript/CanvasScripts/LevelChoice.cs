using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChoice : MonoBehaviour
{
    public GameObject car;
    public GameObject levels;
    private GameObject[] level;

    private void Start()
    {
        level = new GameObject[levels.transform.childCount];

        for (int i = 0; i < level.Length; i++)
        {
            level[i] = levels.transform.GetChild(i).gameObject;
        }
    }

    public void FirstButton()
    {
        for(int i = 0; i < level.Length; i++)
        {
            level[i].SetActive(false);
        }

        level[1].SetActive(true);
        car.transform.position = level[1].transform.position;
        car.transform.rotation = level[1].transform.rotation;
    }

    public void SecondButton()
    {
        for (int i = 0; i < level.Length; i++)
        {
            level[i].SetActive(false);
        }

        level[2].SetActive(true);
        car.transform.position = level[2].transform.position;
        car.transform.rotation = level[2].transform.rotation;
    }

    public void ThirdButton()
    {
        for (int i = 0; i < level.Length; i++)
        {
            level[i].SetActive(false);
        }

        level[3].SetActive(true);
        car.transform.position = level[3].transform.position;
        car.transform.rotation = level[3].transform.rotation;
    }

    public void FourthButton()
    {
        for (int i = 0; i < level.Length; i++)
        {
            level[i].SetActive(false);
        }

        level[4].SetActive(true);
        car.transform.position = level[4].transform.position;
        car.transform.rotation = level[4].transform.rotation;
    }

    public void FifthButton()
    {
        for (int i = 0; i < level.Length; i++)
        {
            level[i].SetActive(false);
        }

        level[5].SetActive(true);
        car.transform.position = level[5].transform.position;
        car.transform.rotation = level[5].transform.rotation;
    }

    public void SixthButton()
    {

    }

}
