using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    //Audio related variables

    private AudioSource audioSource;
    public AudioClip eatingSound;

    //Enemy related variables
    public GameObject enemy;
    public Enemy enemyScript;
    private Vector3 enemySpawnPosition;
    private int enemyCount = 0;
    private int numberOfEnemies = 1;

    //Camera related variables


    //Game state related variables

    public bool isGameActive = false;

    //Player related variables

    public PlayerController player;

    //Score related values
    public int score = 0;

    //UI Related Variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI canStealText;
    public TextMeshProUGUI cannotStealText;
    public Button restartButton;

    //Enemy spawn position related variables
    private float[] enemySpawnPosX = new float[2];
    private float enemySpawnPosY;
    private float[] enemySpawnPosZ = new float[4];
    public float enemySpeed;
    //Wave related variables
    private int waveCounter = 0;

    // Start is called before the first frame update
    public void StartGame()
    {
        player.gameObject.SetActive(true);
        player.StartGame();
        enemyScript = enemy.GetComponent<Enemy>();
        //Start the game
        isGameActive = true;
        AssignEnemySpawnPos();
        SpawnEnemy(numberOfEnemies);
        enemySpeed = 0.6f;
        audioSource = GetComponent<AudioSource>();
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameActive)
        {
            EnemyWaveManager();
        }        
    }

    //Enemy spawn


    //If enemy numbers == 0, then, spawn Enemies. Every wave enemies get more faster

    void EnemyWaveManager()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            WaveCheck();
            SpawnEnemy(numberOfEnemies);
        }
    }

    //Assign All Axis for Enemy Spawn positions in one method
    void AssignEnemySpawnPos()
    {
        AssignEnemySpawnPosX();
        AssignEnemySpawnPosY();
        AssignEnemySpawnPosZ();
    }

    //Assign Enemy Spawn positions for X axis for fixed predetermined random positions

    void AssignEnemySpawnPosX()
    {
        enemySpawnPosX[0] = -17f;
        enemySpawnPosX[1] = 17f;
    }

    //Assign Enemy Spawn positions for Y axis for fixed predetermined random positions

    void AssignEnemySpawnPosY()
    {
        enemySpawnPosY = 1.63f;
    }

    //Assign Enemy Spawn positions for Z axis for fixed predetermined random positions

    void AssignEnemySpawnPosZ()
    {
        enemySpawnPosZ[0] = -1f;
        enemySpawnPosZ[1] = -11f;
        enemySpawnPosZ[2] = 6f;
        enemySpawnPosZ[3] = 16f;
    }
    void WaveCheck()
    {
        if (waveCounter == 4)
        {
            numberOfEnemies++;
            enemySpeed += 0.1f;
            waveCounter = 0;
            waveCounter++;
            Debug.Log("Number of enemies is = " + numberOfEnemies);
        }
        else
        {
            waveCounter++;
            Debug.Log("Wave counter is = " + waveCounter);
        }
    }

    // Generate Random Enemy Spawn Position
    private int RandomEnemySpawnPositionX()
    {
        int randomSpawnPositionX = Random.Range(0, enemySpawnPosX.Length);
        return randomSpawnPositionX;
    }

    private int RandomEnemySpawnPositionZ()
    {
        int randomSpawnPositionZ = Random.Range(0, enemySpawnPosZ.Length);
        return randomSpawnPositionZ;
    }

    private void SpawnEnemy(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemySpawnPosition = new Vector3(enemySpawnPosX[RandomEnemySpawnPositionX()], enemySpawnPosY, enemySpawnPosZ[RandomEnemySpawnPositionZ()]);
            Instantiate(enemy, enemySpawnPosition, enemy.transform.rotation);
        }

    }

    public void ScoreAdd(int scorevalue)
    {
        score += scorevalue;
        scoreText.text = "Score: " + score;
    }

    public void EatingSound()
    {
        audioSource.PlayOneShot(eatingSound);
    }

    public void GameOver()
    {
        //To use set active you have to point out was type of object it is, in this case, it's a gameObject you want to active or
        //inactivate
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        //Load scene method, requires the name of the scene or another scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
