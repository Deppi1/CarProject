using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Vector3 _start;
    public Vector3 _end;
    public float angle;
    public bool test = true;
    public float sec = 0;

    private void Start()
    {
        _start = new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        _end = new Vector3(transform.localRotation.eulerAngles.x + angle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
    void Update()
    {   
        sec += Time.deltaTime;
        
        if (sec < 0.5)
        {
            transform.Rotate(Vector3.right, Space.Self);
        }

        else if(0.5 < sec && sec < 1)
        {
            transform.Rotate(-Vector3.right, Space.Self);
        } else if (sec > 1)
        {
            sec = 0;
        }
        
        
    }
}
