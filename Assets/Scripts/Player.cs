using UnityEngine;
using System.Collections;

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
    private Vector3 target;
    private int bombsOnMap;
    private Animator animator;


    void Awake()
    {
        moving = false;
        mainController = GameObject.FindGameObjectWithTag(Tags.mainController).GetComponent<MainController>();
        bombsOnMap = 0;
        playerBombsLimit = 1;
        animator = bombermanTexture.GetComponent<Animator>();
        hashIDs = GameObject.FindGameObjectWithTag(Tags.mainController).GetComponent<HashIDs>();
    }

    void Update()
    {
        if (!moving)
            WaitForMove();
        else
            Moving();

        WaitForBomb();
    }

    void WaitForMove()
    {
        if (!colTriUp.bombTouch && !colTriUp.softWallTouch && !colTriUp.playerTouch && !colTriUp.hardWallTouch &&
            Input.GetKey(mainController.playersKeys[playerNumber].up))
        {
            moving = true;
            target = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            animator.SetBool(hashIDs.walkingBool, true);
        }

        if (!colTriDown.bombTouch && !colTriDown.softWallTouch && !colTriDown.playerTouch &&
            !colTriDown.hardWallTouch && Input.GetKey(mainController.playersKeys[playerNumber].down))
        {
            moving = true;
            target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            animator.SetBool(hashIDs.walkingBool, true);
        }

        if (!colTriLeft.bombTouch && !colTriLeft.softWallTouch && !colTriLeft.playerTouch &&
            !colTriLeft.hardWallTouch && Input.GetKey(mainController.playersKeys[playerNumber].left))
        {
            moving = true;
            target = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
            animator.SetBool(hashIDs.walkingBool, true);
        }

        if (!colTriRight.bombTouch && !colTriRight.softWallTouch && !colTriRight.playerTouch &&
            !colTriRight.hardWallTouch && Input.GetKey(mainController.playersKeys[playerNumber].right))
        {
            moving = true;
            target = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            bombermanTexture.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
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
        if (!moving && Input.GetKeyDown(mainController.playersKeys[playerNumber].putBomb) && bombsOnMap < playerBombsLimit)
        {
            GameObject bombObj = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;
            bombObj.GetComponent<Bomb>().SetUpOwner(gameObject, playerFirePower);
            ++bombsOnMap;
        }
    }

    public void ReturnOneBomb()
    {
        --bombsOnMap;
    }
}