using UnityEngine;
using System.Collections;

public class RotateWithPlayer : MonoBehaviour
{
    //Player related variables
    private GameObject player;
    private PlayerController playerController;
    private SpawnManager spawnManager;
    private float rotateSpeed = 80;
    public bool rotate;

    // Start is called before the first frame update
    public void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        Debug.Log("isGameActive is = " + spawnManager.isGameActive);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnManager.isGameActive == true)
        {
            RotateModelWithPlayer();
        }
               
    }

    IEnumerator RotationTime()
    {
        yield return new WaitForSeconds(1);
        rotate = false;
        Animator raccoonAnim = GetComponent<Animator>();
        raccoonAnim.enabled = false;
        playerController.smoke.Stop();
    }

    //Rotate with Player Method
    void RotateModelWithPlayer()
    {
        if (playerController.verticalInput > 0 && playerController.horizontalInput == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        if (playerController.verticalInput > 0 && playerController.horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
        }

        if (playerController.verticalInput > 0 && playerController.horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
        }

        if (playerController.verticalInput < 0 && playerController.horizontalInput == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }

        if (playerController.verticalInput < 0 && playerController.horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 135, 0);
        }

        if (playerController.verticalInput < 0 && playerController.horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, -135, 0);
        }

        if (playerController.verticalInput == 0 && playerController.horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        if (playerController.verticalInput == 0 && playerController.horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }


    public void DeathAnimation()
    {
        if (rotate)
        {
            StartCoroutine(RotationTime());
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }
    
}
