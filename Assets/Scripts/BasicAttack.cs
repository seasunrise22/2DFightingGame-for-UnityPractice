using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicAttack : MonoBehaviour
{
    protected bool isAttack = false;// 공격을 이중으로 넣지 않기 위한 상태변수.
    protected GameObject enemy;               // 적 오브젝트 접근용
    protected Animator enemyAnimator;         // 적 애니메이터 접근용.
    protected GameObject gameManager;        // 게임 매니저 접근용
    protected float attackDelay;    // 공격한 다음 바로 공격이 이어지지 않도록 하기 위하여
    protected enum Attacks          // 공격의 종류들
    {
        attack1,
        attack2
    }
    protected Attacks nowAttack;    // 현재 어떤 공격을 하고 있는가?
    protected HeroKnightController hkController;    // HeroKnightController 스크립트에 접근하기 위한 참조용 변수.

    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");        // "Enemy"태그가 붙은 오브젝트를 가져와서 enemy변수에 가져다 놓음.
        hkController = GetComponent<HeroKnightController>();
        enemyAnimator = enemy.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
    }

    // 공격 후딜 설정용 IEnumerator
    protected IEnumerator PostDelay(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        hkController.walkForce = new Vector2(8.0f, 0);  // 매개변수로 받은 후딜이 지난 후 다시 움직임 재개.
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && isAttack)
        {
            isAttack = false;
        }
    }
}
