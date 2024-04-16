using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggerScript : MonoBehaviour
{
    public Timer time;
    public float penaltyTime = 0f;

    private float _timer;
    private int _entered = 0;
    private bool _timerStart = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerStart)
        {
            _timer += Time.deltaTime;
        }

        if (_timer > 10)
        {
            _entered = 0;
            _timer = 0;
            _timerStart = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _timerStart = true;
        _entered++;
        if (_entered == 1)
        {
            time.seconds += penaltyTime;
        }
    }
}
