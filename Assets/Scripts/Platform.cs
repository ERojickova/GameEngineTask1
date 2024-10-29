using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject player; // This is our player on the scene. 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Check every frame if the player is higher than this gameobject, then change this gameobject layer to active layer

        if(transform.position.y < player.transform.position.y - transform.localScale.y  - player.transform.localScale.y/2)
        {
            // Change the layer and the color of the platform
            gameObject.layer = LayerMask.NameToLayer("PlatformActive");
            GetComponent<Renderer>().material.color = Color.cyan;
        }

    }


}
