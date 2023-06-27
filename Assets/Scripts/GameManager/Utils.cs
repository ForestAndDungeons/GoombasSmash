using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    public static Vector3 GetRandomSpawn()
    {
        return new Vector3(Random.Range(-3,3),0.5f,0f);
    }
}
