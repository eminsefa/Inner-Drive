using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    [SerializeField] GameEngine ge;
    
    [SerializeField] Transform[] carPositions;
    [SerializeField] Transform childCar;
    
    private int _gear = 0;

    private float moveSpeed = 0;
    private float currentMoveSpeed;
    private float _lane = 2f;

    private bool goingDown = false;
    private bool isAlive = true;
    enum Gear
    {
        Gear0,
        Gear1,
        Gear2,
        Gear3
    };
    private Gear gearNumber;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _anim.speed = 0;
        currentMoveSpeed = 0;
    }

    void Update()
    {
        var pos = transform.position;
        if (isAlive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ge.GameStarted();
                _anim.speed = 0.25f;
                if (_gear <3)
                {
                    _gear++;
                    moveSpeed += 1f;
                    ge.GearUp(_gear);
                    if (_gear == 1) moveSpeed = 1f;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_gear >0)
                {
                    _gear--;
                    moveSpeed -= 1f;
                    ge.GearDown(_gear);
                    if (_gear == 0) moveSpeed = 15f;
                    if (_gear == 1) moveSpeed = 1f;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(_gear==0) return;
                if (childCar.position.y<=1.1f && goingDown)
                {
                    _lane = 2f;
                    ge.ChangedLane();
                    _anim.Play("MoveUp");
                    goingDown = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(_gear==0) return;
                if (childCar.position.y>=1.9f && !goingDown)
                {
                    _lane = 1f;
                    _anim.Play("MoveDown");
                    ge.ChangedLane();
                    goingDown = true;
                }
            }
            _rb.position = Vector3.MoveTowards(pos,
                new Vector3(carPositions[_gear].position.x,pos.y,0),
                currentMoveSpeed*Time.deltaTime/7f);
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, moveSpeed, 2f*Time.deltaTime);
            if (Mathf.Abs(currentMoveSpeed - moveSpeed) < 0.01f) currentMoveSpeed = moveSpeed;
        }
       
    }

    public void Stopped()
    {
        moveSpeed = 0;
    }

    public void TheEnd()
    {
        isAlive = false;
        if (childCar.position.y >= 1.5f) _anim.Play("TheEndPlayerUp");
        if(childCar.position.y<1.5f) _anim.Play("TheEndPlayerDown");
    }

    public int GetGear()
    {
        return _gear;
    }

}
