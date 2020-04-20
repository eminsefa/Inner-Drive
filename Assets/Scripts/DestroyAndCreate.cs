using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DestroyAndCreate : MonoBehaviour
{
    [SerializeField] GameEngine ge;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House") || other.CompareTag("Restaurant")||other.CompareTag("GasStation"))
        {
            ge.SpawnInteractableObject();
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")) ge.SpawnEnemy();
        Destroy(other.gameObject);
    }
}
