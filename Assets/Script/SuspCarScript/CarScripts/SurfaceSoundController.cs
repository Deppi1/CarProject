using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���������� ������ �� �����������.
/// </summary>
public class SurfaceSoundController : MonoBehaviour
{
    /// <summary>
    /// ����������� �������� ����� ���� ����� ��������.
    /// </summary>
    private const float MIN_NEED_VELOCITY = 5f;

    /// <summary>
    /// ������� �� �������, �� �����������.
    /// </summary>
    public List<ItemDictionary<string, AudioClip>> SurfaceAudioClipDictionary;

    /// <summary>
    /// ������������� ������.
    /// </summary>
    public AudioSource AudioSource;

    /// <summary>
    /// ��������� �����������.
    /// </summary>
    private string _lastSurface;

    /// <summary>
    /// ������ ������.
    /// </summary>
    public Engine Engine;

    /// <summary>
    /// ��������� ���� �� �������� �����������.
    /// </summary>
    public void PlaySoundBySurface(string surface)
    {
        if (Engine.ClutchAngularVelocity < MIN_NEED_VELOCITY)
        {
            // ���� ������ ������������ � ���� ��� �������������, �� ��������� �������������.
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
            Debug.LogWarning($"��� ����� ����������� ���� �� ����� - {surface}.");
            return;
        }

        AudioSource.clip = surfacePair.Value;
        AudioSource.Play();
        */
    }
}
