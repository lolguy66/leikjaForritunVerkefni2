using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerFollower : MonoBehaviour
{
    // Byrjunar breytur
    public Transform player;
    public Vector3 offset;
 

    // Update is called once per frame
    void Update()
    {
        // myndavél elta leikmann.
        transform.position = player.position+offset;
        //transform.rotation = player.rotation ;
       
    }
  
}

