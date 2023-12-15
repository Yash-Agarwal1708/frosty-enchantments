using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    private AudioSource audioSource;

    private AudioClip rotateSound;
    private AudioClip winSound;

    public void PlayRotateSound()
    {
        if (rotateSound == null)
        {
            rotateSound = Resources.Load<AudioClip>("rotation_sound");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(rotateSound);

        }
        else
        {
            audioSource.PlayOneShot(rotateSound);
        }
    }
    
    public void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(Resources.Load<AudioClip>("up_swish"));

            }
            else
            {
                audioSource.PlayOneShot(Resources.Load<AudioClip>("up_swish"));
            }
        }
    }

    public void PlayWinSound()
    {
        if (winSound == null)
        {
            winSound = Resources.Load<AudioClip>("level_win");
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(winSound);

        }
        else
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    public void PlayButtonClick()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

        }
        else
        {
            audioSource.Play();
        }
    }
  
}
