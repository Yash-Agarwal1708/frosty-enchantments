using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gift : MonoBehaviour
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
        gameObject.transform.Rotate(0.0f, (Time.deltaTime * speed), 0.0f, Space.Self);
    }
}
