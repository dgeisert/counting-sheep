using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    BoidManager targetFlock;

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

    void Start()
    {
        boidAvoids.Add(Dog.Instance.dogAvoider);
        boidAvoids.Add(Dog.Instance.barkAvoider);

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

        targetFlock = BoidManager.sheepFlock;
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
                    b.Avoid(ba.transform.position, ba.avoiderRange * 2, avoidTurn, ba.avoiderModifier * 10);
                }
            }

            float i = visualRange;
            float moveX = 0;
            float moveZ = 0;
            Boid target = null;
            b.visualNearbyCenter = Vector3.zero;
            foreach (var b2 in targetFlock.boids)
            {
                float dist = Vector3.Distance(b.xyz, b2.xyz);
                if (dist < i)
                {
                    i = dist;
                    b.visualNearbyCenter = b2.transform.position;
                    target = b2;
                }
            }
            if (i < 1)
            {
                target.Die();
            }
            //if there are sheep in range move towards the center of them
            if (i > 0)
            {
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