using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Контроллер звуков по поверхности.
/// </summary>
public class SurfaceSoundController : MonoBehaviour
{
    /// <summary>
    /// Минимальная скорость чтобы звук начал работать.
    /// </summary>
    private const float MIN_NEED_VELOCITY = 5f;

    /// <summary>
    /// Словарь со звуками, по поверхности.
    /// </summary>
    public List<ItemDictionary<string, AudioClip>> SurfaceAudioClipDictionary;

    /// <summary>
    /// Проигрыватель звуков.
    /// </summary>
    public AudioSource AudioSource;

    /// <summary>
    /// Последняя поверхность.
    /// </summary>
    private string _lastSurface;

    /// <summary>
    /// Движок машины.
    /// </summary>
    public Engine Engine;

    /// <summary>
    /// Проиграть звук по значению поверхности.
    /// </summary>
    public void PlaySoundBySurface(string surface)
    {
        if (Engine.ClutchAngularVelocity < MIN_NEED_VELOCITY)
        {
            // Если машина останавилась и звук еще проигрывается, то выключаем проигрыватель.
            if (AudioSource.isPlaying)
                AudioSource.Stop();
            return;
        }

        if (AudioSource.isPlaying && _lastSurface != null && _lastSurface == surface)
            return;

        _lastSurface = surface;
        /*
        var surfacePair = SurfaceAudioClipDictionary.FirstOrDefault(pair => pair.Key == surface);

        if (surfacePair is null)
        {
            Debug.LogWarning($"Для такой поверхности звук не задан - {surface}.");
            return;
        }

        AudioSource.clip = surfacePair.Value;
        AudioSource.Play();
        */
    }
}
