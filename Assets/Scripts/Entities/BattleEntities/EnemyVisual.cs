using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisual : MonoBehaviour
{
    public Text EnemyName;
    public Text HealthIndicator;
    public Slider HealthSlider;

    public Enemy enemy = null;

    private Vector2 center;
    private float radius = 200f;

    private Vector2 velocity;
    private Vector2 acceleration;
    private float accelerationChangeInterval = 1.5f; // seconds
    private float timer = 0f;

    //private static int timeDelta = 20;

    void Start()
    {
        // Set the center to the starting position
        center = transform.position;

        // Initial random velocity and acceleration
        velocity = Vector2.zero;
        acceleration = new Vector2(
            UnityEngine.Random.Range(-20f, 20f),
            UnityEngine.Random.Range(-20f, 20f)
        );
    }

    void Update()
    {
        if (enemy != null)
        {
            // Keep this UI part exactly as requested
            EnemyName.text = enemy.getName();
            HealthSlider.maxValue = enemy.maxhealth;
            HealthSlider.value = enemy.health;
            HealthIndicator.text = enemy.health + " / " + enemy.maxhealth;

            if (enemy.health <= 0)
            {
                // Set a fixed upward velocity and move upwards
                transform.position += Vector3.up * 100f * Time.deltaTime;
                return; // Skip the rest of the movement logic
            }

            // Normal movement logic
            velocity += acceleration * Time.deltaTime;
            Vector2 newPos = (Vector2)transform.position + velocity * Time.deltaTime;

            // Keep within radius
            if (Vector2.Distance(newPos, center) > radius)
            {
                Vector2 fromCenter = (Vector2)transform.position - center;
                Vector2 normal = fromCenter.normalized;
                velocity = Vector2.Reflect(velocity, normal);
                velocity *= 0.9f;
                newPos = (Vector2)transform.position;
            }

            transform.position = newPos;

            // Change acceleration periodically
            timer += Time.deltaTime;
            if (timer >= accelerationChangeInterval)
            {
                acceleration = new Vector2(
                    UnityEngine.Random.Range(-20f, 20f),
                    UnityEngine.Random.Range(-20f, 20f)
                );
                timer = 0f;
            }
        }
    }
}
