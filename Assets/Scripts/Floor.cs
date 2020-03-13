using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float speed;
    public Player player;

    //바닥을 동적으로 표현하여 생동감있는 배경을 표현
    //플레이어가 마치 오른쪽으로 움직이는 것처럼 보임
    void Update()
    {
        if (player.isDie) return;

        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (transform.position.x < -0.4)
            transform.position = new Vector3(0, transform.position.y,0);
     
        
    }
}
