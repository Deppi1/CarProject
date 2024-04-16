using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearBox : MonoBehaviour
{

    public Engine engine;               // ВЫЗОВ КЛАССА ENGINE
    public InputSystem inputSystem;
    public float[] gearRatio;           // МАССИВ ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ
    public float totalGearRatio;        // КОЭФИЦИЕНТ ТЕКУЩЕЙ И ГЛАНОЙ ПЕРЕДАЧИ
    public float maingGear = 3.18f;     // КОЭФИЦИЕНТ ГЛАВНОЙ ПЕРЕДАЧИ
    public float effic = 0.8f;          // КПД КОРОБКИ ПЕРЕДАЧ
    public float gearChTime;            // ВРЕМЯ ПЕРЕКЛЮЧЕНИЯ ПЕРЕДАЧИ
    
    public enum tType                   // ВЫБОР ТРАНСМИССИИ
    {
        Manual, Automatic
    }
    public tType transmissionType;
    public int gear = 1;                // ПЕРЕДАЧА
    public Text gearNum;                // ВЫВОД В HUD

    public float[] maxSpeedOnGear;     // МАКСИМАЛЬНАЯ СКОРОСТЬ НА КАЖДОЙ ПЕРЕДАЧЕ
    public float speed;                // СКОРОСТЬ ОБЪЕКТА 

    private int gearTr;                 // ПЕРЕПРИСВАИВАНИЕ В КОРУТИНАХ
    private int i = 2;                  // ПЕРЕДАЧА ДЛЯ АВТОМАТИЧЕСКОЙ КОРОБКИ 1 - R, 2 - N, 3 - D
    private int ggPlus = 0;             // ТРИГГЕР ДЛЯ ПОВЫШЕНИЯ АВТОМАТИЧЕСКОЙ КОРОБКИ
    private int gPlus = 0;              // ТРИГГЕР ДЛЯ ПОВЫШЕНИЯ РУЧНОЙ КОРОБКИ
    private int gMinus = 0;             // ТРИГГЕР ДЛЯ ПОНИЖЕНИЯ РУЧНОЙ КОРОБКИ
    public int gearNow = 0;            // ПЕРЕДАЧА ПРИ ПЕРЕКЛЮЧЕНИИ
    private Rigidbody rb;


    private int tmpTime = 2;
    private bool tmpbool = true;

    private void OnEnable()
    {
        InputSystem.sendInputSystem.AddListener(GetInputSystem);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeedOnGear = new float[gearRatio.Length - 3];   // СОЗДАНИЕ МАССИВА ДЛИННОЙ РАЗМЕРА МАССИВА ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ - 3
    }

    void FixedUpdate()
    {
        speed = rb.velocity.magnitude * 3.03f;           // 3.03 ПОДОБРАНО РУКАМИ ДЛЯ СРАВНЕНИЯ СО СКОРОСТЬЮ, РАСЧИТАНОЙ ПО ФОРМУЛЕ В КЛАССЕ ENGINE
        totalGearRatio = gearRatio[gear] * maingGear;    // РАСЧЁТ КОЭФИЦИЕНТА ТЕКУЩЕЙ И ГЛАНОЙ ПЕРЕДАЧИ

        switch(transmissionType)
        {
            // РУЧНАЯ КОРОБКА ПЕРЕДАЧ
            
            case tType.Manual:
                if ((gear < gearRatio.Length - 1) && (inputSystem.TotalGear == 1) && (gPlus == 0))   // ЕСЛИ ПЕРЕДАЧА < ДЛИННЫ МАССИВА ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ И НАЖАТА КЛАВИША "E" И ТРИГГЕР ПОВЫШЕНИЯ = 0
                {
                    StartCoroutine(GearUp());                                                   // ЗАПУСК КОРУТИНЫ ДЛЯ ПОВЫШЕНИЯ ПЕРЕДАЧИ
                    gPlus = 1;                                                                  // ТРИГГЕР ПОВЫШЕНИЯ = 1
                } else if (inputSystem.TotalGear == 0)                                            // ИНАЧЕ ЕСЛИ НЕ НАЖАТА КЛАВИША "E"
                {
                    gPlus = 0;                                                                  // ТРИГГЕР ПОВЫШЕНИЯ = 0
                }

                if ((gear > 0) && (inputSystem.TotalGear == -1) && (gMinus == 0))                     // ЕСЛИ ПЕРЕДАЧА  > 0 И НАЖАТА КЛАВИША "Q" И ТРИГГЕР ПОНИЖЕНИЯ = 0
                {
                    StartCoroutine(GearDown());                                                 // ЗАПУСК КОРУТИНЫ ДЛЯ ПОНИЖЕНИЯ ПЕРЕДАЧИ
                    gMinus = 1;                                                                 // ТРИГГЕР ПОНИЖЕНИЯ = 1
                } else if (inputSystem.TotalGear == 0)                                            // ИНАЧЕ ЕСЛИ НЕ НАЖАТА КЛАВИША "Q"
                {
                    gMinus = 0;                                                                 // ТРИГГЕР ПОНИЖЕНИЯ = 0
                }
            break;
            
            // АВТОМАТИЧЕСКАЯ КОРОБКА ПЕРЕДАЧ
            case tType.Automatic:
                if((inputSystem.TotalGear == 1) && (gPlus == 0) && (i < 2)) {                        // ЕСЛИ ПЕРЕДАЧА < ДЛИННЫ МАССИВА ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ И НАЖАТА КЛАВИША "E" И ТРИГГЕР ПОВЫШЕНИЯ = 0 И ПЕРЕДАЧА < 2 (R ИЛИ N)
                    i++;                                                                        // ПЕРЕДАЧА ПОВЫШАЕТСЯ ДО N ИЛИ D
                    gPlus = 1;                                                                  // ТРИГГЕР ПОВЫШЕНИЯ = 1
                } else if (inputSystem.TotalGear == 0) {                                          // ИНАЧЕ ЕСЛИ НЕ НАЖАТА КЛАВИША "E"
                    gPlus = 0;                                                                  // ТРИГГЕР ПОВЫШЕНИЯ = 0
                }
                if ((inputSystem.TotalGear == -1) && (gMinus == 0) && (i > 0)){                       // ЕСЛИ НАЖАТА КЛАВИША "Q" И ТРИГГЕР ПОНИЖЕНИЯ = 0 И ПЕРЕДАЧА > 0 (N ИЛИ D)
                    i--;                                                                        // ПЕРЕДАЧА ПОНИЖАЕТСЯ ДО R ИЛИ N
                    gMinus = 1;                                                                 // ТРИГГЕР ПОНИЖЕНИЯ = 1
                } else if (inputSystem.TotalGear == 0)
                {                                           // ИНАЧЕ ЕСЛИ НЕ НАЖАТА КЛАВИША "Q"
                    gMinus = 0;                                                                 // ТРИГГЕР ПОНИЖЕНИЯ = 0
                }
                if(i == 1) {            // НЕЙТРАЛЬ
                    gear = 1;
                } else if (i == 0) {    // РЕВЕРС
                    gear = 0;
                } else if (i == 2) {    // ДРАЙВ
                    if(inputSystem.TotalGear == 1) {
                        gear = 2;
                    }
                    // ПОВЫШЕНИЕ ПЕРЕДАЧИ
                    
                    if((engine.rpm > engine.max_rpm-1000f) && (tmpbool == true))   // ЕСЛИ СКОРОСТЬ > РАСЧЁТНОЙ СКОРОСТИ И УСЛОВИЕ = 0 И ПЕРЕДАЧА < ДЛИННЫ МАССИВА ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ И НАЖАТА КЛАВИША "W"
                    {
                        StartCoroutine(GearUp());
                        StartCoroutine(Timer());
                    } else if ((engine.rpm < engine.min_rpm + 4000) && (gear > 2) && (tmpbool == true)) {
                        StartCoroutine(GearDown());
                        StartCoroutine(Timer());
                    }

                    // if((speed > engine.maxWheelSpeed) && (ggPlus == 0) && (gear < gearRatio.Length - 1) && (Input.GetKey(KeyCode.W)))   // ЕСЛИ СКОРОСТЬ > РАСЧЁТНОЙ СКОРОСТИ И УСЛОВИЕ = 0 И ПЕРЕДАЧА < ДЛИННЫ МАССИВА ЗНАЧЕНИЙ КОЭФИЦИЕНТОВ ПЕРЕДАЧ И НАЖАТА КЛАВИША "W"
                    // {
                    //     if (gear > 2)
                    //     {
                    //         gearNow = gear - 2;
                    //     }
                    //     if (maxSpeedOnGear[gearNow] < speed)
                    //     {
                    //         maxSpeedOnGear[gearNow] = engine.maxWheelSpeed - 20;
                    //     }
                    //     StartCoroutine(GearUp());
                    //     ggPlus = 1;
                    // } else if ((ggPlus == 1) && (speed < engine.maxWheelSpeed) && (engine.maxWheelSpeed != double.PositiveInfinity))
                    // {
                    //     ggPlus = 0;                                                 // УСЛОВИЕ ДЛЯ НОРМАЛЬНОГО ПЕРЕКЛЮЧЕНИЯ
                    // }
                    
                    // // ПОНИЖЕНИЕ ПЕРЕДАЧИ
                    // if ((speed < maxSpeedOnGear[gearNow]) && (gear > 2))            // ЕСЛИ СКОРОСТЬ < ЗНАЧЕНИЯ ЭЛЕМЕНТА МАССИВА И ПЕРЕДАЧА > 2
                    // {
                    //     if (gearNow != 0)                                           // ЕСЛИ ПЕРЕДАЧА ПРИ ПЕРЕКЛЮЧЕНИИ НЕ 0
                    //     {
                    //         gearNow--;                                              // УМЕНЬШАЕМ ПЕРЕДАЧУ ПРИ ПЕРЕКЛЮЧЕНИИ
                    //     }
                    //     StartCoroutine(GearDown());                                 // ЗАПУСК КОРУТИНЫ ДЛЯ ПОНИЖЕНИЯ ПЕРЕДАЧИ
                    // }
                }
            break;
            
        }
        
        // ВЫВОД НА ЭКРАН
        if (gearNum != null) 
        {
            if(gear == 0) 
            {
                gearNum.text = "R";
            }
            else if (gear == 1)
            {
                gearNum.text = "N";
            }
            else if (gear >= 2)
            {
                gearNum.text = (gear-1).ToString();
            }
        }
    }

    private int time = 3;
    // КОРУТИН НА ПОВЫШЕНИЕ ПЕРЕДАЧИ
    private IEnumerator GearUp(){
        gear = gear + 1;                                        // ПОВЫШАЕМ ПЕРЕДАЧУ
        gearTr = gear;                                          // ВРЕМЕННАЯ ПЕРЕМЕННАЯ
        gear = 1;                                               // СТАВИМ ПЕРЕДАЧУ НА НЕЙТРАЛЬ
        yield return new WaitForSecondsRealtime(gearChTime);    // ВЫЗОВ ЗАДЕРЖКИ
        gear = gearTr;                                          // ПРИСВАИВАЕМ ОСНОВНОЙ ПЕРЕМЕННОЙ ЗНАЧЕНИЕ ВРЕМЕННОЙ
    }

    // КОРУТИН НА ПОНИЖЕНИЕ ПЕРЕДАЧИ
    private IEnumerator GearDown(){
        gear = gear - 1;                                        // ПОНИЖАЕМ ПЕРЕДАЧУ
        gearTr = gear;                                          // ВРЕМЕННАЯ ПЕРЕМЕННАЯ
        gear = 1;                                               // СТАВИМ ПЕРЕДАЧУ НА НЕЙТРАЛЬ
        yield return new WaitForSecondsRealtime(gearChTime);    // ВЫЗОВ ЗАДЕРЖКИ
        gear = gearTr;                                          // ПРИСВАИВАЕМ ОСНОВНОЙ ПЕРЕМЕННОЙ ЗНАЧЕНИЕ ВРЕМЕННОЙ
    }

    private IEnumerator Timer(){
        tmpbool = false;
        yield return new WaitForSecondsRealtime(tmpTime);
        tmpbool = true;
    }
    private void GetInputSystem(InputSystem input)
    {
        inputSystem = input;
        InputSystem.sendInputSystem.RemoveListener(GetInputSystem);
    }
}