using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingLeton<EnemyManager>
{
    //随机敌人对象 
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;//获取敌人波数数值
    public float TimeBetweenWaves => timeBetweenWaves;//获取每波间隔时间

    [SerializeField] bool spawnRnemy = true;//是否生成敌人

    [SerializeField] GameObject waveUI;//波数ui

    [SerializeField] GameObject[] enemyPrefabs;//敌人预制体

    [SerializeField] float timeBetweenSpawns = 1f;//一波内 敌人生成间隔时间

    [SerializeField] float timeBetweenWaves = 1f;//每波间隔时间

    [SerializeField] int minEnemyAmount = 4;//最少敌人数量

    [SerializeField] int maxEnemyAmount = 10;//最多敌人数量

    [Header("==== Boss Settings ====")]

    [SerializeField] GameObject bossPrefab;//boss预制体

    [SerializeField] int bossWaveNumber;//boss战波数

    WaitForSeconds waitTimeBetweenSpawns;//一波内 每个敌人生成间隔时间

    WaitForSeconds waitTimeBetweenWaves;//每一波的间隔时间

    int waveNumber = 1; //敌人波数

    int enemyAmount; //敌人个数

    List<GameObject> enemyList;//敌人列表 存储敌人 判断是否存在敌人，并且生成下一波敌人用的

    WaitUntil waitUntilNoEnemy;//挂起等待直到没有敌人

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();//初始化列表
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);//初始化
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);//初始化

        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);//匿名函数，敌人列表为空则true
        //waitUntilNoenemy = new WaitUntil(NoEnemy);
    }
    // bool NoEnemy() => enemyList.Count == 0;//委托，敌人列表为空则true

    IEnumerator Start()
    {
        while (spawnRnemy && GameManager.GameState != GameState.GameOver)
        {//可生成敌人，并且游戏状态不是结束游戏时 才继续循环生成

            waveUI.SetActive(true);//展示波数ui

            yield return waitTimeBetweenWaves;//下一波生成间隔1s

            waveUI.SetActive(false);//禁用波数ui

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));//生成一波敌人
        }

    }
    IEnumerator RandomlySpawnCoroutine()
    {//随机生成敌人协程

        if (waveNumber % bossWaveNumber == 0)
        {//当前波数 模上boss站波数;取余数为0时开启boss战
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);//加入敌人列表
        }
        else
        {
            //每一波生成的数量，因为整形所以自动取整
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);
            for (int i = 0; i < enemyAmount; i++)
            {
                //获取随机一个预制体，并创建。并添加到列表
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                yield return waitTimeBetweenSpawns;//等待一秒
            }
        }
        yield return waitUntilNoEnemy;//挂起等待，直到没有敌人
        waveNumber++;//波数增加
    }
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);//列表移除敌人


}
