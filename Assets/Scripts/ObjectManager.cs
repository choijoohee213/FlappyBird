using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //프리펩
    public GameObject pillarUpPrefab;
    public GameObject pillarDownPrefab;
    public GameObject scoreZonePrefab;

    GameObject[] pillarUp;
    GameObject[] pillarDown;
    GameObject[] scoreZone;
    GameObject[] targetPool;

    void Awake()
    {
        pillarUp = new GameObject[3];
        pillarDown = new GameObject[3];
        scoreZone = new GameObject[3];
        Generate();
    }

    //Instantiate로 생성 후 오브젝트 비활성화 해둠
    void Generate()
    {
        for(int i=0; i< pillarUp.Length; i++)
        {
            pillarUp[i] = Instantiate(pillarUpPrefab);
            pillarUp[i].SetActive(false);
        }

        for (int i = 0; i < pillarDown.Length; i++)
        {
            pillarDown[i] = Instantiate(pillarDownPrefab);
            pillarDown[i].SetActive(false);
        }

        for (int i = 0; i < scoreZone.Length; i++)
        {
            scoreZone[i] = Instantiate(scoreZonePrefab);
            scoreZone[i].SetActive(false);
        }
    }

    //원하는 오브젝트를 활성화시킴
    public GameObject MakeObj(string name)
    {
        switch (name)
        {
            case "pillarUp":
                targetPool = pillarUp;
                break;
            case "pillarDown":
                targetPool = pillarDown;
                break;
            case "scoreZone":
                targetPool = scoreZone;
                break;
        }

        for(int i=0; i<targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }

    //원하는 오브젝트를 return함
    public GameObject[] GetPool(string name)
    {
        switch (name)
        {
            case "pillarUp":
                targetPool = pillarUp;
                break;
            case "pillarDown":
                targetPool = pillarDown;
                break;
            case "scoreZone":
                targetPool = scoreZone;
                break;
        }

        return targetPool;
    }

    public void StopObject(string name)
    {
        switch (name)
        {
            case "pillarUp":
                targetPool = pillarUp;
                break;
            case "pillarDown":
                targetPool = pillarDown;
                break;
            case "scoreZone":
                targetPool = scoreZone;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (targetPool[i].activeSelf)
            {
                Rigidbody2D rigid;
                rigid = targetPool[i].GetComponent<Rigidbody2D>();
                rigid.velocity = new Vector2(0,0);
            }
        }

    }
}
