using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{
    /* 상태변수 */
    private bool isDead = false;        // 사망여부    
    private bool isGrounded = false;    // 캐릭터가 점프했을 때 애니메이터에 Set시킬 점프 상태.
    private bool isRun = false;         // 캐릭터를 좌우로 조작했을 때 애니메이터에 Set시킬 달리기 상태.

    /* 타이머 */
    private float idleChecker;          // 아무것도 입력하지 않았을 때의 행동(달리기->Idle)을 결정짓기 위한 타이머

    /* 캐릭터에 적용시킬 힘 */
    private float jumpForce = 350f;     // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Vector2 upRightForce = new Vector2(250f, 330f); // 오른쪽위로 점프할 때 가할 힘
    private Vector2 upLeftForce = new Vector2(-250f, 330f); // 왼쪽위로 점프할 때 가할 힘    
    private Vector2 walkForce = new Vector2(8.0f, 0);       // 좌우 이동 할때의 힘     
    private Vector2 gravityForce = new Vector2(0f, -9.8f);  // 캐릭터가 낙하할 때 적용시킬 중력.

    /* 컴포넌트 접근용 */
    private Rigidbody2D heroKnightRigidbody;    // 리지드바디 접근용    
    private Animator heroKnightAnimator;        // 애니메이터 접근용

    void Start()
    {
        heroKnightRigidbody = GetComponent<Rigidbody2D>();
        heroKnightAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            // Idle 상태인지 아닌지 체크
            idleChecker += Time.fixedDeltaTime;

            if (idleChecker > 0.1f) // 단위 초
                isRun = false;
            else
                isRun = true;

            // 캐릭터가 점프 후 낙하하고 있을 때 좀 더 빨리 떨어지도록 하기 위한 코드
            if(heroKnightRigidbody.velocity.y < 0)
            {
                heroKnightRigidbody.AddForce(gravityForce);
            }

            // '오른쪽+위' 입력
            if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && heroKnightRigidbody.velocity.y == 0)
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
        
        heroKnightAnimator.SetBool("isRun", isRun); // heroKnight의 애니메이션 값을 계속해서 갱신
        heroKnightAnimator.SetBool("isGrounded", isGrounded); // heroKnight의 애니메이션 값을 계속해서 갱신
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
