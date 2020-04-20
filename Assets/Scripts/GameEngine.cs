using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameEngine : MonoBehaviour
{
    [SerializeField] Renderer roadRenderer;
    
    private Camera cam;
    public Animator _anim;

    [SerializeField] GameObject cactusPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] interactableObjectPrefabs;

    [SerializeField] private Text innerVoice;
    [SerializeField] private Text theEndText;
    [SerializeField] Image[] speedBars;
    [SerializeField] Slider gasBar;
    [SerializeField] Slider hungerBar;
    [SerializeField] Slider egoBar;
    [SerializeField] Slider adrenalinBar;
    [SerializeField] Slider ageBar;

    private float currentScrollSpeed;
    private float scrollSpeed;

    private Player _player;

    
    private bool _gameStarted=false;
    private bool canSpawn = true;

    private void Start()
    {
        cam = Camera.main;
        _player = FindObjectOfType<Player>();
        InvokeRepeating("CreateBackGroundObject",0f,Random.Range(1.5f,2.5f));
    }

    private void Update()
    {
        if(!_gameStarted) return;
        roadRenderer.material.mainTextureOffset+=new Vector2(currentScrollSpeed*Time.deltaTime,0);
        currentScrollSpeed = Mathf.Lerp(currentScrollSpeed, scrollSpeed, 2f * Time.deltaTime);
        DecreaseStats();
        
    }

    void DecreaseStats()
    {
        gasBar.value -= 0.1f * Time.deltaTime;
        hungerBar.value -= 0.13f * Time.deltaTime;
        egoBar.value -= 0.20f * Time.deltaTime;
        adrenalinBar.value -= (0.45f-scrollSpeed/4) * Time.deltaTime;
        ageBar.value -= 0.05f * Time.deltaTime;
        CheckDeath();
    }

    void CheckDeath()
    {
        if (gasBar.value <= 0) TheEnd("You forgot how you can drive");
        if (hungerBar.value <= 0) TheEnd("You starved to death");
        if (adrenalinBar.value <= 0) TheEnd("You are bored to death");
        if (egoBar.value <= 0) TheEnd("You lost yourself");
        if (ageBar.value <= 0) TheEnd("You can't drive forever");
    }
    
    public void GameStarted()
    {
        _gameStarted = true;
    }
    public void GearUp(int gear)
    {
        scrollSpeed += 0.5f;
        speedBars[gear-1].gameObject.SetActive(true);
    }

    public void GearDown(int gear)
    {
        scrollSpeed -= 0.5f;
        speedBars[gear].gameObject.SetActive(false);
    }
    public void ChangedLane()
    {
        adrenalinBar.value += 0.5f;
    }
    public float GetSpeed()
    {
        return currentScrollSpeed;
    }

    public void CreateBackGroundObject()
    {
        Instantiate(cactusPrefab,
            new Vector3(cam.transform.position.x + Random.Range(10f,15f), Random.Range(-1.5f, 0.6f), -0.5f),
           Quaternion.identity);
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab,
            new Vector3(cam.transform.position.x + 10f, 1.05f, -0.4f),
            enemyPrefab.transform.rotation);
    }

    public void SpawnInteractableObject()
    {
        Vector3 spawnPos;
        if (egoBar.value < 5f)
        {
            spawnPos = GetSpawnPosition();
            float randomFactor = Random.Range(15f, 20f);
            Instantiate(interactableObjectPrefabs[0], spawnPos+Vector3.right*randomFactor, Quaternion.identity);
        }
        if (hungerBar.value < 5f)
        {
            spawnPos = GetSpawnPosition();
            float randomFactor = Random.Range(25f, 30f);
            Instantiate(interactableObjectPrefabs[1], spawnPos+Vector3.right*randomFactor, Quaternion.identity);
        }
        if (gasBar.value < 3f)
        {
            spawnPos = GetSpawnPosition();
            float randomFactor = Random.Range(35f, 40f);
            Instantiate(interactableObjectPrefabs[2], spawnPos+Vector3.right*randomFactor, Quaternion.identity);
        }
        else
        {
            spawnPos = GetSpawnPosition();
            Instantiate(interactableObjectPrefabs[Random.Range(0,3)],
                spawnPos+Vector3.right*40f, Quaternion.identity);
        }
    }
    public Vector3 GetSpawnPosition()
    {
        var places =FindObjectsOfType<InteractableObject>();
        float xMax = 0;
        foreach (var go in places)
        {
            if (go.transform.position.x > xMax) xMax = go.transform.position.x;
        }
        return new Vector3(xMax,3.45f,-0.2f);
    }
    public void StoppedAtGasStation()
    {
        var i = Random.Range(0, 2);
        if (i == 0)
        {
            innerVoice.text = "This is a shitty gas";
            gasBar.value += 2.5f;
        }

        if (i == 1)
        {
            innerVoice.text = "Oh yeah baby";
            gasBar.value += 5f;
        }
    }
    public void StoppedAtRestaurant()
    {
        var i = Random.Range(0, 3);
        if (i == 0)
        {
            innerVoice.text = "I hate fish..";
            hungerBar.value += 4f;
        }

        if (i == 1)
        {
            innerVoice.text = "Chicken was okay";
            hungerBar.value += 6f;
        }

        if (i == 2)
        {
            innerVoice.text = "Best burger ever";
            hungerBar.value += 8f;
        }
    }
    public void StoppedAtHouse()
    {
        var i = Random.Range(0, 3);
        if (i == 0)
        {
            innerVoice.text = "Worst date ever..";
            egoBar.value += 6f;
            adrenalinBar.value += 4f;
        }

        if (i == 1)
        {
            innerVoice.text = "I had better nights";
            egoBar.value += 8f;
            adrenalinBar.value += 6f;
        }

        if (i == 2)
        {
            innerVoice.text = "I think I fell in love";
            egoBar.value += 10f;
            adrenalinBar.value += 10f;
        }
    }

    public void TheEnd(string text)
    {
        scrollSpeed = 0;
        _player.TheEnd();
        theEndText.text = text;
        _anim.Play("TheEndCanvas");
        
    }

    public void PlayAgainButtonTapped()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGameButtonTapped()
    {
        Application.Quit();
    }
}
