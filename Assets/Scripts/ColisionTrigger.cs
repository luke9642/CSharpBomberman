using System;
using UnityEngine;
using System.Collections;

public class ColisionTrigger : MonoBehaviour
{
    public bool hardWallTouch;
    public bool playerTouch;
    public bool softWallTouch;
    public bool bombTouch;

    void Awake()
    {
        hardWallTouch = false;
        playerTouch = false;
        softWallTouch = false;
        bombTouch = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.hardWall))
            hardWallTouch = true;

        if (other.CompareTag(Tags.player))
            playerTouch = true;

        if (other.CompareTag(Tags.softWall))
        {
            softWallTouch = true;

            if (other.gameObject.GetComponent<SayBeforeDestroy>().aboutToDestroy)
                softWallTouch = false;
        }

        if (other.CompareTag(Tags.bomb))
        {
            bombTouch = true;

            if (other.gameObject.GetComponent<Bomb>().aboutToDestroy)
                bombTouch = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.hardWall))
            hardWallTouch = false;

        if (other.CompareTag(Tags.player))
            playerTouch = false;

        if (other.CompareTag(Tags.softWall))
            softWallTouch = false;

        if (other.CompareTag(Tags.bomb))
            bombTouch = false;
    }
}