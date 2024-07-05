using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton instance of the AudioManager
    public static AudioManager instance;

    // Array of audio clips to be played in different scenes
    public AudioClip[] audios;

    // AudioSource component to play the audio
    private AudioSource audioOnPlay;

    // Current scene
    private Scene scene;

    // Called when the script instance is being loaded
    private void Awake() 
    {
        // Implement singleton pattern
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        DontDestroyOnLoad(gameObject);
        audioOnPlay = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private void Start() 
    {
        // Initialization logic if needed
    }

    // Called once per frame
    private void Update() 
    {
        scene = SceneManager.GetActiveScene();
        // Debug.Log(scene.buildIndex);
        switch(scene.buildIndex)
        {
            case 1: // City
                audioOnPlay.clip = audios[2];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 2 : // Forest
                audioOnPlay.clip = audios[0];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 8: // Forest
                audioOnPlay.clip = audios[0];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 10: // Sewer
                audioOnPlay.clip = audios[5];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 16: // Sewer
                audioOnPlay.clip = audios[5];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 18: // Church Buffer
                audioOnPlay.clip = audios[6];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 19: // Church
                audioOnPlay.clip = audios[7];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 25: // Church
                audioOnPlay.clip = audios[7];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 9: // Wolf Boss
                audioOnPlay.clip = audios[1];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 17: // Rabbit Boss
                audioOnPlay.clip = audios[3];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 26: // Alice
                audioOnPlay.clip = audios[4];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            default:
                break;
        }
    }
}

