using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsExplanation : MonoBehaviour
{
    public Text explanationText;
    public float displayTime = 5f;

    void Start()
    {
        // Set the text to be invisible initially
        explanationText.gameObject.SetActive(true);

        // Display the text for a few seconds
        Invoke("ShowText", 1f); // Adjust the delay as needed
    }

    void ShowText()
    {
        // Set the text to be visible
        explanationText.gameObject.SetActive(true);

        // Hide the text after a few seconds
        Invoke("HideText", displayTime);
    }

    void HideText()
    {
        // Set the text to be invisible
        explanationText.gameObject.SetActive(false);
    }
}
