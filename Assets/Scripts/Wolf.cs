using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    public static Wolf Instance;

    private List<Boid> boids;

    private Vector3 center;

    [SerializeField] protected List<BoidAvoid> boidAvoids;

    [SerializeField] protected float speed = 1;
    [SerializeField] protected float maxSpeed = 1;

    [SerializeField] private float avoidRange = 1;
    [SerializeField] private float visualRange = 1;
    [SerializeField] private float avoidTurn = 1;
    [SerializeField] private float centerTurn = 1;
    [SerializeField] private float globalCenterBias = 1;
    [SerializeField] private float mapPushVal = 0.4f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        boidAvoids.Add(Dog.Instance.dogAvoider);
        //boidAvoids.Add(Dog.Instance.barkAvoider);

        boids = new List<Boid>();
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
            foreach (var b2 in BoidManager.sheepFlock.boids)
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
            if (i != visualRange)
            {
                b.dx += (b.visualNearbyCenter.x - b.x) * centerTurn;
                b.dz += (b.visualNearbyCenter.z - b.z) * centerTurn;
            }
            else
            {
                //global center bias
                b.dx += (b.startPos.x - b.x) * centerTurn / 2;
                b.dz += (b.startPos.z - b.z) * centerTurn / 2;
            }

            //Apply map push values
            Vector2 mapPush = IslandBuilder.Instance.GetPush(b.xyz);
            b.dx += mapPushVal * mapPush.x;
            b.dz += mapPushVal * mapPush.y;

            //Apply avoid of other sheep
            b.dx += moveX * avoidTurn;
            b.dz += moveZ * avoidTurn;
            b.Move();
        }
    }

    public void AddWolf(GameObject go)
    {
        go.transform.SetParent(transform);
        Boid b = go.GetComponent<Boid>();
        b.dx = (Random.value - 0.5f) * 10;
        b.dz = (Random.value - 0.5f) * 10;
        b.speed = speed;
        b.maxSpeed = maxSpeed;
        boids.Add(b);
    }
}