using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnightController : MonoBehaviour
{
    /* 움직임 관련 : 캐릭터에 작용시킬 힘 */
    private float jumpForce = 350f;                         // 위쪽 방향키 입력했을 때 캐릭터의 위쪽으로 작용할 힘.
    private Vector2 upRightForce = new Vector2(250f, 480f); // 오른쪽위로 점프할 때 가할 힘
    private Vector2 upLeftForce = new Vector2(-250f, 480f); // 왼쪽위로 점프할 때 가할 힘    
    private Vector2 walkForce = new Vector2(8.0f, 0);       // 좌우 이동 할때의 힘     
    private Vector2 gravityForce = new Vector2(0f, -9.8f);  // 캐릭터가 낙하할 때 적용시킬 중력.

    /* 각종 타이머 */
    private float idleChecker;          // 아무것도 입력하지 않았을 때의 행동(달리기->Idle)을 결정짓기 위한 타이머
    private float attackDelay;          // 공격 후 후딜 설정용.  

    /* 상태 관련 */
    private bool isGrounded = false;    // 캐릭터가 점프했을 때 애니메이터에 Set시킬 점프 상태.
    private bool isRun = false;         // 캐릭터를 좌우로 조작했을 때 애니메이터에 Set시킬 달리기 상태.
    private string isPlayer;            // 이 오브젝트는 플레이어인가 적인가.
    bool isAttack = false;              // 공격을 이중으로 넣지 않기 위한 상태변수.

    /* 참조용 변수 */
    private Rigidbody2D hkRigidbody2D;      // 리지드바디 접근용.
    private Animator hkAnimator;            // 애니메이터 접근용
    private GameObject enemy;               // 적 오브젝트 접근용
    private GameObject player;              // 플레이어 오브젝트 접근용.
    private GameObject gameManager;         // 게임 매니저 접근용
    private Animator enemyAnimator;         // 적 애니메이터 접근용.

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        hkRigidbody2D = GetComponent<Rigidbody2D>();
        hkAnimator = GetComponent<Animator>();
        isPlayer = gameObject.tag;                      // 이 오브젝트는 플레이어인가 적인가 이 오브젝트에 붙은 태그를 가져온다.        
        enemy = GameObject.FindWithTag("Enemy");        // "Enemy"태그가 붙은 오브젝트를 가져와서 변수에 가져다 놓음.
        enemyAnimator = enemy.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (isPlayer == "Enemy")
        {
            RotateFunction(gameObject.tag);
            hkAnimator.SetBool("isRun", isRun);             // heroKnight의 애니메이션 값을 계속해서 갱신
            hkAnimator.SetBool("isGrounded", isGrounded);   // heroKnight의 애니메이션 값을 계속해서 갱신
        }
        else if(isPlayer == "Player")
        {
            AttackFunction();           // 공격 후딜 설정, 공격 애니메이션 트리거 작동.
            MoveFunction();
            RotateFunction(gameObject.tag);   // 상대와 나의 x좌표값을 비교해서 서로 마주보게끔 방향을 돌리게 하기 위한 함수.
            hkAnimator.SetBool("isRun", isRun);             // heroKnight의 애니메이션 값을 계속해서 갱신
            hkAnimator.SetBool("isGrounded", isGrounded);   // heroKnight의 애니메이션 값을 계속해서 갱신            
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
        if (hkRigidbody2D.velocity.y < 0)
        {
            hkRigidbody2D.AddForce(gravityForce);
        }

        // '오른쪽+위' 입력
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && hkRigidbody2D.velocity.y == 0)
        {
            hkRigidbody2D.AddForce(upRightForce);
        }
        // '왼쪽+위' 입력
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow) && hkRigidbody2D.velocity.y == 0)
        {
            hkRigidbody2D.AddForce(upLeftForce);
        }
        // '위쪽' 입력
        else if (Input.GetKeyDown(KeyCode.UpArrow) && hkRigidbody2D.velocity.y == 0)
        {
            hkRigidbody2D.AddForce(transform.up * jumpForce);
        }
        // '오른쪽' 입력
        else if (Input.GetKey(KeyCode.RightArrow) && hkRigidbody2D.velocity.y == 0)
        {
            idleChecker = 0;
            hkRigidbody2D.MovePosition(hkRigidbody2D.position + walkForce * Time.fixedDeltaTime);
        }
        // '왼쪽' 입력
        else if (Input.GetKey(KeyCode.LeftArrow) && hkRigidbody2D.velocity.y == 0)
        {
            idleChecker = 0;
            hkRigidbody2D.MovePosition(hkRigidbody2D.position + walkForce * -1 * Time.fixedDeltaTime);
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
            hkAnimator.SetTrigger("trigger_Attack_A");
        }

        // 후딜이 끝나면 다시 움직이도록.
        if (attackDelay > 0.5f)
            walkForce = new Vector2(8.0f, 0);
    }

    private void RotateFunction(string isPlayer)
    {
        // 이 오브젝트가 적일 때의 상황
        if (isPlayer == "Enemy")
        {            
            // 적이 더 왼쪽에 있다면
            if (transform.position.x < player.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            // 적이 더 오른쪽에 있다면
            else if (transform.position.x > player.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
        else if(isPlayer == "Player")
        {
            // 내가 더 왼쪽에 있다면
            if (transform.position.x < enemy.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            // 내가 더 오른쪽에 있다면
            else if (transform.position.x > enemy.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
        }        
    }

    // 트리거로 설정해놓은 공격이 상대에게 히트했을 경우
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !isAttack)
        {
            isAttack = true;
            gameManager.GetComponent<PlayerHealth>().PlayerOnDamage(PlayerPrefs.GetInt("position"), 100); // 우선 시험삼아 데미지 100 넘겨줌.
            enemyAnimator.SetTrigger("isHurt");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isAttack)
        {
            isAttack = false;
        }
    }

    // 바닥에 닿았을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            hkRigidbody2D.velocity = Vector2.zero; // 대각선 점프 후 착지했을 때 밀림 방지용.            
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
