using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootSetting
{
    public GameObject prefab;//Ô¤ÖÆÌå
    [Range(0f, 100f)] public float dropPercentage;//±¬ÂÊ

    public void Spawn(Vector3 position)
    {//µôÂäbuff
        if (Random.Range(0f, 100f) <= dropPercentage)
        {
            PoolManager.Release(prefab, position);
        }
    }

}
