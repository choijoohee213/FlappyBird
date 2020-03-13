using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            gameObject.SetActive(false);
        }
    }
}
