using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour
{
    [SerializeField]
    AudioClip splatSound;

    AudioSource src;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        src = GetComponent<AudioSource>();

        // Make sure an audio clip is assigned
        if (splatSound == null)
        {
            Debug.LogError("ClickSound script: Audio clip is not assigned!");
        }
    }

    public void PlaySplatSound()
    {
        // Make sure the AudioSource component and audio clip are valid
        if (src != null && splatSound != null)
        {
            // Play the click sound
            src.Play();
        }
    }
}
