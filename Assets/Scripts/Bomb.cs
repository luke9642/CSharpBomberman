using UnityEngine;
using System.Collections;
using System;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    public bool aboutToDestroy;
    private int firePower;
    private float secToBoom;
    private Player player;
    private float resizer;

    void Awake()
    {
        aboutToDestroy = false;
        secToBoom = 3.0f;
        resizer = 0f;
    }

    void Update()
    {
        secToBoom -= Time.deltaTime;
        resizer += Time.deltaTime;

        if (resizer >= 0.5f)
            Resize();

        if (secToBoom <= 0f && !aboutToDestroy)
            StartCoroutine(Boom());
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
        int layer = 1 << 8;
        // What are up and down rays for?
        //ShotRaycast(new Vector3(0, 1, 0), distance, layer);
        //ShotRaycast(new Vector3(0, -1, 0), distance, layer);
        ExplodeInDirection(new Vector3(0, 0, 0), explosionRange, layer);
        ExplodeInDirection(Vector3.forward, explosionRange, layer);
        ExplodeInDirection(Vector3.back, explosionRange, layer);
        ExplodeInDirection(Vector3.left, explosionRange, layer);
        ExplodeInDirection(Vector3.right, explosionRange, layer);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    void ExplodeInDirection(Vector3 target, float explosionRange, int layer)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, target, out hit, explosionRange))
        {
            var hitObject = hit.collider.gameObject;

            var distanceToHitObject = Vector3.Distance(transform.position, hitObject.transform.position);
            SpawnExplosionsInDirection(target, distanceToHitObject);

            if (hit.collider.CompareTag(Tags.player))
            {
                Destroy(hitObject); //Is coroutine needed?
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

    public void SetUpOwner(GameObject go, int firePower)
    {
        player = go.GetComponent<Player>();
        this.firePower = firePower;
    }
}