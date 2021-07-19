using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{    
    private bool isDead = false; // 사망여부    
    private bool isGrounded = false; // 캐릭터가 점프했을 때 애니메이터에 Set시킬 상태.
    private float jumpForce = 350f; // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Vector2 upRightForce = new Vector2(350f, 300f); // 오른쪽위로 점프할 때 가할 힘
    private Vector2 upLeftForce = new Vector2(-350f, 300f); // 왼쪽위로 점프할 때 가할 힘    
    private Vector2 horizontalMoveForce = new Vector2(8.0f, 0);   

    private Rigidbody2D heroKnightRigidbody; // 리지드바디 접근용    
    private Animator heroKnightAnimator; // 애니메이터 접근용

    // Start is called before the first frame update
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
            // 위쪽, 오른쪽 화살표가 같이 눌려져 있을 때(오른쪽위로 점프)
            if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && heroKnightRigidbody.velocity.y == 0)
            {
                heroKnightRigidbody.AddForce(upRightForce);
            }
            // 위쪽, 왼쪽 화살표가 같이 눌려져 있을 때(왼쪽위로 점프)
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && heroKnightRigidbody.velocity.y == 0)
            {
                heroKnightRigidbody.AddForce(upLeftForce);
            }
            // 위쪽 방향키 입력했을 때(수직점프)
            else if (Input.GetKeyDown(KeyCode.UpArrow) && heroKnightRigidbody.velocity.y == 0) 
            {
                heroKnightRigidbody.AddForce(transform.up * jumpForce);
            }
            // 오른쪽 방향키 입력했을 때(우측무빙)
            else if (Input.GetKey(KeyCode.RightArrow) && heroKnightRigidbody.velocity.y == 0)
            {
                heroKnightRigidbody.MovePosition(heroKnightRigidbody.position + horizontalMoveForce * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && heroKnightRigidbody.velocity.y == 0) // 왼쪽 방향키 입력
            {
                heroKnightRigidbody.MovePosition(heroKnightRigidbody.position + horizontalMoveForce * -1 * Time.fixedDeltaTime);
            }
        }

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
