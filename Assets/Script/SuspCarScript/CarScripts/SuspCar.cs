using UnityEngine;
using UnityEngine.UI;

public class SuspCar : MonoBehaviour
{
    // *************Загрузка файлов*****************
    public Engine motor; // файл с параметрами двигателя
    public InputSystem inputSystem;
    public SurfaceType surfaceType;
    public GameObject[] wheels;
    private Rigidbody rb; // объект на который прилагается сила подвески
    public Collider colCar;
 
    public GameObject wheel_prefab, parent_wheel_prefab; // заготовка колеса, где спавнить колесо
    private GameObject wheel; // объект для монипуляции положения колеса

    // *************Данные для расчета*****************

    // *************Указываем какое это колесо*****************
    [Header("Triger Wheel")]
    public bool wheelFrontLeft; // тригер колеса ПерЛев
    public bool wheelFrontRight; // тригер колеса ПерПрав
    public bool wheelRearLeft; // тригер колеса ЗадЛев
    public bool wheelRearRight; // тригер колеса ЗадПрав

    // *************Настройки подвески*****************

    [Header("Suspension")]
    // !!!!!! Можно менять !!!!!!
    public float restLength; // длинна пружины
    public float springTravel; // хад сжатия пружины
    public float springStiffness; // жесткость пружины
    public float dampersStiffness; // аммортизация

    // !!!!!! Нельзя менять !!!!!!
    private float minLenght; // минимальна длинна
    private float maxLenght; // максимальная длинна
    private float lastLenght; // начальная длина пружины в момент времени
    private float springLength; // длина пружины после действия сил в момент времни
    private float springVelocity; // скорость выпрямления пружины
    private float springForce;  // сила пружины на авто
    private float damperForce;  // сила дампера на авто
    private Vector3 suspencionForce; // комплексная сила подвески на авто

    [Header("Wheel Debug")]
    public float steerAngle; // угол поворота колеса (рассчитывается, не задаём)
    public float steerTime; // время на поворот колеса до расчитанного угла

    private float wheelAngle; // переменная для плавного поворота колес

    [Header("Wheel")]
    public float wheelRadius;  // радиус колеса
    public float wheelMass;  // масса колеса
    private float wheelSlip;  // сопротивление дороги
    private float slipVelocity; // сопротивление дороги от скорости
    public Vector3 wheelVelocityLS; // вектор силы на колесо
    public AnimationCurve ForceCurve; // кривая мощности двигателя

    public Text speedometer; // скорость на колесе для UI

    [Header("Rotation")]
    public float wheelSpeed;  // скорость колеса

    public float wheelRot; // вращение колеса
    public float xInput, yInput;  // данные с устройства

    // направления действующих сил на авто
    public float Fx; // силы по оси x
    private float Fy; // силы по оси y

    //Ещё не настроено....
    private float FrictionCoeff; // коэфициент трения
    public float WheelTorque; // мощность на колесе от двигателя

    // трения колеса (ещё в разработке)
    private float maxFrict; // предел трения резины
    private float longStifness = 1f;
    private float lateralSlipVel;
    private float lateralSlipNorm;
    private float longSlipVel;
    private float longSlipNormalized = 0f;
    public float tireStiffnes;

    private Vector3 CombinedSlip;
    private Vector3 tireForceNorm;
    private Vector3 tireForce;

    public int trRFWD = 0;

    // тормоза
    public float xInp_Down;

    public float brakeStrength;

    public float brakeTorque;

    public float brakeRatio = 0.5f;
    // -----тормоза-----

    public GameObject colliderWheel;
    private GameObject cwm;
    private Rigidbody rbcoll;

    private float stoneSize;

    void OnDrawGizmos(){ // визуализация сил и подвески
        Gizmos.color = Color.blue;
        
        if(Application.isPlaying){
            Gizmos.DrawWireSphere(transform.position - transform.up * (springLength), wheelRadius);
            Debug.DrawRay(transform.position, - transform.up * (springLength), Color.red);
            Debug.DrawRay(transform.position - transform.up * (springLength + wheelRadius), -transform.right * Fy / 800, Color.blue);
            Debug.DrawRay(transform.position - transform.up * (springLength + wheelRadius), transform.forward * Fx / 800, Color.blue);
            Debug.DrawRay(wheel.transform.position, transform.TransformDirection(wheelVelocityLS), Color.black);
        }
    }

    private void OnEnable()
    {
        InputSystem.sendInputSystem.AddListener(GetInputSystem);
    }

    void Start()
    {

        rb = transform.root.GetComponent<Rigidbody>(); // получение твердого тела

        cwm = Instantiate(colliderWheel, transform.position, transform.rotation);
        cwm.transform.localScale = new Vector3(wheelRadius,wheelRadius,wheelRadius);
        rbcoll = cwm.GetComponent<Rigidbody>();
        cwm.transform.SetParent(parent_wheel_prefab.transform);

        minLenght = restLength - springTravel; // минимальная длина пружины
        maxLenght = restLength + springTravel; // максимальная длина пружины

        wheel = Instantiate(wheel_prefab, transform.position, transform.rotation); // визуализация колес по модели

        if (wheelFrontRight || wheelRearRight) { //если колеса на правой стороне, то задаем отрицательный размер для верной визуализации
            wheel.transform.localScale = new Vector3(-wheelRadius,wheelRadius,wheelRadius);
        } else {
            wheel.transform.localScale = new Vector3(wheelRadius,wheelRadius,wheelRadius);
        }

        Physics.IgnoreCollision(colCar.GetComponent<Collider>(), rbcoll.GetComponent<Collider>());

        wheel.transform.SetParent(parent_wheel_prefab.transform); //устанавливаем точки крепления подвески родителем для модели колеса
    }

    void Update() {
        WheelRotate(); // поворачиваем колеса
    }

    void FixedUpdate()
    {
        tireStiffnes = surfaceType.friction;

        //если SweepTest касается земли, то запускаем код
        if (rbcoll.SweepTest(-transform.up, out RaycastHit hit, maxLenght + wheelRadius)){ // испускание коллайдера и проверка касаний
            
            xInp_Down = Mathf.Clamp(inputSystem.TotalBreak, -1f, 0f);
            brakeTorque = brakeStrength * xInp_Down * brakeRatio;

            lastLenght = springLength; // запоминание последнего положения подвески
            springLength = hit.distance; // устанавливаенм длинну пружины равной длины до точки контакта сферы с поверхностью
            springLength = Mathf.Clamp(springLength, minLenght, maxLenght); // плавное изменение длины подвески от сжатия и растяжения в пределах минимального и максимального знаяения работы подвески
            springVelocity = (lastLenght - springLength) / Time.fixedDeltaTime; // скорость сжатия или растяжения подвески
            springForce = springStiffness * (restLength - springLength); // сила пружины 
            damperForce = dampersStiffness * springVelocity; // сила демпфера
            suspencionForce = (springForce + damperForce) * transform.up + stoneSize * transform.up; // сумма действующих сил в подвеске направленная вверх
            

            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point)); // передача сил от подвески в точке соприкосновения колеса с поверхностью и преобразование из глобальных в локальные координаты
            stoneSize = (Mathf.Clamp((new Vector2(wheelVelocityLS.x, wheelVelocityLS.z).magnitude/50), 0, 1) * Random.Range(-8000f, 8000f)) * surfaceType.roughness; // Изменение подвески на шераховатости


            // тормоза
            wheelRot += (Mathf.Sign(wheelRot) * brakeTorque / motor.wheelInert)  * Time.fixedDeltaTime;
            // -----тормоза-----

            longSlipVel = ((wheelRot * wheelRadius) - wheelVelocityLS.z) * surfaceType.friction; // скольжение колеса по направлению вперед назад

            lateralSlipNorm = Mathf.Clamp(wheelVelocityLS.x * (tireStiffnes), -1f, 1f); // скорость поперечного скольжения


            if (wheelVelocityLS.z * longSlipVel > 0)
            {
                longSlipNormalized = Mathf.Clamp((WheelTorque / wheelRadius) / Mathf.Max(0.000000001f, springForce), -2f, 2f);
            } 
            else if (rb.velocity.magnitude < 0.3f && motor.gear.gear == 1)
            {
                longSlipNormalized = 0.0f;
                lateralSlipNorm = 0.0f;
                rb.velocity = Vector3.zero;
            }
            else
            {
               longSlipNormalized = Mathf.Clamp(longStifness * longSlipVel, -2f, 2f); 
            }

            CombinedSlip = new Vector3(longSlipNormalized, lateralSlipNorm, 0f); // направление действия сил трения покрышек

            tireForceNorm = ForceCurve.Evaluate(CombinedSlip.magnitude) * CombinedSlip.normalized; // сила двигателя передаваемая на колеса с учетом трения колес

            tireForce = tireForceNorm * Mathf.Max(0f, springForce) * surfaceType.friction; // как и выше, но с учетом силы, с которой прижимает пружина колесо к земле

            wheelRot += ((Mathf.Clamp(longSlipVel * (-10),-1f,1f) * Mathf.Max(springForce, 0) * wheelRadius) / motor.wheelInert) * Time.fixedDeltaTime; // вращение колеса

            Fx = tireForce.x; // боковая сила на колесо
            Fy = tireForce.y; // сила на колесо вверх
            
            rb.AddForceAtPosition(suspencionForce + (Fx * transform.forward) + (Fy * -transform.right), hit.point); // прередача всех сил на твердое тело
            wheel.transform.position = transform.position - transform.up * (springLength); // перемещение модели колеса 
        } 
        
        if (speedometer != null) // скорость колеса
        {
            speedometer.text = Mathf.Round(wheelVelocityLS.z * 3.6f).ToString();
        }

        WheelSpeed();
    }
    
    private void WheelRotate() { // поворот передних колес по аккерману
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);
    }
    
    void WheelSpeed() { // вращение модели колеса в зависимости от действующих на него сил
        wheelSpeed = -Mathf.Round(wheelVelocityLS.z)/wheelRadius;

        wheel.transform.Rotate(wheelSpeed * Mathf.Rad2Deg, 0f, 0f);
    }
    private void GetInputSystem(InputSystem input)
    {
        inputSystem = input;
        InputSystem.sendInputSystem.RemoveListener(GetInputSystem);
    }
}