using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{
    private bool isDead = false;
    public float jumpForce = 350f; // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Rigidbody2D heroKnightRigidbody;
    private bool isGrounded = false; // 캐릭터가 점프했을 때 애니메이터에 Set시킬 상태.
    private Animator heroKnightAnimator; // 애니메이터 접근용 변수

    // Start is called before the first frame update
    void Start()
    {
        heroKnightRigidbody = GetComponent<Rigidbody2D>();
        heroKnightAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && heroKnightRigidbody.velocity.y == 0) // 위쪽 방향키 입력을 했고, y축으로 속도가 설정되지 않은 상태라면 점프시킨다.
            {
                heroKnightRigidbody.AddForce(new Vector2(0, jumpForce));
            }
            else if (Input.GetKey(KeyCode.RightArrow)) // 오른쪽 방향키 입력
            {
                transform.Translate(0.2f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) // 왼쪽 방향키 입력
            {
                transform.Translate(-0.2f, 0, 0);
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
