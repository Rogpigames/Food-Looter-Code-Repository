using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartButton : MonoBehaviour
{
    public Button button;
    public SpawnManager spawnManager;
    public TextMeshProUGUI titleScreen;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        spawnManager.isGameActive = true;
        titleScreen.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        spawnManager.StartGame();       
    }
}
