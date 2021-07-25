using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntitiy : MonoBehaviour, IDamageable
{
    private float startingHealth = 1000f;       // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; }    // 사망 상태
    public event Action onDeath;                // 사망 시 발동할 이벤트

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnEnable()
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;
    }

    public virtual void OnDamage(float damage)
    {
        health -= damage;       // 대미지 만큼 현재 체력이 감소.

        if(health <=0 && !dead) // 현재 체력이 0 이하이고 아직 죽지 않았다면 사망 처리 실행
        {
            Die();
        }
    }

    public virtual void Die() 
    {
        Debug.Log("죽었다");
    }
}
