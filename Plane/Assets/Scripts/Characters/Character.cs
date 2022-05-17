using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;//死亡特效

    [SerializeField] protected AudioData deathSFX;//死亡音效

    [Header("--------HEALTH--------")]

    [SerializeField] protected float maxHealth;//protected：对子类来说是public

    [SerializeField] Statsbar onHeadHealthBar;//头上的血条

    [SerializeField] bool showOnHeadHealthBar = true;//是否显示头上的血条

    protected float health;
    protected virtual void OnEnable()
    {
        health = maxHealth;
        if (showOnHeadHealthBar)
        {//开启显示头上血条
            ShowOnHeadHealthBar();
        }
        else
        {//隐藏血条
            HideOnHeadHealthBar();
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }


    public void ShowOnHeadHealthBar()
    {//显示 血条
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);
    }
    public void HideOnHeadHealthBar()
    {//隐藏 血条
        onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {//受伤函数
        if (health == 0f)
            return;//假如血量为0则直接返回不再执行
        health -= damage;
        if (showOnHeadHealthBar)
        {//如果血条显示 调用statsbar中更新血条函数
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
        PoolManager.Release(deathVFX, transform.position);//对象池启动死亡特效
        gameObject.SetActive(false);//禁用对象
    }

    public virtual void RestoreHealth(float value)
    {   //回复生命值
        if (health == maxHealth)
            return;
        //health += value;
        //health = Mathf.Clamp(health, 0f, maxHealth);//限制区间值
        health = Mathf.Clamp(health + value, 0f, maxHealth);//限制区间值
        if (showOnHeadHealthBar)
        {//如果血条显示 调用statsbar中更新血条函数
            onHeadHealthBar.UpdateStats(health, maxHealth);
        }

    }
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {   //自回血函数
        while (health < maxHealth)
        {
            yield return waitTime;//等待一会儿
            RestoreHealth(percent * maxHealth);//回血
        }
    }
    protected IEnumerator DemageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {   //自扣血函数
        while (health > 0f)
        {
            yield return waitTime;//等待一会儿
            TakeDamage(percent * maxHealth);//扣血
        }
    }

}
