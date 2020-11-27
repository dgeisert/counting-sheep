using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private Boid boid;
    [SerializeField] private int count;
    [SerializeField] private Vector2 range;
    private List<Boid> boids;

    private Vector3 center;

    [SerializeField] private float speed = 1;
    [SerializeField] private float maxSpeed = 1;

    [SerializeField] private float avoidRange = 1;
    [SerializeField] private float visualRange = 1;
    [SerializeField] private float avoidTurn = 1;
    [SerializeField] private float centerTurn = 1;

    [SerializeField] private float avoidDogRange = 1;
    [SerializeField] private float avoidBarkRange = 1;
    [SerializeField] private float dogModifier = 1;

    void Start()
    {
        boids = new List<Boid>();
        for (int i = 0; i < count; i++)
        {
            Boid b = Instantiate(boid, new Vector3((Random.value - 0.5f) * range.x, 0, (Random.value - 0.5f) * range.y), Quaternion.Euler(0, Random.value * 360, 0), transform);
            b.dx = Random.value;
            b.dz = Random.value;
            b.speed = speed;
            b.maxSpeed = maxSpeed;
            boids.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var b in boids)
        {
            if (Vector3.Distance(Dog.xyz, b.xyz) < avoidDogRange)
            {
                b.dx -= (Dog.xyz.x - b.x) * avoidTurn * dogModifier;
                b.dz -= (Dog.xyz.z - b.z) * avoidTurn * dogModifier;
            }
            float i = 0;
            float moveX = 0;
            float moveZ = 0;
            b.visualNearbyCenter = Vector3.zero;
            foreach (var b2 in boids)
            {
                float dist = Vector3.Distance(b.xyz, b2.xyz);
                if (dist < visualRange)
                {
                    i++;
                    b.visualNearbyCenter += b2.transform.position;
                }
                if (dist < avoidRange)
                {
                    moveX += b.x - b2.x;
                    moveZ += b.z - b2.z;
                }
            }
            if (i > 0)
            {
                b.visualNearbyCenter /= i;
                b.dx += (b.visualNearbyCenter.x - b.x) * centerTurn;
                b.dz += (b.visualNearbyCenter.z - b.z) * centerTurn;
            }
            b.dx += moveX * avoidTurn;
            b.dz += moveZ * avoidTurn;
            b.Move();
        }
    }
}