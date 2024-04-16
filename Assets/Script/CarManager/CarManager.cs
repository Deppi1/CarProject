using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private CarData _carData;
    [SerializeField] private GameObject[] _frontWheels;
    [SerializeField] private GameObject[] _backWheels;
    [SerializeField] private InputSystem.iType _inputType;
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private GameObject _wheelPrefab;
    [SerializeField] private Collider _carCollider;
    [SerializeField] private GameObject _wheelCollider;
    [SerializeField] private InputSystem _inputSystem;

    private List<SuspCar> _wheelsSusp = new List<SuspCar>();
    private CarAng _carAng;
    private Engine _engine;
    private GearBox _gearBox;
    
    // Start is called before the first frame update
    void Awake()
    {
        SetRigidBody();
        SetWheelsComponents();
        SetEngineSettings();
        SetGearBox();
        SetCarAng();
        SetSurfaceType();


        SetDependencies();
        SetInputActions();
    }

    private void SetWheelsComponents()
    {
        
        for (int i = 0; i < _frontWheels.Length; i++)
        {
            SuspCar suspension = _frontWheels[i].AddComponent<SuspCar>();

            if(i % 2 == 0)
            {
                suspension.wheelFrontLeft = true;
            }
            else
            {
                suspension.wheelFrontRight = true;
            }

            _wheelsSusp.Add(suspension);
        }

        for (int i = 0; i < _backWheels.Length; i++)
        {
            SuspCar suspension = _backWheels[i].AddComponent<SuspCar>();

            if (i % 2 == 0)
            {
                suspension.wheelRearLeft = true;
            }
            else
            {
                suspension.wheelRearRight = true;
            }

            _wheelsSusp.Add(suspension);
        }

        for (int i = 0; i < _wheelsSusp.Count; i++)
        {
            _wheelsSusp[i].restLength = _carData.RestLength;
            _wheelsSusp[i].springTravel = _carData.SpringTravel;
            _wheelsSusp[i].springStiffness = _carData.SpringStiffness;
            _wheelsSusp[i].dampersStiffness = _carData.DampersStiffness;
            _wheelsSusp[i].steerTime = _carData.SteerTime;
            _wheelsSusp[i].wheelRadius = _carData.WheelRadius;
            _wheelsSusp[i].wheelMass = _carData.WheelMass;
            _wheelsSusp[i].ForceCurve = _carData.ForceCurve;
            _wheelsSusp[i].brakeRatio = _carData.BrakeRatio;
            _wheelsSusp[i].brakeStrength = _carData.BrakeStrength;
            _wheelsSusp[i].tireStiffnes = _carData.TireStiffnes;

        }
    }

    private void SetSurfaceType()
    {
        for (int i = 0; i < _wheelsSusp.Count; i++)
        {
            GameObject objectSoundController = Instantiate(new GameObject(name: "SurfaceSoundController"), _wheelsSusp[i].gameObject.transform);

            SurfaceSoundController soundController = objectSoundController.AddComponent<SurfaceSoundController>();
            soundController.AudioSource = objectSoundController.AddComponent<AudioSource>();
            soundController.Engine = _engine;

            SurfaceType surfaceType = _wheelsSusp[i].gameObject.AddComponent<SurfaceType>();
            surfaceType.suspCar = _wheelsSusp[i];
            surfaceType.SurfaceSoundController = soundController;
            _wheelsSusp[i].surfaceType = surfaceType;

            objectSoundController.SetActive(false);


        }
    }

    private void SetRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = _carData.CarMass;
    }

    private void SetInputActions()
    {
        /*InputSystem inputSystem = gameObject.AddComponent<InputSystem>();
        inputSystem.inputType = _inputType;*/

        _engine.inputSystem = _inputSystem;
        _carAng.inputSystem = _inputSystem;
        _gearBox.inputSystem = _inputSystem;

        for (int i = 0; i < _wheelsSusp.Count; i++)
        {
            _wheelsSusp[i].inputSystem = _inputSystem;

        }
    }

    private void SetEngineSettings()
    {
        _engine = gameObject.AddComponent<Engine>();
        _engine.min_rpm = _carData.MinRPM;
        _engine.max_rpm = _carData.MaxRPM;
        _engine.inertialEng = _carData.InertialEng;
        _engine.back_torque = _carData.BackTorque;
        _engine.brakeCoef = _carData.BrakeCoef;
        _engine.diffCoefF_toR = _carData.DiffCoefF;
        _engine.driveTorque = _carData.DriveTorque;
        _engine.WD = _carData.WheelDrive;
        _engine.brakeRatio = _carData.BrakeRatioEngine;
        _engine.brakeTorque = _carData.BrakeTorqueEngine;
        _engine.wheelAnggVel = _carData.WheelAnggVel;
    }

    private void SetGearBox()
    {
        _gearBox = gameObject.AddComponent<GearBox>();
        _gearBox.gearRatio = _carData.GearRatio;
        _gearBox.maingGear = _carData.MainGear;
        _gearBox.effic = _carData.Efficiency;
        _gearBox.gearChTime = _carData.TimeChangeGear;
        _gearBox.transmissionType = _carData.TransmissionType;
    }

    private void SetCarAng()
    {
        _carAng = gameObject.AddComponent<CarAng>();
        _carAng.wheelBase = _carData.WheelBase;
        _carAng.rearTrack = _carData.RearTrack;
        _carAng.turnRadius = _carData.TurnRadius;
        _carAng.centerOfMassTransform = _centerOfMass;
    }
    private void SetDependencies()
    {
        _engine.gear = _gearBox;
        _engine.wheels = _wheelsSusp.ToArray();

        _gearBox.engine = _engine;

        _carAng.wheels = _wheelsSusp.ToArray();

        for (int i = 0; i < _wheelsSusp.Count; i++)
        {
            _wheelsSusp[i].motor = _engine;
            _wheelsSusp[i].wheel_prefab = _wheelPrefab;
            _wheelsSusp[i].colCar = _carCollider;
            _wheelsSusp[i].colliderWheel = _wheelCollider;
            _wheelsSusp[i].parent_wheel_prefab = _wheelsSusp[i].gameObject;
        }
    }
}
