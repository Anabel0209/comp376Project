using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainPlanetAudio;
    public AudioSource mainPlanetFinalAudio;
    public AudioSource planet1Audio;
    public AudioSource planet2Audio;

    private cameraController camController;

    private bool isFinalAudioActive = false; 

    private void Start()
    {
        
        camController = Camera.main.GetComponent<cameraController>();

       
        PlayAudioForPlanet();
    }

    private void Update()
    {
        if (!isFinalAudioActive) 
        {
            PlayAudioForPlanet();
        }
        else if (camController.planetnb == 0) 
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
        isFinalAudioActive = true; 

        if (camController.planetnb == 0) 
        {
            StopAllPlanetAudio();
            mainPlanetFinalAudio.Play();
            Debug.Log("Final main planet audio activated.");
        }
    }

    public void DeactivateFinalAudio()
    {
        isFinalAudioActive = false; 
        StopAllPlanetAudio();
        PlayAudioForPlanet(); 
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
