using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{
    /* 상태변수 */
    private bool isDead = false;        // 사망여부    
    private bool isGrounded = false;    // 캐릭터가 점프했을 때 애니메이터에 Set시킬 점프 상태.
    private bool isRun = false;         // 캐릭터를 좌우로 조작했을 때 애니메이터에 Set시킬 달리기 상태.
    private int attackCount;            // 공격이 이중으로 들어가지 않도록 하기 위한 공격판정 변수.
    private int test;                   // 카운트를 세기위한 테스트용 변수.

    /* 타이머 */
    private float idleChecker;          // 아무것도 입력하지 않았을 때의 행동(달리기->Idle)을 결정짓기 위한 타이머
    private float attackDelay;          // 공격 후 후딜 설정용.

    /* 캐릭터에 적용시킬 힘 */
    private float jumpForce = 350f;     // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Vector2 upRightForce = new Vector2(250f, 480f); // 오른쪽위로 점프할 때 가할 힘
    private Vector2 upLeftForce = new Vector2(-250f, 480f); // 왼쪽위로 점프할 때 가할 힘    
    private Vector2 walkForce = new Vector2(8.0f, 0);       // 좌우 이동 할때의 힘     
    private Vector2 gravityForce = new Vector2(0f, -9.8f);  // 캐릭터가 낙하할 때 적용시킬 중력.

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
        if (isDead)
        {
            return;
        }
        else
        {
            MoveFunction();
            AttackFunction();            

            heroKnightAnimator.SetBool("isRun", isRun);             // heroKnight의 애니메이션 값을 계속해서 갱신
            heroKnightAnimator.SetBool("isGrounded", isGrounded);   // heroKnight의 애니메이션 값을 계속해서 갱신
        }
    }

    private void RotateFunction()
    {
        // 내가 더 왼쪽에 있다면
        if (transform.position.x < opponentObject.transform.position.x)
            
        {
            Debug.Log("내가 더 왼쪽");
            if (Mathf.Abs(transform.position.x) == Mathf.Abs(opponentObject.transform.position.x))
                return;
            else
                transform.Rotate(0f, 180f, 0f);
        }
        // 내가 더 오른쪽에 있다면
        else if (transform.position.x > opponentObject.transform.position.x)
        {
            Debug.Log("내가 더 오른쪽");
            if (Mathf.Abs(transform.position.x) == Mathf.Abs(opponentObject.transform.position.x))
                return;
            else
                transform.Rotate(0f, 180f, 0f);
        }
            
    }

    // 이동 관련 기능들을 넣어둔 함수
    private void MoveFunction()
    {
        // Idle 상태인지 아닌지 체크
        idleChecker += Time.fixedDeltaTime;

        if (idleChecker > 0.1f) // 단위 초. idleTime만큼 움직이지 않았다면.
            isRun = false;
        else
            isRun = true;

        // 캐릭터가 점프 후 낙하하고 있을 때 좀 더 빨리 떨어지도록 하기 위한 코드
        if (heroKnightRigidbody.velocity.y < 0)
        {
            heroKnightRigidbody.AddForce(gravityForce);
        }

        // '오른쪽+위' 입력
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && heroKnightRigidbody.velocity.y == 0)
        {
            heroKnightRigidbody.AddForce(upRightForce);
        }
        // '왼쪽+위' 입력
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && heroKnightRigidbody.velocity.y == 0)
        {
            heroKnightRigidbody.AddForce(upLeftForce);
        }
        // '위쪽' 입력
        else if (Input.GetKeyDown(KeyCode.UpArrow) && heroKnightRigidbody.velocity.y == 0)
        {
            heroKnightRigidbody.AddForce(transform.up * jumpForce);
        }
        // '오른쪽' 입력
        else if (Input.GetKey(KeyCode.RightArrow) && heroKnightRigidbody.velocity.y == 0)
        {
            idleChecker = 0;
            heroKnightRigidbody.MovePosition(heroKnightRigidbody.position + walkForce * Time.fixedDeltaTime);
        }
        // '왼쪽' 입력
        else if (Input.GetKey(KeyCode.LeftArrow) && heroKnightRigidbody.velocity.y == 0)
        {
            idleChecker = 0;
            heroKnightRigidbody.MovePosition(heroKnightRigidbody.position + walkForce * -1 * Time.fixedDeltaTime);
        }
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
        if(attackDelay > 0.5f)
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

            RotateFunction(); // 바닥에 닿았을 때 상대와 나의 x값을 비교해서 마주보게끔 만든다.
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
