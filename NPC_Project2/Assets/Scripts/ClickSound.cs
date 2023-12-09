using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField]
    AudioClip clickSound;

    AudioSource src;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        src = GetComponent<AudioSource>();

        // Make sure an audio clip is assigned
        if (clickSound == null)
        {
            Debug.LogError("ClickSound script: Audio clip is not assigned!");
        }
    }

    void Update()
    {
        // Check for right mouse button click
        if (Input.GetMouseButtonDown(1))
        {
            // Play the click sound
            PlayClickSound();
        }
    }

    void PlayClickSound()
    {
        // Make sure the AudioSource component and audio clip are valid
        if (src != null && clickSound != null)
        {
            // Play the click sound
            src.Play();
        }
    }
}
