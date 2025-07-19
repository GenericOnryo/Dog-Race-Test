using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandVec : MonoBehaviour
{
    public float targetMagnitude = 20f; // how fast you want the thing to go?
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randDirect = Random.insideUnitCircle;

        Vector2 randDirectnormalized = randDirect.normalized;

        Vector2 DirectRandFin = randDirectnormalized * targetMagnitude;
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
