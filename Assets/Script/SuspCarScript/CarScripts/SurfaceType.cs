using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    public SuspCar suspCar;
    public string surface;                  // Тип маски
    public float friction = 1f;             // Коэф. трения
    //public float rollResistance = 0f;     // Коэф. трения-качения
    public float roughness = 1f;            // Неровность поверхности
    public float bias = 0f;                 // Проваливание под текстуру (для сыпучих поверхностей)

    /// <summary>
    /// Контроллер звуков по поверхности.
    /// </summary>
    [Header("Контроллер звуков по поверхности.")]
    public SurfaceSoundController SurfaceSoundController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);


        Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up), Color.yellow);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            surface = LayerMask.LayerToName(hit.transform.gameObject.layer);
            if (surface == "Asphalt")
            {
                friction = 0.9f;
                //rollResistance = 0.02;
                roughness = 0f;
                bias = 0f;
            }
            else if (surface == "WetAsphalt")
            {
                friction = 0.5f;
                //rollResistance = 0.02;
                roughness = 0f;
                bias = 0f;
            }
            else if (surface == "Gravel")
            {
                friction = 0.7f;
                //rollResistance = 0.03;
                roughness = 1.5f;
                bias = -3f;
            }
            else if (surface == "Ice")
            {
                friction = 0.1f;
                //rollResistance = 0.03;
                roughness = 0f;
                bias = 0f;
            }
            else
            {
                friction = 1f;
                //rollResistance = 0.03;
                roughness = 0f;
                bias = 0f;
            }

            SurfaceSoundController.PlaySoundBySurface(surface);
        }


    }
}
