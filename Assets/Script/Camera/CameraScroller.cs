//using Assets.Script.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт отвечает за скорлинг камеры вперед назад.
/// </summary>
public class CameraScroller : MonoBehaviour
{
    /// <summary>
    /// Скроллинг коэфициент смещения.
    /// </summary>
    private const float SCROLL_OFFSET_RATIO = 0.5f;

    /// <summary>
    /// Количество уровней детализации модели.
    /// </summary>
    private const float COUNT_LODS = 3f;

    /// <summary>
    /// Максимальное смещение камеры.
    /// </summary>
    private const float MAX_OFFSET_Z = 5;
    
    /// <summary>
    /// Минимальное смещение камеры.
    /// </summary>
    private const float MIN_OFFSET_Z = -5;

    /// <summary>
    /// Текущий LOD.
    /// </summary>
    private int _currentLod;

    // Трансформ машины.
    public Transform TargetCar;

    /// <summary>
    /// Родитель на котором весит меш.
    /// </summary>
    [Tooltip("Родитель на котором весит меш.")]
    public Transform MeshParentTransform;

    // Лоды для машины. Три лода: Low, Medium, High.
    // Заполнять в Unity в Inspector.
    public List<GameObject> CarLods;

    private void Awake()
    {
        _currentLod = GetLevelLodByZ();
        ChangeModel(_currentLod);
    }

    // Update is called once per frame
    private void Update()
    {
        
        //if (Input.mouseScrollDelta.y == 0)
        //    return;

        //var zPos = GetPositionZ();
        //transform.localPosition = new Vector3(0, 0, zPos);

        CheckCarModel();
    }

    /// <summary>
    /// Получить локальную позицию по Z для камеры.
    /// </summary>
    /// <returns>Z.</returns>
    /*
    private float GetPositionZ()
    {
        var zPos = transform.localPosition.z + Input.mouseScrollDelta.y * SCROLL_OFFSET_RATIO;
        var zPosLimited = Mathf.Clamp(zPos, MIN_OFFSET_Z, MAX_OFFSET_Z);

        return zPosLimited;
    }
    */
    /// <summary>
    /// Проверим нужно ли изменить модель машины.
    /// </summary>
    private void CheckCarModel()
    {
        int levelLod = GetLevelLodByZ();
        if (levelLod != _currentLod)
            ChangeModel(levelLod);
    }

    /// <summary>
    /// Получить уровень LOD по Z.
    /// </summary>
    /// <returns>Уровень </returns>
    private int GetLevelLodByZ()
    {
        // Участок для 1 LOD.
        var zPos = transform.localPosition.z;
        var currentZOffset = zPos < 0 ? Mathf.Abs(MIN_OFFSET_Z) - Mathf.Abs(zPos) : zPos + Mathf.Abs(MIN_OFFSET_Z);
        // Смещение в долях (процентах) (не умножено на 100)
        var offsetFillPercent = currentZOffset / (MAX_OFFSET_Z + Mathf.Abs(MIN_OFFSET_Z));
        
        var levelLod = Mathf.RoundToInt(offsetFillPercent * COUNT_LODS);
        if (levelLod == 0)
            levelLod = 1;
        return levelLod;
    }

    /// <summary>
    /// Изменить модель машины в соответствии с уровнем детализации.
    /// </summary>
    private void ChangeModel(int levelLod)
    {
        var indexLevelLod = levelLod - 1;

        // Выключить все что не должно работать в машине когда не работает.
        for (var index = 0; index < MeshParentTransform.childCount; index++)
        {
            var childGameObject = MeshParentTransform.GetChild(index);
            Destroy(childGameObject.gameObject);
        }

        // Создаем нужный объект.
        var modelLod = CarLods[indexLevelLod];
        var newModel = Instantiate(modelLod, MeshParentTransform);
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localEulerAngles = Vector3.zero;

        _currentLod = levelLod;
    }
}
