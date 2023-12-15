using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFirstLevel : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<LevelChanger>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
