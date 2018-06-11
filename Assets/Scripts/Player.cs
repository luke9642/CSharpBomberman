using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int playerNumber;

    [Header("Stats:")]
    [SerializeField] public float playerSpeed;
    [SerializeField] public int playerFirePower;
    [SerializeField] public int playerBombsLimit;

    [Header("Triggers:")]
    public ColisionTrigger colTriUp;
    public ColisionTrigger colTriDown;
    public ColisionTrigger colTriLeft;
    public ColisionTrigger colTriRight;

    [Header("Assets:")]
    public GameObject bombermanModel;
    public GameObject bombermanTexture;
    public GameObject bomb;

    private MainController mainController;
    private HashIDs hashIDs;
    private bool moving;
    private bool dying;
    private Vector3 target;
    private int bombsOnMap;
    private Animator animator;
    public event EventHandler PlayerDied;

    void Awake()
    {
        moving = false;
        dying = false;
        mainController = GameObject.FindGameObjectWithTag(Tags.mainController).GetComponent<MainController>();
        bombsOnMap = 0;
        playerBombsLimit = 1;
        animator = bombermanTexture.GetComponent<Animator>();
        hashIDs = GameObject.FindGameObjectWithTag(Tags.mainController).GetComponent<HashIDs>();
    }

    void Update()
    {
        if (!moving && !dying)
            WaitForMove();
        else
            Moving();
        WaitForBomb();
    }
    void WaitForMove()
    {
        if (Input.GetKey(mainController.playersKeys[playerNumber].up))
        {
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            TryPushingBomb(colTriUp);
            TryMovingPlayer(colTriUp);
        }

        if (Input.GetKey(mainController.playersKeys[playerNumber].down))
        {
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            TryPushingBomb(colTriDown);
            TryMovingPlayer(colTriDown);
        }

        if (Input.GetKey(mainController.playersKeys[playerNumber].left))
        {
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
            TryPushingBomb(colTriLeft);
            TryMovingPlayer(colTriLeft);
        }

        if (Input.GetKey(mainController.playersKeys[playerNumber].right))
        {
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            TryPushingBomb(colTriRight);
            TryMovingPlayer(colTriRight);
        }
    }

    private void TryPushingBomb(ColisionTrigger trigger)
    {
        if (trigger.bombInside != null)
        {
            trigger.bombInside.TryPushing(trigger.transform.localPosition);
        }
    }

    private void TryMovingPlayer(ColisionTrigger trigger)
    {
        if (!trigger.TouchesSomething())
        {
            moving = true;
            target = transform.position + trigger.transform.localPosition;
            animator.SetBool(hashIDs.walkingBool, true);
        }
    }

    void Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);

        if (transform.position == target)
        {
            moving = false;
            StartCoroutine(StopMovingAnimation());
        }
    }

    IEnumerator StopMovingAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        if (!moving)
            animator.SetBool(hashIDs.walkingBool, false);
    }

    void WaitForBomb()
    {
        if (moving) return;
        if (Input.GetKeyDown(mainController.playersKeys[playerNumber].putBomb) && bombsOnMap < playerBombsLimit)
        {
            GameObject bombObj = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
            bombObj.GetComponent<Bomb>().SetUpOwner(gameObject, playerFirePower);
            ++bombsOnMap;
        }
    }

    public void Destroyer()
    {
        dying = true;
        animator.SetBool(hashIDs.dyingBool, true);
        OnPlayerDeath(new EventArgs());
        Destroy(gameObject, 1.75f);
    }

    protected virtual void OnPlayerDeath(EventArgs e)
    {
        if (PlayerDied != null)
            PlayerDied(this, e);
    }

    public void ReturnOneBomb()
    {
        --bombsOnMap;
    }
}