using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    public bool aboutToDestroy;
    [SerializeField] float pushSpeed = 1.5f;
    [SerializeField] GameObject explosion;

    [Header("Triggers:")]
    public ColisionTrigger colTriUp;
    public ColisionTrigger colTriDown;
    public ColisionTrigger colTriLeft;
    public ColisionTrigger colTriRight;

    private Player player;
    private int firePower;
    private float secToBoom;
    private float resizer;
    private bool isMoving;
    private Vector3 target;
    private Dictionary<Vector3Int, ColisionTrigger> fromDirectionToTrigger;

    public void SetUpOwner(GameObject go, int firePower)
    {
        player = go.GetComponent<Player>();
        this.firePower = firePower;
    }

    public void TryPushing(Vector3 direction)
    {
        var trigger = fromDirectionToTrigger[Vector3Int.RoundToInt(direction)];
        if (!trigger.TouchesSomething() && !aboutToDestroy && !isMoving)
        {
            target = transform.position + direction;
            isMoving = true;
        }
    }

    void Awake()
    {
        aboutToDestroy = false;
        secToBoom = 3.0f;
        resizer = 0f;
        fromDirectionToTrigger = new Dictionary<Vector3Int, ColisionTrigger>()
        {
            { new Vector3Int(0,0,1) , colTriUp },
            { new Vector3Int(0,0,-1) , colTriDown },
            { Vector3Int.left , colTriLeft },
            { Vector3Int.right , colTriRight }
        };
    }

    void Update()
    {
        if(isMoving)
        {
            Move();
        }
        secToBoom -= Time.deltaTime;
        resizer += Time.deltaTime;

        if (resizer >= 0.5f)
            Resize();
        if (secToBoom <= 0f && !aboutToDestroy && !isMoving)
            StartCoroutine(Boom());
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, pushSpeed * Time.deltaTime);
        if (transform.position == target)
        {
            isMoving = false;
        }
    }

    private IEnumerator Boom()
    {
        player.ReturnOneBomb();
        aboutToDestroy = true;
        Explode();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    void Explode()
    {
        float explosionRange = 1f + firePower;
        int layer = (1 << 8) + (1 << 9); //Wall and explodable layers
        var origin = transform.position;
        ExplodeInDirection(origin + Vector3.up, Vector3.down, explosionRange, layer); //Explode from above to hit player inside the bomb
        ExplodeInDirection(origin, Vector3.forward, explosionRange, layer);
        ExplodeInDirection(origin, Vector3.back, explosionRange, layer);
        ExplodeInDirection(origin, Vector3.left, explosionRange, layer);
        ExplodeInDirection(origin, Vector3.right, explosionRange, layer);
        Instantiate(explosion, origin, Quaternion.identity);
    }

    void ExplodeInDirection(Vector3 origin, Vector3 target, float explosionRange, int layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, target, out hit, explosionRange, layer))
        {
            var hitObject = hit.collider.gameObject;

            var distanceToHitObject = Vector3.Distance(origin, hitObject.transform.position);
            SpawnExplosionsInDirection(target, distanceToHitObject);

            if (hit.collider.CompareTag(Tags.player))
            {
                hitObject.GetComponent<Player>().Destroyer();
            }

            if (hit.collider.CompareTag(Tags.softWall))
            {
                StartCoroutine(hitObject.GetComponent<SayBeforeDestroy>().Destroyer());
            }
        }
        else
        {
            SpawnExplosionsInDirection(target, explosionRange);
        }
    }

    private void SpawnExplosionsInDirection(Vector3 target, float distance)
    {
        for (int i = 1; i <= (int)Math.Round(distance); i++)
        {
            Instantiate(explosion, transform.position + (target * i), Quaternion.identity);
        }
    }

    void Resize()
    {
        resizer = 0f;

        if (transform.localScale.x == 0.7f)
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        else
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }
}