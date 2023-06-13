using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] audios;
    private AudioSource audioOnPlay;
    private Scene scene;
    private void Awake() 
    {
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
    private void Start() 
    {
        
    }

    private void Update() 
    {
        scene = SceneManager.GetActiveScene ();
        // Debug.Log(scene.buildIndex);
        switch(scene.buildIndex)
        {
            case 1: // 城市
                audioOnPlay.clip = audios [2];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 2 : // 森林
                audioOnPlay.clip = audios [0];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 8: // 森林
                audioOnPlay.clip = audios [0];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
             case 10: // 下水道
                audioOnPlay.clip = audios [5];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 16: // 下水道
                audioOnPlay.clip = audios [5];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 18: // 教堂缓冲
                audioOnPlay.clip = audios [6];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 19: // 教堂
                audioOnPlay.clip = audios [7];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 25: // 教堂
                audioOnPlay.clip = audios [7];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 9: // 狼boss
                audioOnPlay.clip = audios [1];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 17: // 兔子boss
                audioOnPlay.clip = audios [3];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            case 26: // 爱丽丝
                audioOnPlay.clip = audios [4];
                if(!audioOnPlay.isPlaying)
                    audioOnPlay.Play();
                break;
            default :
                break;
        }
    }
}
