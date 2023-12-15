using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    private bool isRotating = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    public float angle = -90;

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
            targetRotation = Quaternion.Euler(0, angle, 0);
            angle -= 90;
            angle = angle%360;
            FindObjectOfType<SoundManager>().PlayRotateSound();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateTime);
    }
}
