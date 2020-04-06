using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float curSpawnDelay;
    public float nextSpawnDelay;
    bool tryStart = true;
    public int score;
    int randomIndexA;
    int randomIndexB;
    
    public Player player;
    public ObjectManager om;
    public ScoreZone scoreZone;
    public Floor floor;
    public Animator[] anim;
    public AudioSource[] audio;
    public Text scoreText;
    public Text finalScoreText;
    public Text BestScoreText;
    public Text[] scoreData;

    public GameObject newScore;
    public GameObject beforeStartUI;
    public Image medalUI;
    public GameObject[] players;
    public GameObject[] backgroundSprites;
    public Sprite[] medalSprites;


    void Awake()
    {
        scoreZone.gm = this;

        //시작할 때 Fade In 애니메이션
        anim[3].SetTrigger("FadeIn");

        //캐릭터와 배경 랜덤 구현
        if (PlayerPrefs.GetInt("isReset") == 1)
        {
            randomIndexA = PlayerPrefs.GetInt("randomPlayer");
            randomIndexB = PlayerPrefs.GetInt("randomBg");
            PlayerPrefs.SetInt("isReset", 0);
        }
        else
        {
            randomIndexA = Random.Range(0, 3);
            randomIndexB = Random.Range(0, 2);
        }
        players[randomIndexA].SetActive(true);
        backgroundSprites[randomIndexB].SetActive(true);

        player = players[randomIndexA].GetComponent<Player>();
        floor.player = player;
        
    }


    private void Update()
    {
        if (player.isStart)  //플레이어가 게임을 시작했으면
        {
            //기둥을 일정시간 간격으로 스폰함
            curSpawnDelay += Time.deltaTime;
            if (curSpawnDelay > nextSpawnDelay && !player.isDie)
            //스폰할 일정시간이 지났고 플레이어가 죽지않았다는 조건 하에
            {
                SpawnPillar();
                curSpawnDelay = 0;
            }

            //터치를 시작하면 처음 UI가 투명해지는 애니메이션 실행
            if (tryStart)
                anim[1].SetTrigger("BeforeStartFade");
            tryStart = false;
        }
       
    }


    void SpawnPillar()  //기둥을 일정 위치에 일정 속도로 움직이도록 스폰함
    {
        float randomIndex = Random.Range(4f,8.1f);
        Vector2 moveDir = new Vector2(-2.3f, 0);

        GameObject pillarUp = om.MakeObj("pillarUp");  //윗 기둥
        GameObject pillarDown = om.MakeObj("pillarDown");  //아랫 기둥
        GameObject scoreZone = om.MakeObj("scoreZone");  //점수 획득 존

        //기둥 스폰 위치
        pillarUp.transform.position = new Vector3(3.5f, randomIndex);
        pillarDown.transform.position = new Vector3(3.5f, randomIndex - 9.82f);
        scoreZone.transform.position = new Vector3(3.5f, randomIndex - 4.9f);

        Rigidbody2D rigidU = pillarUp.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidD = pillarDown.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidS = scoreZone.GetComponent<Rigidbody2D>();

        Pillar pillarULogic = pillarUp.GetComponent<Pillar>();
        Pillar pillarDLogic = pillarDown.GetComponent<Pillar>();
        ScoreZone scoreLogic = scoreZone.GetComponent<ScoreZone>();

        //기둥과 점수획득 존을 왼쪽으로 일정하게 움직이도록 함
        rigidU.velocity = moveDir;
        rigidD.velocity = moveDir;
        rigidS.velocity = moveDir;
    }


    public void ScoreUpdate() 
    {
        //UI의 Text에 현재 점수를 업데이트함
        scoreText.text = score.ToString();
    }


    public void GameOver()
    {
        //한번만 실행
        if (!player.isDie)
        {
            anim[0].SetTrigger("GameOver");  //화면 반짝이는 애니메이션
            anim[2].SetTrigger("GameOverPanel");  //게임오버 panel 애니메이션
            audio[1].Play(); //hit 효과음 
            Invoke("DieAudio", 1.8f);

            //점수 데이터 저장, 로드
            Save();
            Load();
            
            //게임오버 UI       
            scoreText.enabled = false;
            finalScoreText.text = score.ToString();
            BestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
            MedalDecide();
            if (score == PlayerPrefs.GetInt("BestScore")) newScore.SetActive(true);  //최고 점수일 경우

            //모든 오브젝트의 움직임을 멈춤
            om.StopObject("pillarUp");
            om.StopObject("pillarDown");
            om.StopObject("scoreZone");

            player.isStart = false;
            player.isDie = true;
            player.transform.rotation = Quaternion.Euler(0, 0, -90);
        }    
    }

    void DieAudio()
    {
        audio[0].Play();
    }

    void MedalDecide()
    {
        if (score >= 7) medalUI.sprite = medalSprites[0];
        if (score >= 10) medalUI.sprite = medalSprites[1];
        if (score >= 20) medalUI.sprite = medalSprites[2];
        if (score >= 30) medalUI.sprite = medalSprites[3];
    }

    public void Retry() //게임을 다시 시작함
    {
        SceneManager.LoadScene(1);
    }

    public void Save()  //점수 데이터를 저장함. 1등, 2등, 3등
    {
        if (score < PlayerPrefs.GetInt("BestScore")) {            
            if (score < PlayerPrefs.GetInt("SecondScore"))
            {               
                if (score < PlayerPrefs.GetInt("ThirdScore"))
                    return;               
                PlayerPrefs.SetInt("ThirdScore", score);
                return;
            }
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetInt("SecondScore", score);
            return;
        }
        if (score == PlayerPrefs.GetInt("BestScore")) return;
        PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
        PlayerPrefs.SetInt("SecondScore", PlayerPrefs.GetInt("BestScore"));
        PlayerPrefs.SetInt("BestScore", score);
    }

    private void Load() //랭킹 Panel의 text에 점수를 로드
    {
        scoreData[0].text = PlayerPrefs.GetInt("BestScore").ToString() + "점";
        scoreData[1].text = PlayerPrefs.GetInt("SecondScore").ToString() + "점";
        scoreData[2].text = PlayerPrefs.GetInt("ThirdScore").ToString() + "점";
    }

}
