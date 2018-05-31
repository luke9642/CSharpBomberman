using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    public bool     aboutToDestroy;
    public int      firePower;

    private float   secToBoom;
    private Player  player;
    private float   resizer;

    void Awake() {
        aboutToDestroy  = false;
        secToBoom       = 3.0f;
        resizer         = 0f;
        firePower       = 0;
    }

    void Update() {
        secToBoom   -= Time.deltaTime;
        resizer     += Time.deltaTime;

        if (resizer >= 0.5f)
            Resize();

        if (secToBoom <= 0f && !aboutToDestroy)
            StartCoroutine(Boom());
    }

    IEnumerator Boom() {
        player.ReturnOneBomb();
        aboutToDestroy = true;
        BombChecker();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    void BombChecker() {
        float distance = 1.25f + firePower;
        int layer = 1 << 8;
        ShotRaycast(Vector3.forward, distance, layer);
        ShotRaycast(Vector3.back, distance, layer);
        ShotRaycast(Vector3.left, distance, layer);
        ShotRaycast(Vector3.right, distance, layer);
    }

    void ShotRaycast(Vector3 target, float distance, int layer) {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, target, out hit, distance, layer)) {
            if (hit.collider != null) {
                if (hit.collider.tag == Tags.softWall) {
                    StartCoroutine(hit.collider.gameObject.GetComponent<SayBeforeDestroy>().Destroyer());
                }
            }
        }
    }


    void Resize() {
        resizer = 0f;

        if (transform.localScale.x == 0.7f)
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        else
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void SetUpOwner(GameObject go, int firePower) {
        player = go.GetComponent<Player>();
        this.firePower = firePower;
    }
}
