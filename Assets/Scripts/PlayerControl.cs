using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    
    public float force;
    public Rigidbody2D playerRB;

    public GUIStyle mystyle; 

    public float health;
    public float highPoint;
    public bool goingDown;
    public float damage;

    public Vector3 startPosition;

    public GameObject obstacleCollisionEffect;
    public GameObject endLevelEffect;



    // Start is called before the first frame update
    void Start()
    {
        mystyle.normal.textColor = Color.white;
        mystyle.fontSize = 16;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            force += 20 * Time.deltaTime;

        }

        if (Input.GetMouseButtonUp(0))
        {
            // This will transform mouse position from screenlocation to 3D world location
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePos);
            mousePos.z = 0;
            // Direction from mouse to the player is calculated using B-A vector locations
            Vector3 dir = (mousePos - transform.position).normalized;

            if(dir.y < 0)
            {
                dir *= -1;
            }

            Launch(force, dir);

        }

        // Let's check every frame when the ball start to come down.

        if(playerRB.velocity.y < -0.01 && goingDown == false)
        {
            // this is the moment (frame) the ball is starting to come down
            GetComponent<Renderer>().material.color = Color.red;
            goingDown = true;
            highPoint = transform.position.y;

        }

    }

    void Launch(float forceAmt, Vector3 forceDir)
    {
        GetComponent<Renderer>().material.color = Color.white;
        playerRB.AddForce(forceDir * forceAmt, ForceMode2D.Impulse);
        force = 0;
        goingDown = false; 

    }

    void TakeDamage(float dmgTaken)
    {
        health -= dmgTaken;
        if(health < 0)
        {
            Die();
        }

    }

    void Die()
    {
        // Move player back to the startposition
        transform.position = startPosition;
        health = 100;
        playerRB.velocity = Vector3.zero;
        // Let's take all platform to an array and go trough them with for each and change their color and layer to inactive. 
        GameObject[] allPlatforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach(GameObject singlePlatform in allPlatforms)
        {
            singlePlatform.layer = LayerMask.NameToLayer("PlatformInactive");
            //singlePlatform.GetComponent<Renderer>().material.color = Color.magenta;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Debug.Log("Hit a Platform, let's take some damage");
            if(goingDown == true)
            {
                damage = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y)) * Mathf.Abs(highPoint - transform.position.y);
                TakeDamage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(20);
            GameObject obsEfect = Instantiate(obstacleCollisionEffect, transform.position, Quaternion.identity);
            Destroy(obsEfect, 3);
        }

        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            GameObject endEffect = Instantiate(endLevelEffect, collision.gameObject.GetComponent<LevelEnd>().transform.position, Quaternion.identity);
            // SceneManager.LoadScene(collision.gameObject.GetComponent<LevelEnd>().nextLevel);
            StartCoroutine(WaitAndLoadNextLevel(collision.gameObject.GetComponent<LevelEnd>().nextLevel));
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            TakeDamage(-20);
        }

        if (collision.gameObject.CompareTag("Grow_up"))
        {
            Vector3 oldScale = transform.localScale;
            oldScale *= 2;
            transform.localScale = oldScale;
        }

        if (collision.gameObject.CompareTag("Shrink"))
        {
            Vector3 oldScale = transform.localScale;
            oldScale *= 0.5f;
            transform.localScale = oldScale;
        }
    }

    private IEnumerator WaitAndLoadNextLevel(string nextLevel)
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1.0f);

        // Load the next scene
        SceneManager.LoadScene(nextLevel);
    }



    private void OnGUI()
    {
        // this is just for debugging
        GUI.Label(new Rect(10, 10, 200, 20), "Force: " + force, mystyle);
        GUI.Label(new Rect(10, 30, 200, 20), "High Point: " + highPoint, mystyle);
        GUI.Label(new Rect(10, 50, 200, 20), "Going Down: " + goingDown, mystyle);
        GUI.Label(new Rect(10, 70, 200, 20), "Damage: " + damage, mystyle);
        GUI.Label(new Rect(10, 90, 200, 20), "Health: " + health, mystyle);
        GUI.Label(new Rect(10, 110, 200, 20), "Velocity Y: " + playerRB.velocity.y, mystyle);
    }

}
