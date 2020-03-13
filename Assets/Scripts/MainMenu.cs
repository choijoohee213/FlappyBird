using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int randomIndexA;
    public int randomIndexB;
    public Text[] scoreData;
    public GameObject[] players;
    public GameObject[] backgroundSprites;

    private void Start()
    {
        //플레이어와 배경의 sprite가 랜덤으로 적용되어 시작
        randomIndexA = Random.RandomRange(0, 3);
        randomIndexB = Random.RandomRange(0, 2);

        players[randomIndexA].SetActive(true);
        backgroundSprites[randomIndexB].SetActive(true);

        PlayerPrefs.SetInt("randomPlayer", randomIndexA);
        PlayerPrefs.SetInt("randomBg", randomIndexB);
        PlayerPrefs.SetInt("isReset", 1);
        
        //점수 데이터 로드
        Load();
    }


    public void Retry() //게임을 다시 시작함
    {
        SceneManager.LoadScene(1);
    }

    private void Load() //랭킹 Panel의 text에 점수를 로드
    {
        scoreData[0].text = PlayerPrefs.GetInt("BestScore").ToString() + "점";
        scoreData[1].text = PlayerPrefs.GetInt("SecondScore").ToString() + "점";
        scoreData[2].text = PlayerPrefs.GetInt("ThirdScore").ToString() + "점";
    }


}
