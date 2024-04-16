using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFindChild : MonoBehaviour
{
    public GameObject[] _child; 

    void Start()
    {
        _child = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _child[i] = transform.GetChild(i).gameObject;
        }
    }

    public void windowClose()
    {
        foreach(GameObject child in _child)
        {
            child.SetActive(false);
        }
        Time.timeScale = 1;
    }
}
