using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stall : MonoBehaviour
{
    //Color related variables
    private Renderer stallRenderer;
    private Color stallOgColor;
    private Material stallMaterial;

    //Getting collectibles and related variables

    public List<GameObject> collectibles;
    public float launchForce = 1;
    private int foodNumber = 3;
    private int dropRate = 101;

    //Steal Related variables
    public bool canSteal = true;
    public bool canBeStole = true;
    public bool stolen = false;
    public bool dropFood = false;
    public float stealDuration = 0;

    //Player related variables

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //Get the color related components
        stallRenderer = GetComponent<Renderer>();
        stallMaterial = stallRenderer.material;
        stallOgColor = stallMaterial.color;
        //Get the Player Component
        player = player.GetComponent<PlayerController>();
        InstantiateCollectibles();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {

    }

    //Cooldown after player steal from the stall and determines how much time you have to wait before stealing again
    public IEnumerator StallStealCooldown(int stealTime)
    {
        yield return new WaitForSeconds(stealTime);
        stallRenderer = GetComponent<Renderer>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Material stallMaterial = stallRenderer.material;
        stallMaterial.color = stallOgColor;
        canBeStole = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //When the collectible inside the stall and dropFood == true, then it will launch the collectibles and make them visible
        if (other.gameObject.CompareTag("Collectible") && dropFood == true)
        {
            Renderer collectibleRenderer = other.GetComponent<Renderer>();
            Rigidbody collectibleRb = other.GetComponent<Rigidbody>();
            Collectible collectible = other.GetComponent<Collectible>();
            //Launches or moves an physical object to a direction related to it's own coordinates, not the world. 
            collectibleRb.AddRelativeForce(Vector3.right * launchForce, ForceMode.Impulse);
            collectibleRenderer.enabled = false;
            collectibleRb.useGravity = true;
            collectible.wasLaunch = true;
            collectible.transform.rotation = Quaternion.Euler(0, 0, 0); //With this the objects don't clip into the ground
            StartCoroutine(DropFood());
        }
    }

    void InstantiateCollectibles()
    {
        for (int i = 0; i < foodNumber; i++)
        {
            Instantiate(collectibles[player.DropFood(Random.Range(0, dropRate))], transform.position, transform.rotation);
        }
    }

    IEnumerator DropFood()
    {
        yield return new WaitForEndOfFrame();
        dropFood = false;
    }
}
