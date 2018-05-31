using UnityEngine;
using System.Collections;

public class SayBeforeDestroy : MonoBehaviour
{
    public bool aboutToDestroy;

    void Awake()
    {
        aboutToDestroy = false;
    }

    public IEnumerator Destroyer()
    {
        aboutToDestroy = true;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
