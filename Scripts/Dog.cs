using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Dog : MonoBehaviour
{
    Vector2 lastVelocity;

    private Rigidbody2D rb;
    public Dogstats Dogstats;

    [Range(0, 10)] public double Speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Assets.Scripts.DogVectors.GetRandomVector2((float)Speed);
    }

    void Save()
    {
        var savePath = Path.Combine(Application.dataPath, $"{name}.txt");
        var sb = new StringBuilder();
        sb.AppendLine($"Speed: {Speed}");
        File.WriteAllText(savePath, sb.ToString());
        //JsonSerializer serializer = new JsonSerializer();

        //using (StreamWriter sw = new StreamWriter(savePath))
        //using (JsonWriter writer = new JsonTextWriter(sw))
        //{
        //    serializer.Serialize(writer, this);
        //}

    }

    DateTime _lastSaved = DateTime.MinValue;
    const int SaveIntervalInSeconds = 5;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_lastSaved.AddSeconds(SaveIntervalInSeconds) <= DateTime.Now)
        {
            Save();
            _lastSaved = DateTime.Now;
        }
        var rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.velocity;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        //  var direction = Vector2.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
        var collideNormal = coll.GetContact(0).normal;
        var rb = GetComponent<Rigidbody2D>();
        var speed = lastVelocity.magnitude;
        var direction = DogVectors.GetRandomVector2RestrictByNormal((float)Speed, collideNormal);
        rb.velocity = direction;
    }
}
