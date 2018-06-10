using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SayBeforeDestroy : MonoBehaviour
{
    public bool aboutToDestroy;
    [SerializeField] [Range(0,1)] float powerUpDropChance = 0.3f;
    [SerializeField] GameObject[] powerUps;

    void Awake()
    {
        aboutToDestroy = false;
    }

    public IEnumerator Destroyer()
    {
        aboutToDestroy = true;
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
        TryDroppingPickup();
    }

    private void TryDroppingPickup()
    {
        bool dropPowerUp = (powerUpDropChance >= Random.Range(0, 1f));
        if(dropPowerUp)
        {
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], transform.position, Quaternion.identity);
        }
    }
}