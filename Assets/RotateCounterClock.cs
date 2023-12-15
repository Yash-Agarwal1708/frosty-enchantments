using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCounterClock : MonoBehaviour
{
    private float rotationSpeed = 90f;
    private bool isRotating = true;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    [SerializeField]
    private float rotateTime = 2f;
    public PlayerController player;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = originalRotation;
    }

    void Update()
    {
        if (player.walking)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<SoundManager>().PlayRotateSound();
            if (isRotating)
            {
                targetRotation *= Quaternion.Euler(-90, 0, 0);
                targetRotation.x = targetRotation.x % 360;
                //print(targetRotation.eulerAngles);

            }
        }

        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateTime);
        }
    }
}