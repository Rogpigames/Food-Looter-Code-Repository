using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Player related variables
    private PlayerController player;

    //Spawn Manager related variables

    private SpawnManager spawnManager;

    //Movement related variables
    private Vector3 offset;
    public bool isTired = false;

    //Boundarie related variables

    public float rightBound = 17f;
    public float leftBound = -17f;
    public float upBound = 15.5f;
    public float downBound = -16.5f;
    private float randomZDirection;
    private float randomXDirection;
    public bool directionChosen = false;
    public float boundsOffset = 1.63f;

    //Building / Stall related variables

    private int boundNumber = 4;
    public int boundDirection = 4;
    public bool hitWall = false;
    public int randomDirection;
    private int randomDirectionsNumber = 2;


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        offset = new Vector3(0, (transform.position.y - player.transform.position.y), 0);
        StartCoroutine(IsTiredCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnManager.isGameActive == true)
        {
            if (!isTired)
            {
                FollowPlayer();
            }

            if (isTired && !hitWall)
            {
                GoToBounds();
                DestroyBounds();
            }

            if (isTired && hitWall)
            {
                AvoidWall();
            }
        }
    }


    //Method for following the player
    void FollowPlayer()
    {
        //This sums the transform.position of the player with the offset, so when is substracted from the transform.position, it
        //will always be on the same spot and there's won't be collisions
        transform.Translate(((player.transform.position + offset) - transform.position) * spawnManager.enemySpeed * Time.deltaTime);
    }

    //Method for going to Bounds and get deleted 

    void GoToBounds()
    {
        if (boundDirection == 0)
        {
            //With (leftBound - 1) in the X axis it hits the bounds and it's destroyed (working as intended)
            transform.Translate((new Vector3((leftBound - 1), transform.position.y, randomZDirection) - transform.position) * spawnManager.enemySpeed * Time.deltaTime);
        }
        if (boundDirection == 1)
        {
            //With (rightBound + 1) in the X axis it hits the bounds and it's destroyed (working as intended)
            transform.Translate((new Vector3((rightBound + 1), transform.position.y, randomZDirection) - transform.position) * spawnManager.enemySpeed * Time.deltaTime);
        }
        if (boundDirection == 2)
        {
            //With (upBound + 1) in the Z axis it hits the bounds and it's destroyed (working as intended)
            transform.Translate((new Vector3(randomXDirection, transform.position.y, (upBound + 1)) - transform.position) * spawnManager.enemySpeed * Time.deltaTime);
        }
        if (boundDirection == 3)
        {
            //With (downBound - 1)) in the Z axis it hits the bounds and it's destroyed (working as intended)
            transform.Translate((new Vector3(randomXDirection, transform.position.y, (downBound - 1)) - transform.position) * spawnManager.enemySpeed * Time.deltaTime);
        }
    }


    //Method for checking Bounds
    private void DestroyBounds()
    {
        //Horizontal Bounds
        if (transform.position.x > rightBound && isTired)
        {
            Destroy(gameObject);
        }

        if (transform.position.x < leftBound && isTired)
        {
            Destroy(gameObject);
        }
        //Vertical Bounds
        if (transform.position.z > upBound && isTired)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < downBound && isTired)
        {
            Destroy(gameObject);
        }
    }

    //Method for moving up or down when hitting the building

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            randomDirection = Random.Range(0, randomDirectionsNumber);
            hitWall = true;
            if (!directionChosen)
            {
                RandomZDirection();
                RandomXDirection();
                boundDirection = Random.Range(0, boundNumber);
                directionChosen = true;
            }
            StartCoroutine(HitWallCoroutine());
        }

        if (collision.gameObject.CompareTag("Stall"))
        {
            randomDirection = Random.Range(0, randomDirectionsNumber);
            hitWall = true;
            if (!directionChosen)
            {
                RandomZDirection();
                RandomXDirection();
                boundDirection = Random.Range(0, boundNumber);
                directionChosen = true;
            }
            StartCoroutine(HitWallCoroutine());
        }
    }


    //Coroutine that determines when the character will get tired and go to offbounds

    IEnumerator IsTiredCoroutine()
    {
        yield return new WaitForSeconds(8);
        boundDirection = Random.Range(0, boundNumber);
        StartCoroutine(DeadCoroutine());

        isTired = true;
    }

    //Hit Wall Coroutine

    IEnumerator HitWallCoroutine()
    {
        yield return new WaitForSeconds(3);
        hitWall = false;
    }

    //Method for avoiding the walls

    void AvoidWall()
    {
        if (randomDirection == 0)
        {
            transform.Translate(Vector3.forward * spawnManager.enemySpeed * Time.deltaTime, Space.World);
        }

        if (randomDirection == 1)
        {
            transform.Translate(-Vector3.forward * spawnManager.enemySpeed * Time.deltaTime, Space.World);
        }
        
    }

    //Method for generating randomZ and randomX direction

    private void RandomXDirection()
    {
        //Checks what position is closer, right or up, that ways the Enemy will always go to a place where is closer to return
        if (transform.position.x < 0)
        {
            randomXDirection = Random.Range(leftBound, 0);
        }

        if (transform.position.x > 0)
        {
            randomXDirection = Random.Range(0, rightBound);
        }
        else
        {
            if (boundDirection == 2)
            {
                //When going up to hit up boundaries, calculate what position from the X axis, the enemy will go to be destroyed
                randomXDirection = Random.Range(leftBound, rightBound);
            }
            if (boundDirection == 3)
            {
                //When going up to hit down boundaries, calculate what position from the X axis, the enemy will go to be destroyed
                randomXDirection = Random.Range(leftBound, rightBound);
            }
        }
    }

    void RandomZDirection()
    {
        //Checks what position is closer, right or up, that ways the Enemy will always go to a place where is closer to return
        if (transform.position.z < 0)
        {
            randomXDirection = Random.Range(downBound, 0);
        }

        if (transform.position.z > 0)
        {
            randomXDirection = Random.Range(0, upBound);
        }
        else
        {
            if (boundDirection == 0)
            {
                //When going up to hit left boundaries, calculate what position from the Z axis, the enemy will go to be destroyed
                randomZDirection = Random.Range(downBound, upBound);
            }
            if (boundDirection == 1)
            {
                //When going up to hit right boundaries, calculate what position from the Z axis, the enemy will go to be destroyed
                randomZDirection = Random.Range(downBound, upBound);
            }
        }
    }

    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(3);
        isTired = false;
        Destroy(gameObject);
    }
}
