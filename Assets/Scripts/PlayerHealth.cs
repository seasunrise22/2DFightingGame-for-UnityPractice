using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    /* 생명력 관련 */
    private float startingHealth = 1000f;       // 시작 체력
    public float playerHealth { get; protected set; }   // 플레이어 '현재' 체력
    public float enemyHealth { get; protected set; }    // 적 '현재' 체력     
    public bool playerDead { get; protected set; }      // 플레이어 사망 상태
    public bool enemyDead { get; protected set; }       // 적 사망상태
    public event Action onDeath;                // 사망 시 발동할 이벤트

    public Slider healthSlider1P;    // 체력을 표시할 UI 슬라이더. Inspector 창에서 끌어다 여기로 넣음.
    public Slider healthSlider2P;    // 체력을 표시할 UI 슬라이더. Inspector 창에서 끌어다 여기로 넣음.

    private void Start()
    {
        // 사망하지 않은 상태로 시작
        playerDead = false;
        enemyDead = false;

        // 체력을 시작 체력으로 초기화
        playerHealth = startingHealth;
        enemyHealth = startingHealth;
    }

    // 플레이어가 공격했을 때 호출될 메서드
    public void PlayerOnDamage(int position, float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0 && !enemyDead) // 현재 체력이 0 이하이고 적이 아직 죽지 않았다면 사망 처리 실행
        {
            EnemyDie();
        }

        switch (position)
        {
            case 1:
                // 플레이어가 1P일 경우
                healthSlider2P.value = enemyHealth;                
                break;
            case 2:
                // 플레이어가 2P일 경우
                healthSlider1P.value = enemyHealth;
                break;
        }        
    }

    public void EnemyDie()
    {
        Debug.Log("적 죽었다");
    }
}
