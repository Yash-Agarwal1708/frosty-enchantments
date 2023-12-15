using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class fakeGift : MonoBehaviour
{
    [SerializeField]
    public float speed = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0.0f, -(Time.deltaTime * speed), 0.0f, Space.Self);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("collidede");
    //    if (other.gameObject.tag == "FakeGift")
    //    {
    //        gameObject.transform.GetChild(6).gameObject.SetActive(true);
    //        SceneManager.LoadScene(6);


    //    }

    //}
    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("collidede");
    //    //if (other.gameObject.tag == "FakeGift")
    //    //{
    //    //    gameObject.transform.GetChild(6).gameObject.SetActive(true);
    //    //    SceneManager.LoadScene(6);


    //    //}

    //}
}
