using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public bool walking = false;

    [SerializeField]
    public Image image;
    

    [Space]

    public Transform currentCube;
    public Transform clickedCube;
    public Transform indicator;

    [Space]

    public List<Transform> finalPath = new List<Transform>();

    private float blend;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private GameObject winCube;

    private bool hasWon = false;


    // Timer class reference
    [SerializeField]
    private Timer timer;

    // PlayFab manager reference
    public PlayfabManager playfabManager;

    public bool True { get; private set; }

    void Start()
    {
        RayCastDown();
        soundManager = GameObject.FindAnyObjectByType<SoundManager>();
    }

    void Update()
    {

        //GET CURRENT CUBE (UNDER PLAYER)

        RayCastDown();

        if (currentCube.GetComponent<Walkable>().movingGround)
        {
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        if (currentCube == winCube.transform && !hasWon)
        {
            soundManager.PlayWinSound();
            hasWon = true;
            // adding the level completion time when player steps on the winning cube
            Point_Counter.points += timer.remainingDuration;
            Debug.Log("Current Points: "+Point_Counter.points);
            StartCoroutine(LoadAfterDelay());
        }

        // CLICK ON CUBE

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); RaycastHit mouseHit;

            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    clickedCube = mouseHit.transform;
                    DOTween.Kill(gameObject.transform);
                    finalPath.Clear();
                    FindPath();

                    blend = transform.position.y - clickedCube.position.y > 0 ? -1 : 1;

                    indicator.position = mouseHit.transform.GetComponent<Walkable>().GetWalkPoint();
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendCallback(() => indicator.GetComponentInChildren<ParticleSystem>().Play());
                    sequence.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.white, .1f));
                    sequence.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.black, .3f).SetDelay(.2f));
                    sequence.Append(indicator.GetComponent<Renderer>().material.DOColor(Color.clear, .3f));

                }
            }
        }
    }

    void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();

        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }

    void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        if (current == clickedCube)
        {
            return;
        }

        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }

        visitedCubes.Add(current);

        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    void BuildPath()
    {
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }

        finalPath.Insert(0, clickedCube);

        FollowPath();
    }

    void FollowPath()
    {
        Sequence s = DOTween.Sequence();

        walking = true;

        for (int i = finalPath.Count - 1; i > 0; i--)
        {
            float time = finalPath[i].GetComponent<Walkable>().isStair ? 1.5f : 1;

            s.Append(transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint(), .2f * time).SetEase(Ease.Linear));

            if (!finalPath[i].GetComponent<Walkable>().dontRotate)
                s.Join(transform.DOLookAt(finalPath[i].position, .1f, AxisConstraint.Y, Vector3.up));
        }

        if (clickedCube.GetComponent<Walkable>().isButton)
        {
            s.AppendCallback(() => GameManager.instance.RotateRightPivot());
        }

        s.AppendCallback(() => Clear());
    }

    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
        walking = false;
    }

    public void RayCastDown()
    {

        Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;

                if (playerHit.transform.GetComponent<Walkable>().isStair)
                {
                    DOVirtual.Float(GetBlend(), blend, .1f, SetBlend);
                }
                else
                {
                    DOVirtual.Float(GetBlend(), 0, .1f, SetBlend);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.GetChild(0).position, -transform.up);
        Gizmos.DrawRay(ray);
    }

    float GetBlend()
    {
        return GetComponentInChildren<Animator>().GetFloat("Blend");
    }
    void SetBlend(float x)
    {
        GetComponentInChildren<Animator>().SetFloat("Blend", x);
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(2);
        if(SceneManager.GetActiveScene().buildIndex == 5){
            playfabManager.SendLeaderBoard(Point_Counter.points);
        }   
        FindObjectOfType<LevelChanger>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }



    private void OnTriggerEnter(Collider other)
    {
       Debug.Log("collidede");
       if (other.gameObject.tag == "FakeGift")
       { 
           other.gameObject.transform.GetChild(6).gameObject.SetActive(true);
           Invoke("gameOver", 1.5f);
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(3).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(5).gameObject.SetActive(false);

        }
       else if(other.gameObject.tag =="Gift")
       {
           timer.remainingDuration += 4;
            other.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
            Invoke("deactivateGift",3);
            
       }
    }
    public void gameOver()
    {
        SceneManager.LoadScene(7);
    }
    public void deactivateGift()
    {
        image.gameObject.SetActive(false);
    }
}