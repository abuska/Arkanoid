using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDown : MonoBehaviour
{
    private static float lockTimer = 5f;
    private float lockTimerSetter = lockTimer;


    public void ResetTimer()
    {
        lockTimerSetter = lockTimer;
    }


    void Update()
    {
        if (lockTimerSetter > 0)
        {
            lockTimerSetter -= Time.deltaTime;
            if (lockTimerSetter <= 0)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
