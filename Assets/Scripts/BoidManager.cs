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
    [SerializeField] private float globalCenterBias = 1;

    [SerializeField] private float avoidDogRange = 1;
    [SerializeField] private float avoidBarkRange = 1;
    [SerializeField] private float dogModifier = 1;
    [SerializeField] private float barkModifier = 1;

    void Start()
    {
        //init boid sheep, probably need to have sheep predetermined in scenes
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

    void Update()
    {
        foreach (var b in boids)
        {
            //if dog is near move away
            if (Vector3.Distance(Dog.xyz, b.xyz) < avoidDogRange)
            {
                b.dx -= (Dog.xyz.x - b.x) * avoidTurn * dogModifier;
                b.dz -= (Dog.xyz.z - b.z) * avoidTurn * dogModifier;
            }
            //when dog barks move away
            if (Controls.Bark && Vector3.Distance(Dog.xyz, b.xyz) < avoidBarkRange)
            {
                b.dx -= (Dog.xyz.x - b.x) * avoidTurn * barkModifier;
                b.dz -= (Dog.xyz.z - b.z) * avoidTurn * barkModifier;
            }

            float i = 0;
            float moveX = 0;
            float moveZ = 0;
            b.visualNearbyCenter = Vector3.zero;
            foreach (var b2 in boids)
            {
                float dist = Vector3.Distance(b.xyz, b2.xyz);
                //if sheep is nearby add it to the group you are moving with
                if (dist < visualRange)
                {
                    i++;
                    b.visualNearbyCenter += b2.transform.position;
                }
                //if sheep is very close move away from it
                if (dist < avoidRange)
                {
                    moveX += b.x - b2.x;
                    moveZ += b.z - b2.z;
                }
            }
            //if there are sheep in range move towards the center of them
            if (i > 0)
            {
                b.visualNearbyCenter /= i;
                b.dx += (b.visualNearbyCenter.x - b.x) * centerTurn;
                b.dz += (b.visualNearbyCenter.z - b.z) * centerTurn;
            }
            //global center bias
            b.dx += -b.x * globalCenterBias;
            b.dz += -b.x * globalCenterBias;

            //Apply avoid of other sheep
            b.dx += moveX * avoidTurn;
            b.dz += moveZ * avoidTurn;
            b.Move();
        }
    }
}