using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingVelocity : MonoBehaviour
{

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Assets.Scripts.DogVectors.GetRandomVector2(13f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

}
