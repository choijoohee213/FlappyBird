using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpPower;
    public bool isStart = false;
    public bool isDie = false;

    Rigidbody2D rigid;
    Animator anim;
    public GameManager gm;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //점프
        //WINDOW : Input.GetButtonDown("Jump")
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isDie)
        {
            gm.audio[4].Play(); //wing 효과음

            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);  //일정한 높이로 점프
            transform.rotation = Quaternion.Euler(0, 0, 15);
            anim.enabled = false;
            isStart = true;
        }

        if (!isStart) return;

        //회전
        if (rigid.velocity.y < -4 && rigid.velocity.y > -7)
                transform.Rotate(0, 0, -6f);

        //플레이어가 화면 밖으로 나가지 않도록 이동 제한
        if (transform.position.y > 4.8f)
            rigid.position = new Vector2(-1.2f, 4.8f);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //바닥에 닿을 시에 게임오버와 물리적 요소 제거
        if (collision.gameObject.tag == "Floor")
        {
            gm.GameOver();
            rigid.simulated = false;
        }

        //기둥에 닿을 시에 게임오버
        if (collision.gameObject.tag == "Pillar")
            gm.GameOver();        
    }

}
