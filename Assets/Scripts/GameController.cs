using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public Material[] bombermanMaterials;
    public int lvlSize;
    public int playersNumber;
    public GameObject hardWall;
    public GameObject floor;
    public GameObject player;
    public GameObject softWall;

    private List<Vector3> softWallPositions;

    private void Awake()
    {
//        lvlSize             = ;
        playersNumber = 4;
        softWallPositions = new List<Vector3>();
    }

    void Start()
    {
        CreateLvl();
        SpanPlayers();
        FillSoftWallsList();
        PutSoftWalls();
    }

    void CreateLvl()
    {
        for (var i = 0; i < lvlSize; ++i)
        {
            Instantiate(hardWall, new Vector3(14f + i, 0f, 6f), Quaternion.identity);
            Instantiate(hardWall, new Vector3(14f + i, 0f, -6f), Quaternion.identity);
            Instantiate(floor, new Vector3(14f + i, -0.5f, 0f), Quaternion.identity);

            if (i % 2 == 0)
            {
                for (int z = 4; z > -5; z -= 2)
                    Instantiate(hardWall, new Vector3(14f + i, 0f, z), Quaternion.identity);
            }
        }

        float x = lvlSize + 14;

        for (int i = 0; i < 13; ++i)
        {
            Instantiate(hardWall, new Vector3(x, 0f, 6f - i), Quaternion.identity);
        }
    }

    void SpanPlayers()
    {
        if (playersNumber >= 1)
        {
            GameObject playerObj = Instantiate(player, new Vector3(1f, 0f, 5f), Quaternion.identity) as GameObject;
            playerObj.GetComponent<Player>().bombermanModel.GetComponent<SkinnedMeshRenderer>().material =
                bombermanMaterials[0];
            playerObj.GetComponent<Player>().playerNumber = 0;
        }


        if (playersNumber >= 2)
        {
            GameObject playerObj = Instantiate(player, new Vector3(1f, 0f, -5f), Quaternion.identity) as GameObject;
            playerObj.GetComponent<Player>().bombermanModel.GetComponent<SkinnedMeshRenderer>().material =
                bombermanMaterials[1];
            playerObj.GetComponent<Player>().playerNumber = 1;
        }


        if (playersNumber >= 3)
        {
            GameObject playerObj =
                Instantiate(player, new Vector3(13f + lvlSize, 0f, 5f), Quaternion.identity) as GameObject;
            playerObj.GetComponent<Player>().bombermanModel.GetComponent<SkinnedMeshRenderer>().material =
                bombermanMaterials[2];
            playerObj.GetComponent<Player>().playerNumber = 2;
        }


        if (playersNumber >= 4)
        {
            GameObject playerObj =
                Instantiate(player, new Vector3(13f + lvlSize, 0f, -5f), Quaternion.identity) as GameObject;
            playerObj.GetComponent<Player>().bombermanModel.GetComponent<SkinnedMeshRenderer>().material =
                bombermanMaterials[3];
            playerObj.GetComponent<Player>().playerNumber = 3;
        }
    }

    void FillSoftWallsList()
    {
        if (playersNumber >= 2)
        {
            for (int i = 3; i >= -3; --i)
                softWallPositions.Add(new Vector3(1f, 0f, i));

            for (int i = 3; i >= -3; i -= 2)
                softWallPositions.Add(new Vector3(2f, 0f, i));
        }
        else
        {
            for (int i = 3; i >= -5; --i)
                softWallPositions.Add(new Vector3(1f, 0f, i));

            for (int i = 3; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(2f, 0f, i));
        }

        if (playersNumber == 4)
        {
            for (int i = 3; i >= -3; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (int i = 3; i >= -3; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }
        else if (playersNumber == 3)
        {
            for (int i = 3; i >= -5; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (int i = 3; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }
        else
        {
            for (int i = 5; i >= -5; --i)
                softWallPositions.Add(new Vector3(13f + lvlSize, 0f, i));

            for (int i = 5; i >= -5; i -= 2)
                softWallPositions.Add(new Vector3(12f + lvlSize, 0f, i));
        }


        for (int i = 3; i <= 11 + lvlSize; ++i)
        {
            for (int j = 5; j >= -5; --j)
            {
                if ((i % 2 != 0) || ((i % 2 == 0) && (Mathf.Abs(j % 2) == 1)))
                    softWallPositions.Add(new Vector3(i, 0f, j));
            }
        }
    }

    void PutSoftWalls()
    {
        int softWallsNumer = (int) (softWallPositions.Count * 0.9f);

        for (int i = 0; i < softWallsNumer; ++i)
        {
            int tmp = Random.Range(0, softWallPositions.Count);

            Instantiate(softWall, softWallPositions[tmp], Quaternion.identity);

            softWallPositions.RemoveAt(tmp);
        }
    }
}