using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public Rigidbody rb;
    public SuspCar[] wheels; // импорт колес для передачи на них мощности
    public GearBox gear; // импорт кпп для получения значений коэф передачи
    public InputSystem inputSystem;
    public float interp; 

    public enum wheelDrive // переключение типа привода авто
    {
        AWD,               // работает в текущей реализации
        RWD,               // не работает в текущей реализации
        FWD                // не работает в текущей реализации
    }
    public wheelDrive WD;

    [Header("Engine Set")]

    public float torque; // крутящий момент двигателя EngineTorque
    public float rpm; // обороты двигателя EngineRPM
    public float TotalDriveAxisAngularvelocity; // сумма среднего числа оборотов колес
    public float ClutchAngularVelocity; // угловая скорость 

    public int min_rpm = 700; // обороты холостого хода idle_rpm
    public int max_rpm = 7000; // максимальные обороты двигателя max_rpm
    public float inertialEng = 0.3f; // инерциальный момент двигателя inertia
    public float back_torque = -100f; // момент сопротивлений в двигателе back_torque
    private float RPMtoRad; // Обороты в радианы RPM_to_RadPS
    private float RadtoRPM; // Радианы в обороты RadPS_to_RPM

    private float graphTorq; // График развития мощности двигателем torque_curve

    private float angAccel; // ускорение

    public float EngAngVel = 0f; // угловая скорость двигателя EngineAngularVelocity

    [Header("Wheel")]
    public float[] driveTorque; // крутящий момент колеса DriveTorque
    public float wheelInert; // инерция колеса WheelINI
    public float[] wheelAnggVel; // угловая скорость колеса WheelAngularVelocity

    public float maxWheelSpeed; // максимальная скорость колеса с текущими параметрами

    public float[] brakeTorque;
    public float brakeStrength;
    public float[] brakeRatio;

    [Range(0f, 1f)] public float brakeCoef; // смещение мощности относительно осей TorqueRatio

    [Range(0f, 1f)] public float diffCoefF_toR; // смещение мощности относительно осей TorqueRatio
    private float diffCF; //коэф на переднюю ось (в видосе такого нет)
    private float diffCR; //коэф на заднюю ось (в видосе такого нет)

    // inputs

    public float xInp_Up; // вход горизонтали +
    public float xInp_Down; // вход горизонтали -
    private float summ;

    private void OnEnable()
    {
        InputSystem.sendInputSystem.AddListener(GetInputSystem);
    }

    //public AudioSource CarSound; // звуковой движок
    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        RPMtoRad = (2 * Mathf.PI) / 60; // обороты в радианы
        RadtoRPM = 1 / RPMtoRad; // радианы в обороты

        brakeRatio[0] = 1 - brakeCoef;
        brakeRatio[1] = 0 + brakeCoef;

        diffCF = 1 - diffCoefF_toR; // рассчет коэф для передней оси
        diffCR = 0 + diffCoefF_toR; // рассчет коэф для задней оси
        //CarSound = GetComponent<AudioSource>(); // получение компонента звук
        wheelInert = wheels[0].wheelMass * Mathf.Pow(wheels[0].wheelRadius, 2) * 0.5f; // расчет момента инерции колеса
    }
    public void FixedUpdate()
    {
        //-----------------------------
        //CarSound.pitch = Mathf.Clamp(rpm / 6000, 0.1f, 2.2f); // изменение уровня звука по оборотам

        if (gear.gear == 1)
        {
            xInp_Up = 0f;
        }
        else
        {
            xInp_Up = Mathf.Clamp(inputSystem.TotalGas, 0f, 1f);
        }
        graphTorq = -((Mathf.Pow(rpm - 9000, 2) / 324000000000000) - 250); // функция графика мощности

        torque = Mathf.Lerp(back_torque, graphTorq * xInp_Up, xInp_Up); // крутящий момент

        angAccel = torque / inertialEng; // ускорение двигателя

        //-----------------------------
        // распределение крутящего момента на колеса
        driveTorque[0] = Mathf.Clamp(torque, 0f, 1000f) * gear.totalGearRatio * diffCF * 0.5f; // мощность на переднем колесе
        driveTorque[1] = Mathf.Clamp(torque, 0f, 1000f) * gear.totalGearRatio * diffCF * 0.5f; // мощность на переднем колесе
        driveTorque[2] = Mathf.Clamp(torque, 0f, 1000f) * gear.totalGearRatio * diffCR * 0.5f; // мощность на заднем колесе
        driveTorque[3] = Mathf.Clamp(torque, 0f, 1000f) * gear.totalGearRatio * diffCR * 0.5f; // мощность на заднем колесе

        // рассчет углового ускорения колеса

        if (gear.gear != 1)
        {
            maxWheelSpeed = EngAngVel / gear.totalGearRatio; // максимальная скорость колеса на передаче исключая нейтальную  
        }
        else
        {
            maxWheelSpeed = float.PositiveInfinity; // если нейтральная то бесконечная макс скорость колеса
        }

        for (int i = 0; i <= wheelAnggVel.Length - 1; i++)
        {
            wheelAnggVel[i] = Mathf.Min(Mathf.Abs(wheelAnggVel[i] + (driveTorque[i] / wheelInert) * Time.fixedDeltaTime), Mathf.Abs(maxWheelSpeed)) * Mathf.Sign(maxWheelSpeed);
        }

        if (gear.gear == 0)
        {
            TotalDriveAxisAngularvelocity = (wheelAnggVel[0] + wheelAnggVel[1] + wheelAnggVel[2] + wheelAnggVel[3]) * 0.25f;
        }
        else
        {
            TotalDriveAxisAngularvelocity = (wheels[0].wheelSpeed + wheels[1].wheelSpeed + wheels[2].wheelSpeed + wheels[3].wheelSpeed) * 0.25f * Mathf.Sign(wheels[0].wheelSpeed + wheels[1].wheelSpeed + wheels[2].wheelSpeed + wheels[3].wheelSpeed);
        }

        ClutchAngularVelocity = TotalDriveAxisAngularvelocity * gear.totalGearRatio;

        if (gear.gear != 1)
        {
            EngAngVel = Mathf.Clamp(((ClutchAngularVelocity - EngAngVel) * interp) * gear.totalGearRatio * Mathf.Sign(gear.totalGearRatio) * Time.fixedDeltaTime + EngAngVel + angAccel * Time.fixedDeltaTime, RPMtoRad * min_rpm, RPMtoRad * max_rpm);
        }
        else
        {
            EngAngVel = Mathf.Clamp(((ClutchAngularVelocity - EngAngVel) * interp) * Time.fixedDeltaTime + EngAngVel + angAccel * Time.fixedDeltaTime, RPMtoRad * min_rpm, RPMtoRad * max_rpm);
        }

        interp = Mathf.Lerp(0.07f, 3f, Mathf.Abs(Vector3.Dot(rb.velocity, Vector3.forward)));

        foreach (SuspCar s in wheels){
            if (s.wheelFrontLeft)
            {
                s.wheelRot = wheelAnggVel[0];
                s.WheelTorque = driveTorque[0];
            }
            if (s.wheelFrontRight)
            {
                s.wheelRot = wheelAnggVel[1];
                s.WheelTorque = driveTorque[1];
            }
            if (s.wheelRearLeft)
            {
                s.wheelRot = wheelAnggVel[2];
                s.WheelTorque = driveTorque[2];
            }
            if (s.wheelRearRight)
            {
                s.wheelRot = wheelAnggVel[3];
                s.WheelTorque = driveTorque[3];
            }
        }
        rpm = EngAngVel * RadtoRPM; // фактические обороты двигателя  
    }

    private void GetInputSystem(InputSystem input)
    {
        inputSystem = input;
        InputSystem.sendInputSystem.RemoveListener(GetInputSystem);
    }

}