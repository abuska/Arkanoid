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
        bool offsetMode = Random.Range(0, 2) == 1;
        bool isRow = Random.Range(0, 2) == 1;

        float offsetX = isRow && offsetMode ? Random.Range(-2, 2) : 0;
        float origOffsetX = ((mapSize.x * brickSize.x));
        float gapOffsetX = (((mapSize.x - 1) * (brickOffset.x - brickSize.x)));
        float columnOffsetX = (((mapSize.y - 1) * offsetX));

        float offsetY = !isRow && offsetMode ? Random.Range(-2, 2) : 0;
        float origOffsetY = ((mapSize.y * brickSize.y));
        float gapOffsetY = (((mapSize.y - 1) * (brickOffset.y - brickSize.y)));
        float rowOffset = (((mapSize.x - 1) * offsetY));
        mapOffset = new Vector2(
            (origOffsetX + gapOffsetX + columnOffsetX) / 2,
            (origOffsetY + gapOffsetY + rowOffset) / 2
        );

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
                float origXPos = (transform.position.x + x * brickOffset.x);
                float offsetXPos = (y * offsetX);

                float origYPos = (transform.position.y + y * brickOffset.y);
                float offsetYPos = (x * offsetY);

                Vector3 position = new Vector3(
                    origXPos - mapOffset.x + offsetXPos + brickSize.x / 2,
                    origYPos - mapOffset.y + offsetYPos + brickSize.y / 2,
                    0
                );

                if (position.x > boundX || position.x < -boundX || position.y > boundY || position.y < -boundY + 2) continue;
                GameObject brick = Instantiate(isRowPattern ? rowBricks[y] : columnBrick, transform);
                brick.transform.position = position;
            }
        }
    }

    private void Awake()
    {
        GenerateLevel();
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
    }

    private void TriggerGeneration()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ChangeLevel();
        }
    }
}