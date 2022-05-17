using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingLeton<EnemyManager>
{
    //������˶��� 
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;//��ȡ���˲�����ֵ
    public float TimeBetweenWaves => timeBetweenWaves;//��ȡÿ�����ʱ��

    [SerializeField] bool spawnRnemy = true;//�Ƿ����ɵ���

    [SerializeField] GameObject waveUI;//����ui

    [SerializeField] GameObject[] enemyPrefabs;//����Ԥ����

    [SerializeField] float timeBetweenSpawns = 1f;//һ���� �������ɼ��ʱ��

    [SerializeField] float timeBetweenWaves = 1f;//ÿ�����ʱ��

    [SerializeField] int minEnemyAmount = 4;//���ٵ�������

    [SerializeField] int maxEnemyAmount = 10;//����������

    [Header("==== Boss Settings ====")]

    [SerializeField] GameObject bossPrefab;//bossԤ����

    [SerializeField] int bossWaveNumber;//bossս����

    WaitForSeconds waitTimeBetweenSpawns;//һ���� ÿ���������ɼ��ʱ��

    WaitForSeconds waitTimeBetweenWaves;//ÿһ���ļ��ʱ��

    int waveNumber = 1; //���˲���

    int enemyAmount; //���˸���

    List<GameObject> enemyList;//�����б� �洢���� �ж��Ƿ���ڵ��ˣ�����������һ�������õ�

    WaitUntil waitUntilNoEnemy;//����ȴ�ֱ��û�е���

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();//��ʼ���б�
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);//��ʼ��
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);//��ʼ��

        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);//���������������б�Ϊ����true
        //waitUntilNoenemy = new WaitUntil(NoEnemy);
    }
    // bool NoEnemy() => enemyList.Count == 0;//ί�У������б�Ϊ����true

    IEnumerator Start()
    {
        while (spawnRnemy && GameManager.GameState != GameState.GameOver)
        {//�����ɵ��ˣ�������Ϸ״̬���ǽ�����Ϸʱ �ż���ѭ������

            waveUI.SetActive(true);//չʾ����ui

            yield return waitTimeBetweenWaves;//��һ�����ɼ��1s

            waveUI.SetActive(false);//���ò���ui

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));//����һ������
        }

    }
    IEnumerator RandomlySpawnCoroutine()
    {//������ɵ���Э��

        if (waveNumber % bossWaveNumber == 0)
        {//��ǰ���� ģ��bossվ����;ȡ����Ϊ0ʱ����bossս
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);//��������б�
        }
        else
        {
            //ÿһ�����ɵ���������Ϊ���������Զ�ȡ��
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);
            for (int i = 0; i < enemyAmount; i++)
            {
                //��ȡ���һ��Ԥ���壬������������ӵ��б�
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));
                yield return waitTimeBetweenSpawns;//�ȴ�һ��
            }
        }
        yield return waitUntilNoEnemy;//����ȴ���ֱ��û�е���
        waveNumber++;//��������
    }
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);//�б��Ƴ�����


}
