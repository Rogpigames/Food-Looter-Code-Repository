using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private PlayerController player;
    public Enemy enemyScript;
    private SpawnManager spawnManager;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {       
        enemyScript = enemyScript.GetComponent<Enemy>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        offset = enemyScript.transform.position.y - transform.position.y;
        StartCoroutine(GetPlayerController());
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnManager.isGameActive == true)
        {
            transform.position = new Vector3(enemyScript.transform.position.x, enemyScript.transform.position.y - offset, enemyScript.transform.position.z);
            if (enemyScript.isTired == false)
            {
                transform.LookAt(player.transform.position);
            }
            else if (enemyScript.isTired == true)
            {
                if (enemyScript.boundDirection == 0)
                {
                    //Look at left
                    transform.LookAt(-Vector3.right);
                }
                if (enemyScript.boundDirection == 1)
                {
                    //Look at right
                    transform.LookAt(Vector3.right);
                }
                if (enemyScript.boundDirection == 2)
                {
                    //Look up
                    transform.LookAt(Vector3.forward);
                }
                if (enemyScript.boundDirection == 3)
                {
                    //Look down
                    transform.LookAt(-Vector3.forward);
                }
                if (enemyScript.randomDirection == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                if (enemyScript.randomDirection == 1)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
            
    }

    IEnumerator GetPlayerController()
    {
        yield return new WaitForEndOfFrame();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

}
