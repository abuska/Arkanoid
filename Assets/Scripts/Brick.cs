using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public float value = 1;
    public float life = 1;

    public GameObject[] powerUps;

    public float Hit()
    {
        life -= 1;
        if (life <= 0)
        {
            bool spawnPowerUp = Random.Range(0, 100) < 15;
            if (spawnPowerUp)
            {
                GameObject powerUp = powerUps[Random.Range(0, powerUps.Length)];
                GameObject item = Instantiate(powerUp, transform.position, Quaternion.identity);
                item.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            Destroy(this.gameObject);
            return value;
        }
        return 0;
    }
}
