using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private GameEngine _ge;

    private void Start()
    {
        _ge = FindObjectOfType<GameEngine>();
    }

    private void Update()
    {
        transform.position+=new Vector3(-_ge.GetSpeed()*Time.deltaTime*4f,0,0);
    }
}
