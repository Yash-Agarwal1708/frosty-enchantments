using UnityEngine;
using System.Collections;

public class Up : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0f, 2.5f, -2f);
    public float arrivalDistance = 0.1f;
    public float rotationDuration = 1.25f;

    private bool hasArrived = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float elapsedTime = 0f;

    private SoundManager soundManager;

    void Start()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        if (!hasArrived && Vector3.Distance(GameObject.Find("Player").transform.position, targetPosition) < arrivalDistance)
        {
            hasArrived = true;
            if (soundManager != null)
            {
                soundManager.PlayRotateSound();
            }
            else
            {
                soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
                soundManager.PlayRotateSound();
            }
            StartCoroutine(SmoothRotation());
        }
    }

    IEnumerator SmoothRotation()
    {
        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
