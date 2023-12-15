using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField]
    private Animator settingsAnimator;

    [SerializeField]
    private LevelChanger levelChanger;

    [SerializeField] 
    GameObject leaderBoard;

    [SerializeField]
    PlayfabManager playfabManager;
    private void Start()
    {
        levelChanger = FindObjectOfType<LevelChanger>();
    }
    public void Retry()
    {
        levelChanger.FadeToLevel(0);
    }
    public void Play()
    {
        levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenLeaderBoard(){
        // GameObject leaderBoard = GameObject.FindGameObjectWithTag("LeaderBoard");
        if(leaderBoard){
            playfabManager.GetLeaderBoard();
            leaderBoard.SetActive(true);
        }
    }
    public void CloseLeaderBoard(){
        // GameObject leaderBoard = GameObject.FindGameObjectWithTag("LeaderBoard");
        if(leaderBoard){
            leaderBoard.SetActive(false);
        }
    }

    public void Restart()
    {
        levelChanger.FadeToLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void Settings()
    {
        if (settingsAnimator.GetBool("enter"))
        {
            settingsAnimator.SetBool("enter", false);
            settingsAnimator.SetBool("exit", true);
        }
        else
        {
            settingsAnimator.SetBool("enter", true);
            settingsAnimator.SetBool("exit", true);
        }
    }

    [SerializeField]
    private AudioMixer musicAudioMixer;
    [SerializeField]
    private AudioMixer sfxAudioMixer;

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("Volume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        sfxAudioMixer.SetFloat("Volume", volume);
    }

    public void SetGraphics(int graphics)
    {
        QualitySettings.SetQualityLevel(graphics);
    }

    public void MainMenu()
    {
        levelChanger.FadeToLevel(0);
        Time.timeScale = 1;
    }

}