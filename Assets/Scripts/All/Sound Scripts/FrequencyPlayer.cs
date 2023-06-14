
using UnityEngine;
using System.Collections.Generic;
public class FrequencyPlayer : MonoBehaviour
{
    public float frequency = 440.0f;
    public float duration = 1.0f;
    public string Note = "C4";

    private bool isPlaying = false;


    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 40), "PlayFrequency"))
        {
            PlayFrequency();
            Invoke(nameof(StopFrequency), duration);
        }

        if (GUI.Button(new Rect(500, 10, 300, 40), "Note"))
        {
            PlayNote(Note);
            Invoke(nameof(StopFrequency), duration);
        }
    }


    public Dictionary<string, float> noteFrequencies = new()
    {
        {"C4", 261.63f},
        {"D4", 293.66f},
        {"E4", 329.63f},
        {"F4", 349.23f},
        {"G4", 392.00f},
        {"A4", 440.00f},
        {"B4", 493.88f}
        // Add more notes and frequencies as needed
    };

    void Start()
    {

    }

    void PlayFrequency()
    {
        // Calculate the number of samples needed for the desired frequency
        int sampleRate = AudioSettings.outputSampleRate;
        int numSamples = Mathf.CeilToInt(sampleRate * duration);

        // Create an array to hold the audio data
        float[] data = new float[numSamples];

        // Generate the audio data for the desired frequency
        for (int i = 0; i < numSamples; i++)
        {
            float t = (float)i / sampleRate;
            data[i] = Mathf.Sin(2.0f * Mathf.PI * frequency * t);
        }

        // Create an AudioClip with the generated data
        AudioClip clip = AudioClip.Create("FrequencyClip", numSamples, 1, sampleRate, false);
        clip.SetData(data, 0);

        // Play the AudioClip on the AudioSource
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        isPlaying = true;
    }
    void PlayNote(string note)
    {
        // Check if the note exists in the dictionary
        if (noteFrequencies.ContainsKey(note))
        {
            // Get the frequency of the note
            float frequency = noteFrequencies[note];

            // Play the note using the AudioSource component
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.pitch = frequency / noteFrequencies["A4"]; // Set the pitch based on A4 (440 Hz) as reference
            audioSource.Play();
            isPlaying = true;
        }
        else
        {
            Debug.LogWarning("Note not found: " + note);
        }


    }
    void StopFrequency()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        isPlaying = false;
    }
}

