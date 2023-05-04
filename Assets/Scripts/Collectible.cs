using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //Score related variables

    public int scoreValue;

    //Particle related variables

    public ParticleSystem eatExplosionEffect;
    private bool canBeCollected = true;

    //Renderer related variables

    private Renderer mRenderer;
    public Renderer foodRenderer;

    //Spawn Manager related variables

    private SpawnManager spawnManager;

    //Stall related variables
    public bool outOfStall = false;
    private int stallOutDistanceTime = 2;

    //Physics related variables
    private Rigidbody collectibleRb;
    private SphereCollider collectibleCollider;
    public float launchForce = 10;
    public float rotationSpeed = 90;
    public float speed = 5;

    //Bool related variables
    public bool wasLaunch = false;
    public bool onGround = false;


    // Start is called before the first frame update
    void Start()
    {
        //Get SpawnManager component
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        collectibleRb = GetComponent<Rigidbody>();
        transform.Rotate(new Vector3(0, Random.Range(-45, 45), 45));
        //Get Renderer component
        mRenderer = GetComponent<Renderer>();
        mRenderer.enabled = false;

    }


    // Update is called once per frame
    void Update()
    {
        //Rotate the object when it hits the ground (next time I'll work on better descripted variables)
        if (wasLaunch)
        {
            //Rotate the object
            transform.Rotate(new Vector3(0, -rotationSpeed, 0) * Time.deltaTime);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        //When the collectible is on the ground, starts rotating, stops dropping and it's possible to grab it
        if (other.gameObject.CompareTag("Ground"))
        {
            collectibleRb = GetComponent<Rigidbody>();
            collectibleCollider = GetComponent<SphereCollider>();
            onGround = true;
            Debug.Log("On Ground = " + onGround);
            collectibleRb.useGravity = false;
            collectibleCollider.isTrigger = true;
            collectibleRb.Sleep();
            wasLaunch = true;
        }

        //When it collides with a Stall after being launched, it stops the force and the drops so the player can grab it
        if (other.gameObject.CompareTag("Stall") && wasLaunch)
        {
            collectibleRb = GetComponent<Rigidbody>();
            collectibleCollider = GetComponent<SphereCollider>();
            collectibleRb.Sleep();
            collectibleRb.WakeUp();
            wasLaunch = false;
        }

        //When it collides with a Building after being launched, it stops the force and the drops so the player can grab it
        if (other.gameObject.CompareTag("Building") && wasLaunch)
        {
            collectibleRb = GetComponent<Rigidbody>();
            collectibleCollider = GetComponent<SphereCollider>();
            collectibleRb.Sleep();
            collectibleRb.WakeUp();
            wasLaunch = false;
        }

        if (other.gameObject.CompareTag("Wall") && wasLaunch)
        {
            collectibleRb = GetComponent<Rigidbody>();
            collectibleCollider = GetComponent<SphereCollider>();
            collectibleRb.Sleep();
            collectibleRb.WakeUp();
            wasLaunch = false;
        }

        if (other.gameObject.CompareTag("Stall") && onGround && !outOfStall)
        {
            StartCoroutine(FixingCollisionCoroutine());
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if(canBeCollected)
            {               
                spawnManager.ScoreAdd(scoreValue);
                spawnManager.EatingSound();
                eatExplosionEffect.Play();
                foodRenderer.enabled = false;
                Debug.Log("Score: " + spawnManager.score);
                StartCoroutine(CollectedCoroutine());
                canBeCollected = false;
            }         
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Stall") && onGround && !outOfStall)
        {
            transform.rotation = other.transform.rotation;
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }
    }

    IEnumerator FixingCollisionCoroutine()
    {
        yield return new WaitForSeconds(stallOutDistanceTime);
        outOfStall = true;
    }

    IEnumerator CollectedCoroutine()
    {
        yield return new WaitForSeconds(1);
        
    }
}
