using UnityEngine;
using System.Collections;

public class RotateOnArrival : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(-4f, 3.5f, 2f);
    public float arrivalDistance = 0.1f;
    private bool hasArrived = false;
    private bool rotationTriggered = false;
    [SerializeField]
    private float rotationDuration = 1.272f;

    [SerializeField]
    private SoundManager soundManager;

    void Start()
    {
        soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        if (!hasArrived && Vector3.Distance(GetPlayerPosition(), targetPosition) < arrivalDistance)
        {
            if (soundManager != null)
            {
                soundManager.PlayRotateSound();
            }
            else
            {
                soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
                soundManager.PlayRotateSound();
            }
            hasArrived = true;
        }

        if (hasArrived && !rotationTriggered)
        {
            RotateObject();
        }
    }

    Vector3 GetPlayerPosition()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            return playerObject.transform.position;
        }

        return Vector3.zero;
    }

    void RotateObject()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(180f, 0f, 0f), 360f / rotationDuration * Time.deltaTime);

        if (Mathf.Approximately(transform.eulerAngles.x, 180.0f))
        {
            rotationTriggered = true;
        }
    }
}
