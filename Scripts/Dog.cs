using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Dog : MonoBehaviour
{
    Vector2 lastVelocity;

    private Rigidbody2D rb;
    public ScriptableObject Dogstats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Assets.Scripts.DogVectors.GetRandomVector2(13f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
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
