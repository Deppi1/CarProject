using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArtemSpawner : MonoBehaviour
{
    public GameObject Artem;
    public float SpaceChair = 0.665f;
    public float SpaceFromChair = 0.73f;

    public float SpaceFromChairY = 0.6f;
    public float SpaceFromChairz = 1.3f;

    void Start()
    {
        float[] arrayList = new float[] {0.0f, -8.29100037f, -22.2210007f, -30.5219994f, -44.5460014f,
        -52.8199997f, -64.3519974f, -72.6470032f, -85.2450027f, -93.5309982f };

        foreach(float value in arrayList)
        {
            Vector3 point = new Vector3(value, 0f, 0f);
            Spawner(transform.TransformPoint(point).x);
        }
    }

    private void Spawner(float stepx)
    {
        
        float stepY = transform.position.y;
        float stepZ = transform.position.z;

        for(int i = 0; i <  12; i++)
        {
            float stepX = stepx;

            for (int l = 0; l < 2; l++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vector3 point = new Vector3(stepX, stepY, stepZ);
                    Instantiate(Artem, point, Artem.transform.rotation);
                    stepX -= SpaceChair;
                }
                stepX -= SpaceFromChair;
            }
            stepY -= SpaceFromChairY;
            stepZ += SpaceFromChairz;
        }  
    }
}
