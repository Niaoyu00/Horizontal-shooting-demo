using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;//战利品设置数组,掉落buff

    public void Spawn(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);//生成位置随机偏移
        }
    }
}
