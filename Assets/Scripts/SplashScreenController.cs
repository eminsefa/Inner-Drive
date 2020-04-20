using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private GameObject cactusPrefab;
    [SerializeField] private Renderer roadRenderer;
    [SerializeField] private float scrollSpeed = 2f;
    private Camera cam;

    private void Start()
    {
        cam=Camera.main;
        InvokeRepeating("CreateBackGroundObject",0f,Random.Range(1f,2f));
    }

    private void Update()
    {
        
        roadRenderer.material.mainTextureOffset+=new Vector2(scrollSpeed*Time.deltaTime,0);
    }

    public void CreateBackGroundObject()
    {
        Instantiate(cactusPrefab,
            new Vector3(cam.transform.position.x + Random.Range(10f,15f), Random.Range(-2.5f, 0.6f), -0.5f),
            Quaternion.identity);
    }

    public float GetSpeed()
    {
        return scrollSpeed;
    }

    public void StartGameButtonTapped()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void QuitGameButtonTapped()
    {
        Application.Quit();
    }
}
