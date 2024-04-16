using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public enum iType
    {
        SteeringWheel, KeyBoard
    }
    public iType inputType;

    public static UnityEvent<InputSystem> sendInputSystem = new UnityEvent<InputSystem>();
    
    private InputActionMap _currentActionMap;

    private float _totalI;

    private float _steer;
    private float _gas;
    private float _break;
    private float _gear;
    private float _mouseRightClick;
    private float _mouseX;
    private float _mouseY;
    
    private float _totalGas;
    private float _totalSteer;
    private float _totalBreak;
    private float _totalGear = 0;
    private float _totalMouseRightClick;
    private float _totalMouseX;
    private float _totalMouseY;

    

    private void Awake()
    {
        inputActionAsset = Resources.Load<InputActionAsset>("PlayerInputActions");
        switch (inputType)
        {
            case iType.KeyBoard:
                _currentActionMap = inputActionAsset.FindActionMap("Keyboard"); ;
                break;
            case iType.SteeringWheel:
                _currentActionMap = inputActionAsset.FindActionMap("SteeringWheel");
                break;
        }
    }

    private void OnEnable()
    {
        sendInputSystem.Invoke(this);
        _currentActionMap.Enable();
    }

    private void OnDisable()
    {
        _currentActionMap.Disable();
    }

    private void FixedUpdate()
    {

        _steer = _currentActionMap["Steering"].ReadValue<float>();
        _gas = _currentActionMap["Gas"].ReadValue<float>();
        _break = _currentActionMap["Break"].ReadValue<float>();
        _gear = _currentActionMap["GearBox"].ReadValue<float>();
        _mouseRightClick = _currentActionMap["MouseRightClick"].ReadValue<float>();
        _mouseX = _currentActionMap["MouseX"].ReadValue<float>();
        _mouseY = _currentActionMap["MouseY"].ReadValue<float>();

        _totalI = _currentActionMap["KeyI"].ReadValue<float>();

        if (_currentActionMap == inputActionAsset.FindActionMap("SteeringWheel"))
        {
            _totalSteer = CalcSteer(_steer);
            _totalGas = -(_gas - 1f) / 2f;
            _totalBreak = -(_break + 1f) / 2f;
        }
        else if (_currentActionMap == inputActionAsset.FindActionMap("Keyboard"))
        {
            _totalSteer = _steer;
            _totalGas = _gas;
            _totalBreak = _break;
        }
        _totalGear = _gear;
        _totalMouseRightClick = _mouseRightClick;
        _totalMouseX = _mouseX;
        _totalMouseY = _mouseY;
    }

    private float CalcSteer(float steeringValue)
    {
        float _steerCalc;
        _steerCalc = Mathf.InverseLerp(-1f, 1f, steeringValue);
        if (steeringValue < 0)
        {
            _steerCalc = Mathf.Lerp(0f, 1f, _steerCalc) * 2;
        }
        else if(steeringValue > 0)
        {
            _steerCalc = -Mathf.Lerp(0f, 1f, 1f - _steerCalc) * 2;
        }
        else if (steeringValue == 0)
        {
            _steerCalc = _totalSteer;
        }
        return _steerCalc;
    }

    public float TotalSteer
    {
        get { return _totalSteer; }
    }

    public float TotalGas
    {
        get { return _totalGas; }
    }

    public float TotalBreak
    {
        get { return _totalBreak; }
    }
    public float TotalGear
    {
        get { return _totalGear; }
    }

    public float RightMouseClick
    {
        get { return _totalMouseRightClick; }
    }

    public float MouseX
    {
        get { return _totalMouseX; }
    }

    public float MouseY
    {
        get { return _totalMouseY; }
    }

    public float KeyI
    {
        get { return _totalI; }
    }


}
