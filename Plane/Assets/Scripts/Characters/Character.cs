using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;//������Ч

    [SerializeField] protected AudioData deathSFX;//������Ч

    [Header("--------HEALTH--------")]

    [SerializeField] protected float maxHealth;//protected����������˵��public

    [SerializeField] Statsbar onHeadHealthBar;//ͷ�ϵ�Ѫ��

    [SerializeField] bool showOnHeadHealthBar = true;//�Ƿ���ʾͷ�ϵ�Ѫ��

    protected float health;
    protected virtual void OnEnable()
    {
        health = maxHealth;
        if (showOnHeadHealthBar)
        {//������ʾͷ��Ѫ��
            ShowOnHeadHealthBar();
        }
        else
        {//����Ѫ��
            HideOnHeadHealthBar();
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }


    public void ShowOnHeadHealthBar()
    {//��ʾ Ѫ��
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }
    public void HideOnHeadHealthBar()
    {//���� Ѫ��
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {//���˺���
        if (health == 0f)
            return;//����Ѫ��Ϊ0��ֱ�ӷ��ز���ִ��
        health -= damage;
        if (showOnHeadHealthBar)
        {//���Ѫ����ʾ ����statsbar�и���Ѫ������
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }
        if (health <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);//���������������Ч
        gameObject.SetActive(false);//���ö���
    }

    public virtual void RestoreHealth(float value)
    {   //�ظ�����ֵ
        if (health == maxHealth)
            return;
        //health += value;
        //health = Mathf.Clamp(health, 0f, maxHealth);//��������ֵ
        health = Mathf.Clamp(health + value, 0f, maxHealth);//��������ֵ
        if (showOnHeadHealthBar)
        {//���Ѫ����ʾ ����statsbar�и���Ѫ������
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }

    }
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {   //�Ի�Ѫ����
        while (health < maxHealth)
        {
            yield return waitTime;//�ȴ�һ���
            RestoreHealth(percent * maxHealth);//��Ѫ
        }
    }
    protected IEnumerator DemageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {   //�Կ�Ѫ����
        while (health > 0f)
        {
            yield return waitTime;//�ȴ�һ���
            TakeDamage(percent * maxHealth);//��Ѫ
        }
    }

}
