using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Engine;
using static GearBox;

[CreateAssetMenu(fileName = "New CarData", menuName = "Car Data", order = 51)]
public class CarData : ScriptableObject
{
    [SerializeField] private float _carMass = 1500.0f;
    [Header("Engine setup")]
    [SerializeField] private int _minRpm = 700;
    [SerializeField] private int _maxRpm = 10000;
    [SerializeField] private float _inertialEng = 0.3f;
    [SerializeField] private float _backTorque = -100f;
    [SerializeField][Range(0f, 1f)] private float _brakeCoef = 0.5f;
    [SerializeField][Range(0f, 1f)] private float _diffCoefF = 0.5f;

    private float[] _driveTorque = new float[] {1f, 1f, 1f, 1f};
    private wheelDrive _wd = wheelDrive.AWD;
    private float[] _brakeRatioEngine = new float[2];
    private float[] _brakeTorqueEngine = new float[2];
    private float[] _wheelAnggVel = new float[4];

    [Header("GearBox")]
    [SerializeField] private float[] _gearRatio = new float[]
    {
        -3.615f,
        0.0f,
        3.583f,
        2.038f,
        1.414f,
        1.108f,
        0.878f
    };
    [SerializeField] private float _mainGear = 3.18f;
    [SerializeField] private float _efficiency = 0.8f;
    [SerializeField] private float _timeChangeGear = 0.5f;
    [SerializeField] private tType _transmissionType = tType.Automatic;

    [Header("Car Angle Rotation")]
    [SerializeField] private float _wheelBase = 2.55f;
    [SerializeField] private float _rearTrack = 1.528f;
    [SerializeField] private float _turnRadius = 3.0f;

    [Header("Suspension Settings")]
    [SerializeField] private float _restLength = 0.43f;
    [SerializeField] private float _springTravel = 0.25f;
    [SerializeField] private float _springStiffness = 80000.0f;
    [SerializeField] private float _dampersStiffness = 7000.0f;
    [SerializeField] private float _steerTime = 15f;
    [SerializeField] private float _wheelRadius = 0.33f;
    [SerializeField] private float _wheelMass = 20.0f;
    [SerializeField] private AnimationCurve _forceCurve;
    [SerializeField] private float _brakeRatio = 0.5f;
    [SerializeField] private float _brakeStrength = 5000.0f;
    [SerializeField] private float _tireStiffnes = 1.0f;

    //Suspension
    public float RestLength
    {
        get
        {
            return _restLength;
        }
    }
    public float SpringTravel
    {
        get
        {
            return _springTravel;
        }
    }
    public float SpringStiffness
    {
        get
        {
            return _springStiffness;
        }
    }
    public float DampersStiffness
    {
        get
        {
            return _dampersStiffness;
        }
    }
    public float SteerTime
    {
        get
        {
            return _steerTime;
        }
    }
    public float WheelRadius
    {
        get
        {
            return _wheelRadius;
        }
    }
    public float WheelMass
    {
        get
        {
            return _wheelMass;
        }
    }
    public AnimationCurve ForceCurve
    {
        get
        {
            return _forceCurve;
        }
    }
    public float BrakeRatio
    {
        get
        {
            return _brakeRatio;
        }
    }
    public float BrakeStrength
    {
        get
        {
            return _brakeStrength;
        }
    }
    public float TireStiffnes
    {
        get
        {
            return _tireStiffnes;
        }
    }

    //CarAng
    public float WheelBase
    {
        get
        {
            return _wheelBase;
        }
    }
    public float RearTrack
    {
        get
        {
            return _rearTrack;
        }
    }
    public float TurnRadius
    {
        get
        {
            return _turnRadius;
        }
    }
    //Car
    public float CarMass
    {
        get
        {
            return _carMass;
        }
    }
    //GearBox
    public float[] GearRatio
    {
        get
        {
            return _gearRatio;
        }
    }
    public float MainGear
    {
        get
        {
            return _mainGear;
        }
    }
    public float Efficiency
    {
        get
        {
            return _efficiency;
        }
    }
    public float TimeChangeGear
    {
        get
        {
            return _timeChangeGear;
        }
    }
    public tType TransmissionType
    {
        get
        {
            return _transmissionType;
        }
    }

    //Engine
    public int MinRPM
    {
        get
        {
            return _minRpm;
        }
    }
    public int MaxRPM
    {
        get
        {
            return _maxRpm;
        }
    }
    public float InertialEng
    {
        get
        {
            return _inertialEng;
        }
    }
    public float BackTorque
    {
        get
        {
            return _backTorque;
        }
    }
    public float BrakeCoef
    {
        get
        {
            return _brakeCoef;
        }
    }
    public float DiffCoefF
    {
        get
        {
            return _diffCoefF;
        }
    }
    public float[] DriveTorque
    {
        get
        {
            return _driveTorque;
        }
    }
    public float[] BrakeRatioEngine
    {
        get
        {
            return _brakeRatioEngine;
        }
    }
    public float[] BrakeTorqueEngine
    {
        get
        {
            return _brakeTorqueEngine;
        }
    }
    public wheelDrive WheelDrive
    {
        get
        {
            return _wd;
        }
    }

    public float[] WheelAnggVel
    {
        get
        {
            return _wheelAnggVel;
        }
    }
}
