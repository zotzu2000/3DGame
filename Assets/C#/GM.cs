using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using OfficeOpenXml;
using System.IO;
using System.Data;
using Excel;
using UnityEditor;

public class GM : MonoBehaviour
{
    #region 生成怪物
    [Header("怪物事件")]
    public GameObject NPC;
    [Header("產生怪物的方塊")]
    public GameObject CreatePos;
    [Header("Npc多久產出")]
    public float WaitTime;
    [Header("每個關卡Npc的最多數量")]
    public float MaxNum;
    // 目前在場景已經產出多少數量
    int Num;
    [Header("King物件")]
    public GameObject King;
    // NPC死亡的數量
    public float DeadNum;
    #endregion
    #region 遊戲暫停畫面
    public GameObject PauseObject;

    #endregion
    #region 怪物死亡條
    public Image MonsterBar;
    #endregion
    #region 信仰條
    [Header("放入信仰條UI圖")]
    public Image MagicBar;
    [Header("設定信仰條全滿的時間")]
    public float MagicTime;
    // 在程式中計算法術條時間
    public float MagicTimeScript;
    #endregion
    #region Level
    [Header("輸入目前的關卡")]
    public string LevelIDString;
    //抓取關卡字串轉成整數值
    int LevelID;
    [Header("關卡個位數與十位數Image位置")]
    public Image[] LevelImage;
    [Header("0~9數字圖片")]
    public Sprite[] NumberSprite;
    #endregion
    #region 遊戲分數
    //計算玩家的得分
    int TotalScore;
    //儲存玩家的得分
    string SaveTotalScore = "SaveTotalScore";
    //將玩家的得分轉成字串
    string TotalScoreString;
    [Header("放入要動態生成數字的物件")]
    public GameObject ScoreObject;
    [Header("生成數字的父物件")]
    public GameObject ScoreGridObject;
    [Header("抓取所有分數的圖片")]
    public List<Image> ScoreImage;
    #endregion
    #region 遊戲結束
    //isWin=true代表勝利 false失敗
    public bool isWin;
    [Header("遊戲結束的UI物件")]
    public GameObject GameOverUI;
    [Header("遊戲結束的勝利失敗圖")]
    public Sprite WinSprite, LoseSprite;
    [Header("遊戲結束的勝利失敗Image")]
    public Image WinSprie;
    [Header("關卡個位數和十位數的Image")]
    public Image[] GameOverLevelImage;
    //獎勵分數
    string RewardScoreString;
    int GameOverScore;
    string GameOverScoreString;
    [Header("放入要動態生成數字的物件-獎勵")]
    public GameObject RewardScoreObject;
    [Header("生成數字的父物件-獎勵")]
    public GameObject RewardScoreGridObject;
    [Header("放入要動態生成數字的物件-遊戲結束總分")]
    public GameObject GameOverScoreObject;
    [Header("生成數字的父物件-遊戲結束總分")]
    public GameObject GameOverScoreGridObject;
    public List<Image> RewardScoreImage;
    public List<Image> GameOverScoreImage;
    #endregion
    #region 遊戲結束的按鈕
    public Button NextGameButton;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // 恢復整體遊戲時間
        Time.timeScale = 1;
        InvokeRepeating("CreateNpc", WaitTime, WaitTime);

        #region 第一種-計算Level數值與顯示
        //將字串轉化成整數值
        LevelID = int.Parse(LevelIDString);
        //十位數
        int a = LevelID / 10;
        //個位數
        int b = LevelID % 10;
        //將個位數的圖片帶入個位數欄位
        LevelImage[0].sprite = NumberSprite[b];
        //將十位數的圖片帶入十位數欄位
        LevelImage[1].sprite = NumberSprite[a];

        #endregion
        #region 第二種-計算Level數值與顯示
        /*//將字串裡面的數值切割(用底線_區隔)直接存在字串陣列
        string[] TotalLevelIDString = LevelIDString.Split('_');
        //將字串陣列裡面的文字轉成數值帶入圖片
        LevelImage[0].sprite = NumberSprite[int.Parse(TotalLevelIDString[1])];
        LevelImage[1].sprite = NumberSprite[int.Parse(TotalLevelIDString[0])];*/
        #endregion
        #region 第三種-計算Level數值與顯示
        /*LevelImage[0].sprite = NumberSprite[int.Parse(LevelIDString.Substring(1,1))];
        LevelImage[1].sprite = NumberSprite[int.Parse(LevelIDString.Substring(0,1))];
        print(int.Parse(LevelIDString.Substring(0, 1)));
        print(int.Parse(LevelIDString.Substring(0, 2)));
        print(int.Parse(LevelIDString.Substring(1, 1)));
        print(int.Parse(LevelIDString.Substring(1, 2)));*/
        #endregion
        //遊戲一開始沒有任何加總分
        FinalScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        // 程式中計算法術條時間一直累加
        MagicTimeScript += Time.deltaTime;
        // 程式中計算的法術條時間介於0~MagicTime之間
        MagicTimeScript = Mathf.Clamp(MagicTimeScript, 0, MagicTime);
        // 將法術條的時間反應在MagicBar上
        MagicBar.fillAmount = MagicTimeScript / MagicTime;
    }

    void CreateNpc()
    {
        if(Num < MaxNum)
        {
            // 抓取Collider三維座標最大值
            Vector3 MaxValue = CreatePos.GetComponent<Collider>().bounds.max;
            // 抓取Collider三維座標最小值
            Vector3 MinValue = CreatePos.GetComponent<Collider>().bounds.min;
            // 隨機抓取NPC要產生的位置點
            Vector3 RandomPox = new Vector3(Random.Range(MinValue.x, MaxValue.x), MinValue.y, MinValue.z);
            // 動態生成NPC
            Instantiate(NPC, RandomPox, CreatePos.transform.rotation);
            // 生成後數量+1
            Num++;
        }

        // 如果npc死亡數量=每個關卡Npc的最多數量
        // 場景上沒有任何一隻King
        if(DeadNum == MaxNum && GameObject.FindGameObjectsWithTag("King").Length <= 0)
        {
            Instantiate(King, CreatePos.transform.position, CreatePos.transform.rotation);
        }
    }
    public void DeadCount()
    {
        DeadNum++;
        MonsterBar.fillAmount = (MaxNum - DeadNum) / MaxNum;
    }

    // 遊戲暫停畫面
    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseObject.SetActive(true);
    }
    // 恢復遊戲畫面
    public void ReturnGame()
    {
        // 恢復整體遊戲時間
        Time.timeScale = 1;
        PauseObject.SetActive(false);
    }
    // 回首頁
    public void BackMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    //怪物或Boss死亡要顯示分數
    public void FinalScore(int AddScore)
    {
        //怪物或Boss死亡，加分
        TotalScore += AddScore;
        //Debug.Log(TotalScore);
        //總分整數值轉化為字串
        TotalScoreString = TotalScore + "";
        //儲存總分數
        PlayerPrefs.SetInt(SaveTotalScore, TotalScore);
        //抓取字串總共有多少字數量=字串.Length
        //看List總數量在生成對應的Image數量在空物件下
        for(int i = ScoreImage.Count; i < TotalScoreString.Length; i++)
        {
            //動態生成Image
            GameObject ScoreObjectPrefab = Instantiate(ScoreObject) as GameObject;
            //將生成出來Image移動到空物件階層下
            ScoreObjectPrefab.transform.parent = ScoreGridObject.transform;
            //抓取動態生成物件的Image存放在list
            ScoreImage.Add(ScoreObjectPrefab.GetComponent<Image>());
        }
        //將每個Image帶入數字圖片
        for(int spritID = 0; spritID < TotalScoreString.Length; spritID++)
        {
            ScoreImage[spritID].sprite = NumberSprite[int.Parse(TotalScoreString.Substring(spritID, 1))];
        }
    }
    //Boss/玩家死亡
    public void GameOver(int RewardScore)
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
        if (isWin)
        {
            WinSprie.sprite = WinSprite;
            NextGameButton.interactable = true;
        }
        else
        {
            WinSprie.sprite = LoseSprite;
            NextGameButton.interactable = false;
        }
        //遊戲結束Level
        GameOverLevelImage[0].sprite = NumberSprite[int.Parse(LevelIDString.Substring(1, 1))];
        GameOverLevelImage[1].sprite = NumberSprite[int.Parse(LevelIDString.Substring(0, 1))];
        //遊戲結束總分=遊戲分數+獎勵分數;
        GameOverScore = TotalScore + RewardScore;
        GameOverScoreString = GameOverScore + "";
        RewardScoreString = RewardScore + "";      
        for (int i = RewardScoreImage.Count; i < RewardScoreString.Length; i++)
        {
            //動態生成Image
            GameObject RewardScorePrefab = Instantiate(ScoreObject) as GameObject;
            //將生成出來Image移動到空物件階層下
            RewardScorePrefab.transform.parent = RewardScoreGridObject.transform;
            //抓取動態生成物件的Image存放在list
            RewardScoreImage.Add(RewardScorePrefab.GetComponent<Image>());
        }
        //將每個Image帶入數字圖片
        for (int spritID = 0; spritID < RewardScoreString.Length; spritID++)
        {
            RewardScoreImage[spritID].sprite = NumberSprite[int.Parse(RewardScoreString.Substring(spritID, 1))];
        }
        for (int i_g = GameOverScoreImage.Count; i_g < GameOverScoreString.Length; i_g++)
        {
            //動態生成Image
            GameObject GameOverScorePrefab = Instantiate(ScoreObject) as GameObject;
            //將生成出來Image移動到空物件階層下
            GameOverScorePrefab.transform.parent = GameOverScoreGridObject.transform;
            //抓取動態生成物件的Image存放在list
            GameOverScoreImage.Add(GameOverScorePrefab.GetComponent<Image>());
        }
        //將每個Image帶入數字圖片
        for (int spritID_g = 0; spritID_g < GameOverScoreString.Length; spritID_g++)
        {
            GameOverScoreImage[spritID_g].sprite = NumberSprite[int.Parse(GameOverScoreString.Substring(spritID_g, 1))];
        }
        //將資料寫入至Excel表單
        ExcelWritter.ansList.Add(LevelIDString);
        ExcelWritter.ansList.Add(GameOverScoreString);
        //寫入Excel檔名與表單名稱
        ExcelWritter.WriteExcel("SaveData", "Data");
    }
    //重新遊戲
    public void Regame()
    {
        SceneManager.LoadScene("Game");
    }
    //下一關
    public void Nextgame()
    {
        SceneManager.LoadScene("Game");
    }
}