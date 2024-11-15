using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Source")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sFXSource;

    [Header("Audio Clip")] 
    public AudioClip backgroundMusic;

    public AudioClip winGame;

    public AudioClip buttonSound;

    public AudioClip hurtSound;

    public AudioClip looseSound;

    public AudioClip fallSound;

    public AudioClip shootSound;
    

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sFXSource.PlayOneShot(clip);
    }

    public void PlayButtonSFX()
    {
        sFXSource.PlayOneShot(buttonSound);
    }
}
