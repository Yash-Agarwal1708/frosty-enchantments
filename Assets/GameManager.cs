using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player;
    public List<PathCondition> pathConditions = new List<PathCondition>();
    public List<Transform> pivots;

    public Transform[] objectsToHide;

    private SoundManager soundManager;

    [SerializeField]
    private float rotateTime = 1.272f;

    private void Awake()
    {
        instance = this;
        soundManager = GetComponent<SoundManager>();
    }

    void Update()
    {
        //print((Mathf.Abs(Vector3.Distance(pathConditions[0].conditions[0].conditionObject.eulerAngles, pathConditions[0].conditions[0].eulerAngle))>=0f && Mathf.Abs(Vector3.Distance(pathConditions[0].conditions[0].conditionObject.eulerAngles, pathConditions[0].conditions[0].eulerAngle))<=5f)|| (Mathf.Abs(Vector3.Distance(pathConditions[0].conditions[0].conditionObject.eulerAngles, pathConditions[0].conditions[0].eulerAngle)) >= 355f && Mathf.Abs(Vector3.Distance(pathConditions[0].conditions[0].conditionObject.eulerAngles, pathConditions[0].conditions[0].eulerAngle)) <= 360f));
        //print(pathConditions[0].conditions[0].conditionObject.rotation);
        //print(Vector3.Angle(pathConditions[0].conditions[0].conditionObject.eulerAngles, pathConditions[0].conditions[0].eulerAngle));

        foreach (PathCondition pc in pathConditions)
        {
            int count = 0;
            //print(pc.conditions.Count);
            for (int i = 0; i < pc.conditions.Count; i++)
            {
                //print(pc.conditions[i].conditionObject.eulerAngles + " " + pc.conditions[i].eulerAngle);
                //print(Vector3.Angle(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle));

                //if (pc.conditions[i].conditionObject.eulerAngles == pc.conditions[i].eulerAngle)
                //print((Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) >= 0f && Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) <= 5f) || (Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) >= 355f && Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) <= 360f));
                if ((Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) >= 0f && Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) <= 5f) || (Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) >= 355f && Mathf.Abs(Vector3.Distance(pc.conditions[i].conditionObject.eulerAngles, pc.conditions[i].eulerAngle)) <= 360f))
                {
                    count++;
                    //print(count + "hello");
                }
            }
            foreach (SinglePath sp in pc.paths)
            {
                //print(count + " " + pc.conditions.count);
                sp.block.possiblePaths[sp.index].active = (count == pc.conditions.Count);
            }
        }

        if (player.walking)
            return;

        if(pivots.Count!=0)
        {
            //print(pivots.Count);
        if (Input.GetMouseButtonDown(1))
        {
            soundManager.PlayRotateSound();
            pivots[0].DOComplete();
            pivots[0].DORotate(new Vector3(0, 90, 0), rotateTime, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);
        }

        foreach (Transform t in objectsToHide)
        {
            t.gameObject.SetActive(pivots[0].eulerAngles.y > 45 && pivots[0].eulerAngles.y < 90 + 45);
        }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

    }

    public void RotateRightPivot()
    {
        soundManager.PlayRotateSound();
        pivots[1].DOComplete();
        pivots[1].DORotate(new Vector3(0, 0, 90), rotateTime).SetEase(Ease.OutBack);
    }
}

[System.Serializable]
public class PathCondition
{
    public string pathConditionName;
    public List<Condition> conditions;
    public List<SinglePath> paths;
}
[System.Serializable]
public class Condition
{
    public Transform conditionObject;
    public Vector3 eulerAngle;

}
[System.Serializable]
public class SinglePath
{
    public Walkable block;
    public int index;
}