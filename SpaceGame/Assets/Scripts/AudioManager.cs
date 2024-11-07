using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainPlanetAudio;
    public AudioSource planet1Audio;
    private cameraController camController;

    private void Start()
    {
        // Assuming cameraController script is on the same GameObject as the main camera
        camController = Camera.main.GetComponent<cameraController>();

        // Start with main planet audio playing and planet 1 audio stopped
        if (camController.planetnb == 0)
        {
            mainPlanetAudio.Play();
            planet1Audio.Stop();
        }
        else
        {
            mainPlanetAudio.Stop();
            planet1Audio.Play();
        }
    }

    private void Update()
    {
        // Switch audio based on current planet
        if (camController.planetnb == 0 && !mainPlanetAudio.isPlaying)
        {
            mainPlanetAudio.Play();
            planet1Audio.Stop();
        }
        else if (camController.planetnb == 1 && !planet1Audio.isPlaying)
        {
            mainPlanetAudio.Stop();
            planet1Audio.Play();
        }
    }
}
