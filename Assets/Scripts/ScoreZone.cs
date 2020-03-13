using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public GameManager gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
        }

        if(collision.gameObject.tag == "Player")
        {
            //점수 증가
            gm.score++;

            //점수 UI 업데이트
            gm.ScoreUpdate();

            //Sound
            gm.audio[2].Play();
        }
    }
}
