using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;    // 체력을 표시할 UI 슬라이더

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable 실행(상태 초기화)
        base.OnEnable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }    

    public override void OnDamage(float damage)
    {
        // LivingEntity의 OnDamage 실행(대미지 적용)
        base.OnDamage(damage);
        healthSlider.value = health;
    }
}
