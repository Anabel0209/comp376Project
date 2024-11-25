using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainPlanetAudio;
    public AudioSource mainPlanetFinalAudio;
    public AudioSource planet1Audio;
    public AudioSource planet2Audio;

    private cameraController camController;

    private bool isFinalAudioActive = false; // To track whether the final audio is active

    private void Start()
    {
        // Assuming cameraController script is on the same GameObject as the main camera
        camController = Camera.main.GetComponent<cameraController>();

        // Start with initial planet audio
        PlayAudioForPlanet();
    }

    private void Update()
    {
        if (!isFinalAudioActive) // Only handle regular switching if final audio isn't active
        {
            PlayAudioForPlanet();
        }
        else if (camController.planetnb == 0) // Ensure final audio continues playing on the main planet
        {
            if (!mainPlanetFinalAudio.isPlaying)
            {
                StopAllPlanetAudio();
                mainPlanetFinalAudio.Play();
                Debug.Log("Ensuring final audio remains active on the main planet.");
            }
        }
    }

    private void PlayAudioForPlanet()
    {
        if (camController.planetnb == 0)
        {
            if (!mainPlanetAudio.isPlaying)
            {
                StopAllPlanetAudio();
                mainPlanetAudio.Play();
            }
        }
        else if (camController.planetnb == 1)
        {
            if (!planet1Audio.isPlaying)
            {
                StopAllPlanetAudio();
                planet1Audio.Play();
            }
        }
        else if (camController.planetnb == 2)
        {
            if (!planet2Audio.isPlaying)
            {
                StopAllPlanetAudio();
                planet2Audio.Play();
            }
        }
    }

    public void ActivateFinalAudio()
    {
        isFinalAudioActive = true; // Mark final audio as active

        if (camController.planetnb == 0) // Only activate final audio if on the main planet
        {
            StopAllPlanetAudio();
            mainPlanetFinalAudio.Play();
            Debug.Log("Final main planet audio activated.");
        }
    }

    public void DeactivateFinalAudio()
    {
        isFinalAudioActive = false; // Reset the final audio flag
        StopAllPlanetAudio();
        PlayAudioForPlanet(); // Resume normal audio logic
        Debug.Log("Final audio deactivated. Resuming normal audio logic.");
    }

    private void StopAllPlanetAudio()
    {
        mainPlanetAudio.Stop();
        mainPlanetFinalAudio.Stop();
        planet1Audio.Stop();
        planet2Audio.Stop();
    }
}
