using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCar : MonoBehaviour
{
    private Player _player;
    [SerializeField] GameEngine ge;

    private void Start()
    {
        _player = transform.parent.gameObject.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ge.TheEnd("You Crashed");
        }
        if (other.CompareTag("StopPosition"))
        {
            _player.Stopped();
            Ray rayUp = new Ray(transform.position, Vector3.forward);
            
            RaycastHit hit;
            if (Physics.Raycast(rayUp, out hit, 0.5f,
                LayerMask.GetMask("InteractableObject")))
            {
                if(hit.collider.CompareTag("GasStation")) ge.StoppedAtGasStation();
                if(hit.collider.CompareTag("Restaurant")) ge.StoppedAtRestaurant();
                if(hit.collider.CompareTag("House")) ge.StoppedAtHouse();
            }
        }
    }
}
