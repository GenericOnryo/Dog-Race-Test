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

    [Range(-10, 10)] public double Size;
    [Range(0, 10)] public double Speed;
    [Range(0, 10)] public double Friendliness;
    [Range(0, 10)] public double Aggression;
    [Range(0, 10)] public double Intelligence;
    [Range(-2, 2)] public double Mood;


    private double Acceleration;
    private double accelerSize;
    private double accelerSpeed;

    private double maxSpeed;
    public double speedCurrent;
    private float elapsedTimeTraveling;
    public float duration = 2f;
    private float startTime;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Assets.Scripts.DogVectors.GetRandomVector2((float)1);


        if (Size < -5)
            accelerSize = (0.01 * -Size) - 0.05;
        else
            accelerSize = 0;
        if (Speed < 5)
            accelerSpeed = 0.02 * (Speed - 5);
        else
            accelerSpeed = 0;
        Acceleration = (0.01 * Mood) + accelerSize + accelerSpeed;

        if (Size < 0)
            maxSpeed = 10 + Mood + (Speed * (-0.1 * Size));
        else if (Size > 0)
            maxSpeed = 10 + Mood + (Speed * ((0.1 * Size) + (Size * Acceleration)));
        else
            maxSpeed = 10 + Mood;


    }

    void Save()
    {
        var savePath = Path.Combine(Application.dataPath, $"{name}.txt");
        var sb = new StringBuilder();
        sb.AppendLine($"SPED: {Speed}");
        if (Size < 0)
            sb.AppendLine($"TINY: {-Size}");
        else
            sb.AppendLine($"BULK: {Size}"); ;
        sb.AppendLine($"FREN: {Friendliness}");
        sb.AppendLine($"AGRO: {Aggression}");
        sb.AppendLine($"SMRT: {Intelligence}");
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
        //if (_lastSaved.AddSeconds(SaveIntervalInSeconds) <= DateTime.Now)
        //{
        //    Save();
        //    _lastSaved = DateTime.Now;
        //}
        var rb = GetComponent<Rigidbody2D>();
        lastVelocity = rb.velocity;

        elapsedTime = Time.time - startTime;

        elapsedTimeTraveling = elapsedTime;

        double speedNumerator = (maxSpeed + (0.1 * elapsedTimeTraveling));
        if (Size < -5)
        {
            speedCurrent = speedNumerator / (1 + Math.Exp(-(((Math.Abs(Size) - 5) * Acceleration) + 0.1) * (elapsedTimeTraveling - 20 + (10 * ((10 * Acceleration) + (0.1 * Math.Abs(Size)))) - Math.Abs(Speed - 4))));
        }
        else if (Speed > 5)
        {
            speedCurrent = speedNumerator / (1 + Math.Exp(-(Acceleration + 0.1) * (elapsedTimeTraveling - 20 + (10 * (10 * Acceleration)))));
        }
        else
        {
            speedCurrent = speedNumerator / (1 + Math.Exp(-0.1 * (elapsedTimeTraveling - 20)));
        }


    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        //  var direction = Vector2.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
        var collideNormal = coll.GetContact(0).normal;
        var rb = GetComponent<Rigidbody2D>();
        //var speed = lastVelocity.magnitude;
        var direction = DogVectors.GetRandomVector2RestrictByNormal((float)speedCurrent, collideNormal);
        rb.velocity = direction;

        startTime += (0.1f * elapsedTime);



    }
}
