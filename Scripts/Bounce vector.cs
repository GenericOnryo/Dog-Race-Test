using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Bouncevector : MonoBehaviour
{
    Vector2 lastVelocity;

    // Start is called before the first frame update
    public void Awake()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        var rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.velocity;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    { 
        //  var direction = Vector2.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
        var collideNormal = coll.GetContact(0).normal;
        var rb = GetComponent<Rigidbody2D>();
        var speed = lastVelocity.magnitude;
        var direction = DogVectors.GetRandomVector2RestrictByNormal(13f, collideNormal);
        rb.velocity = direction;
    }
}
