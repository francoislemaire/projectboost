﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    

    //todo remove from inspector later
    [Range(0,1)]
    [SerializeField]
    float movementFactor; //0 for not moved, 1 for fully moved.

    Vector3 startingPos; //stored for absolute movement
   
    
    void Start()
    {
        startingPos = transform.position;
      
    }

    void Update()
    {
        if (period != 0)
        {
            
            MoveObject();
        }
    }

    private void MoveObject()
    {

        float cycles = Time.time / period;  //grows continually from 0

        const float tau = Mathf.PI * 2; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}