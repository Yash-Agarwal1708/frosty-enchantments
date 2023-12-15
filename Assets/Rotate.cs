using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private bool isRotating = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    [SerializeField]
    private float rotateTime = 2f;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = !isRotating;
            targetRotation = isRotating ? Quaternion.Euler(90, 0, 0) : originalRotation;
            FindObjectOfType<SoundManager>().PlayRotateSound();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateTime);
    }
}
