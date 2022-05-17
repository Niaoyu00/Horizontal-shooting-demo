using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] LootSetting[] lootSettings;//ս��Ʒ��������,����buff

    public void Spawn(Vector2 position)
    {
        foreach (var item in lootSettings)
        {
            item.Spawn(position + Random.insideUnitCircle);//����λ�����ƫ��
        }
    }
}
