using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public GameObject live;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        int currentLives = gameManager.getLives();
        for (int i = 0; i < currentLives; i++)
        {
            GameObject newLive = Instantiate(live, transform);
        }
    }

    public void RemoveLive()
    {
        if (transform.childCount <= 0) return;
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }

    public void AddLive()
    {
        GameObject newLive = Instantiate(live, transform);
    }

    public void Update()
    {
        int currentLives = gameManager.getLives();
        if (currentLives < transform.childCount)
        {
            RemoveLive();
        }
        else if (currentLives > transform.childCount)
        {
            AddLive();
        }
    }
}
