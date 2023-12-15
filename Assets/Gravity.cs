using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Rigidbody rb;
    public float speed = -20f;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 player_position = transform.position;
        RaycastHit hit;
        Vector3 position = new Vector3(player_position.x, player_position.y-1f, player_position.z);
        bool grounded = (Physics.Raycast(player_position, Vector3.down, out hit,1f)); // raycast down to look for ground is not detecting ground? only works if allowing jump when grounded = false; // return "Ground" layer as layer
        Debug.DrawRay((new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z)), Vector3.down, Color.green, 5);
        print(grounded);
        if(grounded)
            Debug.Log("Colpito: " + hit.collider.name);

        if (grounded ==false)
        {
            
            
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            player_position.y += Time.deltaTime * speed;
            transform.position = player_position;
        }

    }
}
