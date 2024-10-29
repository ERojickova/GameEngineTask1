using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject otherTeleport;
    public GameObject player;
    public bool teleportActiveted = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && teleportActiveted) 
        {
            otherTeleport.GetComponent<Teleport>().teleportActiveted = false;
            
            Vector3 newPosition = otherTeleport.transform.position;
            player.transform.position = newPosition;
        }
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            teleportActiveted = true;
        }
    }
}
