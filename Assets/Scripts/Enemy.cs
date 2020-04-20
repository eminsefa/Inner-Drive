using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private GameEngine _ge;

    private Player _player;
    private float moveSpeed;

    private void Start()
    {
        _ge = FindObjectOfType<GameEngine>();
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        float speed = RandomSpeed() ;
        moveSpeed = Mathf.Lerp(moveSpeed, speed* ((_player.GetGear() + 1f) / 3f), 2f * Time.deltaTime);
        transform.position+=new Vector3(-moveSpeed*Time.deltaTime,0,0);
    }
    float RandomSpeed()
    {
        float randomSpeed = Random.Range(4f, 6.0f);
        return randomSpeed;
    }
}
