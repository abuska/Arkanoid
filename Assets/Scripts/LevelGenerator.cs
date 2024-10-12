using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int mapSize;
    public Vector2 brickOffset;
    private Vector2 brickSize = new Vector2(1, 0.5f);
    private Vector2 mapOffset;
    private float boundX = 7.5f;
    private float boundY = 4.5f;
    public GameObject[] brickPrefabs;


    private GameObject getRandomBrick()
    {
        return brickPrefabs[Random.Range(0, brickPrefabs.Length)];
    }

    private void GenerateLevel()
    {
        mapOffset = new Vector2(mapSize.x * brickSize.x / 2, mapSize.y * brickSize.y / 2);
        bool isRowPattern = Random.Range(0, 2) == 1;
        GameObject[] rowBricks = new GameObject[mapSize.y];
        if (isRowPattern)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                rowBricks[y] = getRandomBrick();
            }
        }
        for (int x = 0; x < mapSize.x; x++)
        {
            GameObject columnBrick = getRandomBrick();
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 position = new Vector3(
                    ((transform.position.x + x) - (mapOffset.x - brickSize.x / 2)) * brickOffset.x,
                    transform.position.y + y * brickOffset.y,
                    0
                );

                if (position.x > boundX || position.x < -boundX || position.y > boundY) continue;
                GameObject brick = Instantiate(isRowPattern ? rowBricks[y] : columnBrick, transform);
                brick.transform.position = position;
            }
        }
    }

    private void Awake()
    {
        GenerateLevel();

    }
    private void Update()
    {
        if (transform.childCount == 0)
        {
            ChangeLevel();
        }
        // For testing purposes
        // TriggerGeneration();
    }
    public void ChangeLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        mapSize = new Vector2Int(Random.Range(5, 10), Random.Range(5, 10));
        brickOffset = new Vector2(Random.Range(brickSize.x, 3), Random.Range(brickSize.y, 3));
        GenerateLevel();
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.levelUp();
    }
    private void TriggerGeneration()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ChangeLevel();
        }
    }
}