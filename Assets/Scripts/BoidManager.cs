﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager sheepFlock;
    [SerializeField] private Boid boid;
    public List<Boid> boids;

    private Vector3 center;

    [SerializeField] protected List<BoidAvoid> boidAvoids;

    [SerializeField] protected float speed = 1;
    [SerializeField] protected float maxSpeed = 1;

    [SerializeField] private float avoidRange = 1;
    [SerializeField] private float visualRange = 1;
    [SerializeField] private float avoidTurn = 1;
    [SerializeField] private float centerTurn = 1;
    [SerializeField] private float globalCenterBias = 1;
    [SerializeField] private bool isSheepFlock;

    void Awake()
    {
        if (isSheepFlock)
        {
            sheepFlock = this;
        }
    }

    void Start()
    {
        boids = new List<Boid>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Boid b = transform.GetChild(i).GetComponent<Boid>();
            b.dx = Random.value;
            b.dz = Random.value;
            b.speed = speed;
            b.maxSpeed = maxSpeed;
            boids.Add(b);
        }
        /*
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
        */
    }

    void Update()
    {
        foreach (var b in boids)
        {
            //handle avoids
            foreach (var ba in boidAvoids)
            {
                if (ba.gameObject.activeSelf)
                {
                    b.Avoid(ba.transform.position, ba.avoiderRange, avoidTurn, ba.avoiderModifier);
                }
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