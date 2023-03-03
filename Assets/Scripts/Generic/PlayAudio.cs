using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSfx _audio;
    void Start()
    {
        _audio.PlayAudioWithoutParenting();
    }
}
