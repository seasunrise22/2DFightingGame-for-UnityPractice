using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * 살아있는 생명체라면 공통적으로 가질 범용적인 기능들을 구현한 클래스.
 */
public class LivingEntity : MonoBehaviour, IDamageable
{
    /* 생명력 관련 */
    private float startingHealth = 1000f;       // 시작 체력
    public float health { get; protected set; } // 현재 체력                                       
    public bool dead { get; protected set; }    // 사망 상태
    public event Action onDeath;                // 사망 시 발동할 이벤트

    /* 상태 관련 */
    private bool isDead = false;        // 사망여부    
    private bool isGrounded = false;    // 캐릭터가 점프했을 때 애니메이터에 Set시킬 점프 상태.
    private bool isRun = false;         // 캐릭터를 좌우로 조작했을 때 애니메이터에 Set시킬 달리기 상태.
    private int attackCount;            // 공격이 이중으로 들어가지 않도록 하기 위한 공격판정 변수.

    /* 각종 타이머 */
    private float idleChecker;          // 아무것도 입력하지 않았을 때의 행동(달리기->Idle)을 결정짓기 위한 타이머

    /* 움직임 관련 : 캐릭터에 작용시킬 힘 */
    private float jumpForce = 350f;                         // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Vector2 upRightForce = new Vector2(250f, 480f); // 오른쪽위로 점프할 때 가할 힘
    private Vector2 upLeftForce = new Vector2(-250f, 480f); // 왼쪽위로 점프할 때 가할 힘    
    private Vector2 walkForce = new Vector2(8.0f, 0);       // 좌우 이동 할때의 힘     
    private Vector2 gravityForce = new Vector2(0f, -9.8f);  // 캐릭터가 낙하할 때 적용시킬 중력.

    /* 참조용 변수 */
    Rigidbody2D objRigidbody2D;

    private void Start()
    {
        objRigidbody2D = gameObject.GetComponent<Rigidbody2D>();    // 이 오브젝트에 물리력을 행사하기 위해 Rigidbody2D 컴포넌트에 접근한다.
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            MoveFunction();     // 캐릭터 이동을 구현한 함수    
            /*RotateFunction();*/   // 상대와 나의 x좌표값을 비교해서 서로 마주보게끔 방향을 돌리게 하기 위한 함수.
        }
    }

    // 이동 관련 기능들을 넣어둔 함수
    private void MoveFunction()
    {
        // Idle 상태인지 아닌지 체크
        idleChecker += Time.fixedDeltaTime;

        if (idleChecker > 0.1f) // 단위 초. idleTime만큼 움직이지 않았다면.
            isRun = false;      // 현재 달리고 있지 않다.
        else
            isRun = true;       // idleChecker가 초기화 되어 0.1을 넘지 않았다면 현재 달리고 있는 상태이다.

        // 캐릭터가 점프 후 낙하하고 있을 때 좀 더 빨리 떨어지도록 하기 위한 코드
        if (objRigidbody2D.velocity.y < 0)
        {
            objRigidbody2D.AddForce(gravityForce);
        }

        // '오른쪽+위' 입력
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && objRigidbody2D.velocity.y == 0)
        {
            objRigidbody2D.AddForce(upRightForce);
        }
        // '왼쪽+위' 입력
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && objRigidbody2D.velocity.y == 0)
        {
            objRigidbody2D.AddForce(upLeftForce);
        }
        // '위쪽' 입력
        else if (Input.GetKeyDown(KeyCode.UpArrow) && objRigidbody2D.velocity.y == 0)
        {
            objRigidbody2D.AddForce(transform.up * jumpForce);
        }
        // '오른쪽' 입력
        else if (Input.GetKey(KeyCode.RightArrow) && objRigidbody2D.velocity.y == 0)
        {
            idleChecker = 0;
            objRigidbody2D.MovePosition(objRigidbody2D.position + walkForce * Time.fixedDeltaTime);
        }
        // '왼쪽' 입력
        else if (Input.GetKey(KeyCode.LeftArrow) && objRigidbody2D.velocity.y == 0)
        {
            idleChecker = 0;
            objRigidbody2D.MovePosition(objRigidbody2D.position + walkForce * -1 * Time.fixedDeltaTime);
        }
    }

    /*private void RotateFunction()
    {
        // 내가 더 왼쪽에 있다면
        if (transform.position.x < opponentObject.transform.position.x)

        {
            Debug.Log("내가 더 왼쪽");
            *//*if (Mathf.Abs(transform.position.x) == Mathf.Abs(opponentObject.transform.position.x))
                return;
            내가 이걸 왜 넣었지? 이거 넣으면 첨 시작할 때 -5, 5로 절대값 같아서 방향 안 바뀌는데... 무슨 이유가 있어서 넣었나?
            else*//*
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        // 내가 더 오른쪽에 있다면
        else if (transform.position.x > opponentObject.transform.position.x)
        {
            Debug.Log("내가 더 오른쪽");
            *//*if (Mathf.Abs(transform.position.x) == Mathf.Abs(opponentObject.transform.position.x))
                return;
            else*//*
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }*/



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
