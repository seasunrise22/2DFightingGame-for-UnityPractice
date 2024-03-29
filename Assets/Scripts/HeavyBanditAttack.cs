﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBanditAttack : BasicAttack
{
    private HeavyBanditController hbController;    // HeavyBanditController 스크립트에 접근하기 위한 참조용 변수.

    private void Start()
    {
        base.Start();
        hbController = GetComponent<HeavyBanditController>();
    }

    private void FixedUpdate()
    {
        if (gameObject.tag == "Player")
        {
            attackDelay += Time.fixedDeltaTime;

            // 키보드 a키를 눌렀을 경우
            if (Input.GetKey(KeyCode.A) && attackDelay > 0.3f)
            {
                nowAttack = Attacks.attack1;    // a버튼을 누르면 attack1을 쓰고 있다 라고 명시함.
                attackDelay = 0;                // 후딜 초기화
                hbController.walkForce = Vector2.zero;  // HeroKnightController에 접근하여 움직임을 순간 멈추게 한다.
                hbController.hbAnimator.SetTrigger("trigger_Attack1");  // HeroKnightController에 접근하여 attack1에 해당하는 애니메이션을 재생시킨다.
                StartCoroutine(PostDelay(0.3f));
            }

            // 키보드 s키를 눌렀을 경우
            if (Input.GetKey(KeyCode.S) && attackDelay > 0.6f)
            {
                nowAttack = Attacks.attack2;    // s버튼을 누르면 attack2을 쓰고 있다 라고 명시함.
                attackDelay = 0;                // 후딜 초기화
                hbController.walkForce = Vector2.zero;  // HeroKnightController에 접근하여 움직임을 순간 멈추게 한다.
                hbController.hbAnimator.SetTrigger("trigger_Attack2");  // HeroKnightController에 접근하여 attack2에 해당하는 애니메이션을 재생시킨다.
                StartCoroutine(PostDelay(0.6f));
            }
        }            
    }

    // 공격 후딜 설정용 IEnumerator
    protected IEnumerator PostDelay(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        hbController.walkForce = new Vector2(8.0f, 0);  // 매개변수로 받은 후딜이 지난 후 다시 움직임 재개.
    }

    // 트리거로 설정해놓은 공격이 상대에게 히트했을 경우
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.tag == "Enemy" && !isAttack)
        {            
            isAttack = true;
            enemyAnimator.SetTrigger("isHurt");
            switch (nowAttack)
            {
                case Attacks.attack1:
                    gameManager.GetComponent<PlayerHealth>().PlayerOnDamage(PlayerPrefs.GetInt("position"), 100);   // 1번 공격의 데미지 적용.
                    break;
                case Attacks.attack2:
                    gameManager.GetComponent<PlayerHealth>().PlayerOnDamage(PlayerPrefs.GetInt("position"), 200);   // 2번 공격의 데미지 적용.
                    break;
            }
        }
    }    
}
