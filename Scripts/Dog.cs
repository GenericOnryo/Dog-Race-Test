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

    public float scale;

    [Range(0, 10)] public double Speed;

    private double maxSpeed;
    public double speedCurrent;

    [Range(0, 10)] public double Friendliness;
    [Range(0, 10)] public double Aggression;
    [Range(0, 10)] public double Intelligence;
    [Range(-2, 2)] public double Mood;

    private double speedCap;
    private double sizeCap;
    private double friendCap;
    private double agroCap;
    private double friendtoaggrorestrict;
    private double sizetospeedrestrict;

    private double Acceleration;
    private double accelerSize;
    private double accelerSpeed;

    private float elapsedTimeTraveling;
    private float startTime;
    private float elapsedTime;


    // Start is called before the first frame update
    void Start()
    {
        // Mutual-Exlcusion for stats.
        friendCap = Aggression - 5;
        agroCap = Friendliness - 5;
        speedCap = Math.Abs(Size) - 5;
        sizeCap = Speed - 5;
        friendtoaggrorestrict = 15 - (Friendliness + Aggression);
        sizetospeedrestrict = 15 - (Math.Abs(Size) + Speed);

        if (Friendliness + Aggression > 15 && friendCap > agroCap)
        {
            Friendliness += friendtoaggrorestrict;
        }
        else if (Friendliness + Aggression > 15 && friendCap < agroCap)
        {
            Aggression += friendtoaggrorestrict;
        }

        if (Math.Abs(Size) + Speed > 15 && speedCap > sizeCap)
        {
            Size += sizetospeedrestrict;
        }
        else if (Math.Abs(Size) + Speed > 15 && speedCap < sizeCap)
        {
            Speed += sizetospeedrestrict;
        }

        // get base time and get rigidbody, move in random direction.
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Assets.Scripts.DogVectors.GetRandomVector2((float)1);

        // Scale Character.
        if (Size < -5)
        {
            scale = (0.04f * (float)Size) + 1f;
        }
        else if (Size > 5 ) 
        {
            scale = (0.04f * (float)Size) + 1f;
        }
        else
        {
            scale = (0.02f * (float)Size) + 1f;
        }

        Vector2 currentCharacterScale = transform.localScale;
        transform.localScale = currentCharacterScale * scale;

        // Mood and Stat acceleration Bonuses and Max Speed.
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

        // Accelerating Speed over Time for Character.
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
            // Apply and update current vector speed(Magnitude) in realtime.
        rb.velocity = lastVelocity.normalized * (float)speedCurrent;

    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        // On a Standard impact, bounce in random direction away from impacted object.
        //  var direction = Vector2.Reflect(lastVelocity.normalized, coll.contacts[0].normal);
        var collideNormal = coll.GetContact(0).normal;
        var rb = GetComponent<Rigidbody2D>();
        //var speed = lastVelocity.magnitude;
        var direction = DogVectors.GetRandomVector2RestrictByNormal((float)speedCurrent, collideNormal);
        rb.velocity = direction;

        // Reduce current acceleraton time by a %
        startTime += (0.3f * elapsedTime);



    }
}
