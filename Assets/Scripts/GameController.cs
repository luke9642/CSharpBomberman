using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public Material[] bombermanMaterials;
    public int lvlSize;
    public int playersNumber;
    public GameObject hardWall;
    public GameObject floor;
    public GameObject player;
    public GameObject softWall;
    public GameObject gameOverPanel;
    public Text gameOverText;
    
    [Header("Runtime objects:")]
    [SerializeField] Transform runtimePlayers;
    [SerializeField] Transform runtimeSoftWalls;
    [SerializeField] Transform runtimeHardWalls;
    
    private List<Vector3> softWallPositions;
    private List<Player> alivePlayers;

    private void Awake()
    {
        softWallPositions = new List<Vector3>();
        alivePlayers = new List<Player>();
        gameOverPanel.SetActive(false);
    }

    void Start()
    {
        CreateLvl();
        SpawnPlayers();
        FillSoftWallsList();
        PutSoftWalls();
    }

    void CreateLvl()
    {
        for (var i = 0; i < lvlSize; ++i)
        {
            Instantiate(hardWall, new Vector3(14f + i, 0f, 6f), Quaternion.identity).transform.parent = runtimeHardWalls;
            Instantiate(hardWall, new Vector3(14f + i, 0f, -6f), Quaternion.identity).transform.parent = runtimeHardWalls;
            Instantiate(floor, new Vector3(14f + i, -0.5f, 0f), Quaternion.identity);

            if (i % 2 == 0)
                for (var z = 4; z > -5; z -= 2)
                    Instantiate(hardWall, new Vector3(14f + i, 0f, z), Quaternion.identity).transform.parent = runtimeHardWalls;
        }

        float x = lvlSize + 14;

        for (var i = 0; i < 13; ++i)
        {
            Instantiate(hardWall, new Vector3(x, 0f, 6f - i), Quaternion.identity).transform.parent = runtimeHardWalls;
        }
    }

    void SpawnPlayers()
    {
        if (playersNumber >= 1)
        {
            var playerObj = Instantiate(player, new Vector3(1f, 0f, 5f), Quaternion.identity);
            InitializePlayer(playerObj, 0);
        }

        if (playersNumber >= 2)
        {
            var playerObj = Instantiate(player, new Vector3(1f, 0f, -5f), Quaternion.identity);
            InitializePlayer(playerObj, 1);
        }

        if (playersNumber >= 3)
        {
            var playerObj = Instantiate(player, new Vector3(13f + lvlSize, 0f, 5f), Quaternion.identity);
            InitializePlayer(playerObj, 2);
        }

        if (playersNumber >= 4)
        {
            var playerObj = Instantiate(player, new Vector3(13f + lvlSize, 0f, -5f), Quaternion.identity);
            InitializePlayer(playerObj, 3);
        }
    }

    private void InitializePlayer(GameObject playerObj, int playerIndex)
    {
        playerObj.transform.parent = runtimePlayers;
        playerObj.GetComponent<Player>().bombermanModel.GetComponent<SkinnedMeshRenderer>().material =
            bombermanMaterials[playerIndex];

        var player = playerObj.GetComponent<Player>();
        player.playerNumber = playerIndex;
        player.PlayerDied += OnPlayerDeath;
        alivePlayers.Add(player);
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        var player = sender as Player;
        print("Player " + (player.playerNumber+1) + " died.");
        alivePlayers.Remove(player);
        CheckForWinner();
    }

    private void CheckForWinner()
    {
        if (alivePlayers.Count == 1)
        {
            StartCoroutine(LoadLevel(0, 5f));
            var playerNo = alivePlayers.Single().playerNumber + 1;
            var info = "Game Over! Player " + playerNo + " wins!";
            gameOverText.text = info;
            gameOverPanel.SetActive(true);
        }
    }

    IEnumerator LoadLevel(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }

    void FillSoftWallsList()
    {
        if (playersNumber >= 2)
        {
            for (var i = 3; i >= -3; --i)
                softWallPositions.Add(new Vector3(1f, 0f, i));

            for (var i = 3; i >= -3; i -= 2)
                softWallPositions.Add(new Vector3(2f, 0f, i));
        }
        else
        {
            for (var i = 3; i >= -5; --i)
                softWallPositions.Add(new Vector3(1f, 0f, i));

            for (var i = 3; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(2f, 0f, i));
        }

        if (playersNumber == 4)
        {
            for (var i = 3; i >= -3; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (var i = 3; i >= -3; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }
        else if (playersNumber == 3)
        {
            for (var i = 3; i >= -5; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (var i = 3; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }
        else
        {
            for (var i = 5; i >= -5; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (var i = 5; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }


        for (var i = 3; i <= 11 + lvlSize; ++i)
            for (var j = 5; j >= -5; --j)
                if (i % 2 != 0 || i % 2 == 0 && Mathf.Abs(j % 2) == 1)
                    softWallPositions.Add(new Vector3(i, 0f, j));
    }

    void PutSoftWalls()
    {
        var softWallsNumer = (int) (softWallPositions.Count * 0.9f);

        for (var i = 0; i < softWallsNumer; ++i)
        {
            var tmp = Random.Range(0, softWallPositions.Count);
            Instantiate(softWall, softWallPositions[tmp], Quaternion.identity).transform.parent = runtimeSoftWalls;
            softWallPositions.RemoveAt(tmp);
        }
    }
}