using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{
    

    
    private float attackDelay;          // 공격 후 후딜 설정용.

    

    /* 컴포넌트 접근용 */
    private Rigidbody2D heroKnightRigidbody;    // 리지드바디 접근용    
    private Animator heroKnightAnimator;        // 애니메이터 접근용
    private GameObject opponentObject;          // 상대 오브젝트 접근용

    void Start()
    {
        heroKnightRigidbody = GetComponent<Rigidbody2D>();
        heroKnightAnimator = GetComponent<Animator>();
        opponentObject = GameObject.FindWithTag("Opponent");
    }

    private void FixedUpdate()
    {
        AttackFunction();
        heroKnightAnimator.SetBool("isRun", isRun);             // heroKnight의 애니메이션 값을 계속해서 갱신
        heroKnightAnimator.SetBool("isGrounded", isGrounded);   // heroKnight의 애니메이션 값을 계속해서 갱신
    }

    



    // 공격 관련 기능들을 넣어둔 함수
    private void AttackFunction()
    {
        attackDelay += Time.fixedDeltaTime;

        // 키보드 a키를 눌렀을 경우
        if (Input.GetKey(KeyCode.A) && attackDelay > 0.5f)
        {
            attackDelay = 0;
            walkForce = Vector2.zero;
            heroKnightAnimator.SetTrigger("trigger_Attack_A");
        }

        // 후딜이 끝나면 다시 움직이도록.
        if (attackDelay > 0.5f)
            walkForce = new Vector2(8.0f, 0);
    }

    // 트리거로 설정해놓은 공격이 상대에게 히트했을 경우
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Opponent")
        {
            if(attackCount == 0)
            {
                Debug.Log("Hit! : " + test);
                GetComponent<PlayerHealth>().OnDamage(100); // 시험삼아 공겨 적중 시 데미지 100을 넘겨준다.
                attackCount++;
                test++;
            }
            else if(attackCount == 1)
            {
                attackCount--;
            }
        }
    }

    // 바닥에 닿았을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            heroKnightRigidbody.velocity = Vector2.zero; // 대각선 점프 후 착지했을 때 밀림 방지용.            
        }
    }

    // 바닥에서 떨어졌을 때 == 점프했을 때. tag로 지정해주지 않으면 상대와 붙고 난 뒤 떨어졌을 때에도 뛰는 모션이 적용되어 버린다.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
