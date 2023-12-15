using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private int levelToLoad;
    public void FadeToLevel(int levelIndex)
    {
        animator.SetTrigger("fadeOut");
        levelToLoad = levelIndex;

    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
