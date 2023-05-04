using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //UI variables

    bool canStealUI;

    //Audio related variables

    private AudioSource audioSource;
    public AudioClip raccoonSound;
    public AudioClip raccoonDeathSound;
    public AudioClip deathSound;

    //Physics related variables

    private float speed = 10.0f;

    //Particle System related variables

    public ParticleSystem dirtSplat;
    public ParticleSystem smoke;

    //Animator / Raccoon related variables

    private Animator raccoonAnim;
    public RotateWithPlayer racoon;

    //Spawn Manager related variables

    private SpawnManager spawnManager;

    //Movement related variables

    public float horizontalInput;
    public float verticalInput;

    //Boundarie related variables

    private float rightBound = 18.6f;
    private float leftBound = -17.6f;
    private float upBound = 17.9f;
    private float downBound = -19.4f;

    //Enemy related variables

    public Enemy enemy;

    //Collectible related components and variables
    public List<GameObject> collectibles;
    private int foodDropChance = 101;
    private int collectiblesDropNumber = 3;

    //Stall related variables
    private Renderer stallRenderer;
    private Stall stall;
    private Color cooldownColor = new Color(0.247f, 0.031f, 0.471f);


    //Steal Related variables
    public bool canSteal = true;
    public bool stolen = false;
    int stealCount = 0;
    public float stealDuration = 0;
    private int stallStealCooldown = 7;


    // Start is called before the first frame update
    public void StartGame()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        raccoonAnim = GetComponentInChildren<Animator>();
        enemy = enemy.GetComponent<Enemy>();     
        dirtSplat = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        racoon.GetComponent<RotateWithPlayer>();
        InvokeRepeating("PlayRaccoonSound", 0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnManager.isGameActive == true)
        {
            Movement();
            LimitBounds();
            racoon.DeathAnimation();
        }       
    }

    private void Movement()
    {
        // Get Horizontal Input and Vertical Input from the player
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // Horizontal movement
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput, Space.World);
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput, Space.World);
        transform.rotation = Quaternion.Euler(0, 0, 1);    
        if(horizontalInput == 0 && verticalInput == 0)
        {
            if (dirtSplat.isPlaying == true)
            {
                dirtSplat.Stop();
            }
        }
        if (horizontalInput > 0 || verticalInput > 0)
        {
            if(dirtSplat.isPlaying == false)
            {
                dirtSplat.Play();
            }        
        }
        if (horizontalInput < 0 || verticalInput < 0)
        {
            if (dirtSplat.isPlaying == false)
            {
                dirtSplat.Play();
            }
        }
    }


    //Code that tracks that the player doesn't go Off bounds
    private void LimitBounds()
    {
        //Horizontal Bounds
        if (transform.position.x > rightBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x < leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        }
        //Vertical Bounds
        if (transform.position.z > upBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, upBound);
        }
        if (transform.position.z < downBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, downBound);
        }
    }


    //Method that checks objects that the player collides with
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && enemy.isTired == false)
        {
            if(spawnManager.isGameActive == true)
            {
                //Death related script
                racoon.rotate = true;
                audioSource.PlayOneShot(raccoonDeathSound, 0.7f);
                audioSource.PlayOneShot(deathSound, 0.7f);
                StartCoroutine(DeathSmokeCoroutine());
                spawnManager.GameOver();
                CancelInvoke();
            }
            
        }

    }


    private void OnTriggerStay(Collider other)
    {
        //Stall Color related variables
        stallRenderer = other.GetComponent<Renderer>();
        //Get the Stall script
        stall = other.GetComponent<Stall>();
        Material stallMaterial = stallRenderer.material;

        //Steal Mechanic that works with Stalls
        if (other.gameObject.CompareTag("Stall"))
        {           
            if(spawnManager.isGameActive == true)
            {
                //Starts the Steal sequence when the stall can be stole
                if (Input.GetKeyDown(KeyCode.E) && canSteal && stall.canBeStole)
                {
                    StartCoroutine(Steal());
                    canSteal = false;
                }

                //Executes the Steal action and starts the cooldown of the Stall
                if (Input.GetKey(KeyCode.E) && stolen)
                {
                    stealCount++;
                    stolen = false;
                    stall.canBeStole = false;
                    stall.dropFood = true;
                    stallMaterial.color = cooldownColor;
                    spawnManager.canStealText.gameObject.SetActive(false);
                    spawnManager.cannotStealText.gameObject.SetActive(true);
                    //Instantiate the amount of food the stall has
                    for (int i = 0; i < collectiblesDropNumber; i++)
                    {
                        //Drop Food will return the int number needed for the object to spawn
                        Instantiate(collectibles[DropFood(Random.Range(0, foodDropChance))], other.transform.position, other.transform.rotation);
                    }
                    StartCoroutine(stall.StallStealCooldown(stallStealCooldown));
                    Debug.Log("Amount of Steals = " + stealCount);
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Stall"))
        {
            spawnManager.canStealText.gameObject.SetActive(false);
            spawnManager.cannotStealText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stall"))
        {
            Stall actualStall = other.GetComponent<Stall>();
            if (actualStall.canBeStole == true)
            {
                spawnManager.canStealText.gameObject.SetActive(true);
            }
            if (actualStall.canBeStole == false)
            {
                spawnManager.cannotStealText.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator Steal()
    {
        while (stealDuration < 1)
        {
            // Track charge level, and use it in other scripts if desired
            stealDuration += Time.deltaTime;
            raccoonAnim.SetBool("Eat_b", true);
            //If the condition doesn't work, then it resets
            if (Input.GetKeyUp(KeyCode.E))
            {
                // Steal Failed
                Debug.Log("Steal Failed.");
                raccoonAnim.SetBool("Eat_b", false);
                stealDuration = 0;
                canSteal = true;
                yield break;
            }
            // wait here for next frame
            yield return null;
        }
        // Steal succeeded, and we're ready to complete the steal
        Debug.Log("Stolen");
        stolen = true;
        canSteal = true;
        stealDuration = 0;
        raccoonAnim.SetBool("Eat_b", false);
    }

    public int DropFood(int dropRate)
    {
        int foodNumber = 0;
        if (dropRate >= 0 && dropRate < 35)
        {
            foodNumber = 0;
        }
        if (dropRate >= 35 && dropRate < 70)
        {
            foodNumber = 1;
        }
        if (dropRate >= 70 && dropRate < 90)
        {
            foodNumber = 2;
        }
        if (dropRate >= 90 && dropRate < 101)
        {
            foodNumber = 3;
        }
        return foodNumber;
    }

    IEnumerator DeathSmokeCoroutine()
    {
        float smokeDuration = 0;
        while (smokeDuration < 1)
        {
            smokeDuration += Time.deltaTime;
            smoke.Play();
        }
        yield return null;
    }

    void PlayRaccoonSound()
    {
        audioSource.PlayOneShot(raccoonSound, 0.5f);
    }
}
