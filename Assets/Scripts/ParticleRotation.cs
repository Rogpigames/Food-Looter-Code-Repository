using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateModelWithPlayer();
    }

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
}
