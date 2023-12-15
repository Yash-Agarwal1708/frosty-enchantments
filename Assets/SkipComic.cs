using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipComic : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
        {
            FindObjectOfType<LevelChanger>().FadeToLevel(2);
        }
    }
}
