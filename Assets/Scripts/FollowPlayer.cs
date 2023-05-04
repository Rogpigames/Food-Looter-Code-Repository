using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private PlayerController player;
    private Vector3 offset;
    public SpawnManager spawnManager;
    private bool gotPlayerController = false;

    // Start is called before the first frame update
    public void Start()
    {           
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        if (spawnManager.isGameActive == true)
        {
            if(!gotPlayerController)
            {
                //Get Player Component
                player = GameObject.Find("Player").GetComponent<PlayerController>();
                offset = new Vector3(0, 10 - player.transform.position.y, 0);
                gotPlayerController = true;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(spawnManager.isGameActive == true)
        {
            //Follows Player
            transform.position = player.transform.position + offset;
        }
        
    }
}
