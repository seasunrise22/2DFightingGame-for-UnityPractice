# 2DFightingGame-for-UnityPractice
- 개발인원 : 1명
- 역할
  - 전체
## Introduction
유니티로 만들어 본 2D 격투게임 입니다.

두 개의 캐릭터 중 하나의 캐릭터를 고를 수 있으며 1P 와 2P 를 선택할 수 있습니다.
## Development Environment
- Unity 2019.1.0f2
## Screenshots
![스크린샷 2023-10-19 193227](https://github.com/seasunrise22/2DFightingGame-for-UnityPractice/assets/45503931/92d3fbb4-168b-4280-a2cd-f4ad77f7fbf4)
![스크린샷 2023-10-19 193305](https://github.com/seasunrise22/2DFightingGame-for-UnityPractice/assets/45503931/28094a66-f121-4511-8eb3-4469d918f5df)
![스크린샷 2023-10-19 193324](https://github.com/seasunrise22/2DFightingGame-for-UnityPractice/assets/45503931/a8a0ff4c-2bfc-4c4b-aadc-770b32342358)
![스크린샷 2023-10-19 193416](https://github.com/seasunrise22/2DFightingGame-for-UnityPractice/assets/45503931/6cb953ea-152a-4734-82ff-04867319b363)
![스크린샷 2023-10-19 193532](https://github.com/seasunrise22/2DFightingGame-for-UnityPractice/assets/45503931/4adce837-9f4a-4b4f-a9fa-067d5f50f5d1)
## Code Preview
### 게임의 흐름을 제어하는 GameManager
```c#
GameManager.cs

public float timeLeft = 99.5f;        

    private void Awake()
    {
        characterPrefabLists = GameObject.Find("CharacterPrefabs");     //변수에 실제 캐릭터 프리펩이 담긴 리스트 오브젝트를 연결한다.

        //선택된 플레이어 캐릭터와 적 캐릭터 인덱스를 가져다가 변수에 넣는다.
        selectedPlayerIdx = PlayerPrefs.GetInt("CharacterSelected");
        selectedEnemyIdx = PlayerPrefs.GetInt("EnemySelected");

        //선택된 플레이어 캐릭터에게는 'Player' 태그를, 선택된 적 캐릭터에게는 'Opponent' 태그를 부착한다.
        characterPrefabLists.transform.GetChild(selectedPlayerIdx).tag = "Player";
        characterPrefabLists.transform.GetChild(selectedEnemyIdx).tag = "Enemy";
    }

    private void Start()
    {
        timerText = GameObject.Find("Timer");
    }

    private void Update()
    {
        timerText.GetComponent<Text>().text = (timeLeft).ToString("0"); // "0" = 정수형태로 변환. 반올림값임. ex) 98.5 = 99로 표시
        timeLeft -= Time.deltaTime;         
        if(timeLeft < 0.5f)
        {
            timerText.GetComponent<Text>().text = "GameOver";
        }
    }
```
### 캐릭터 제어(공격)
```c#
HeroKnightAttack.cs

private void FixedUpdate()
    {
        attackDelay += Time.fixedDeltaTime;

        if(gameObject.tag == "Player")
        {
            // 키보드 a키를 눌렀을 경우
            if (Input.GetKey(KeyCode.A) && attackDelay > 0.3f)
            {
                nowAttack = Attacks.attack1;    // a버튼을 누르면 attack1을 쓰고 있다 라고 명시함.
                attackDelay = 0;                // 후딜 초기화
                hkController.walkForce = Vector2.zero;  // HeroKnightController에 접근하여 움직임을 순간 멈추게 한다.
                hkController.hkAnimator.SetTrigger("trigger_Attack1");  // HeroKnightController에 접근하여 attack1에 해당하는 애니메이션을 재생시킨다.
                StartCoroutine(PostDelay(0.3f));
            }
        }        
    }
```
### 캐릭터 제어(이동)
```c#
HeroKnightController.cs

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
```
