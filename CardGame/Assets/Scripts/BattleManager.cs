﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// 對戰管理器實體物件
    /// </summary>
    public static BattleManager instance;

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;
    [Header("畫布")]
    public Transform canvas;
    [Header("手牌區域")]
    public Transform handArea;
    [Header("水晶"), Tooltip("水晶圖片，用來顯示的 10 張")]
    public GameObject[] crystalObject;
    [Header("水晶數量介面")]
    public Text textCrystal;
    [Header("擲金幣畫面")]
    public GameObject coinView;
    [Header("手牌卡牌資訊")]
    /// <summary>
    /// 對戰用牌組 : 手牌
    /// </summary>
    public List<CardData> battleDeck = new List<CardData>();
    [Header("手牌卡牌遊戲物件")]
    public List<GameObject> handGameObject = new List<GameObject>();

    /// <summary>
    /// 先後攻
    /// true 先
    /// false 後
    /// </summary>
    private bool firstAttack;

    private bool myTurn;
    private int crystalTotal;

    /// <summary>
    /// 水晶數量
    /// </summary>
    public int crystal;

    private void Start()
    {
        instance = this;
    }
    /// <summary>
    /// 我方結束
    /// </summary>
    public void EndTurn()
    {
        myTurn = false;
    }
    /// <summary>
    /// 對方結束 : 水晶 +1
    /// </summary>
    public void StartTurn()
    {
        myTurn = true;
        crystalTotal++;
        crystalTotal = Mathf.Clamp(crystalTotal, 1, 10);   // 夾住最大水晶數量
        crystal = crystalTotal;
        Crystal();
        StartCoroutine(GetCard(1));

    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartBattle()
    {
        gameView.SetActive(true);     // 顯示遊戲畫面

        ThrowCoin();
    }

    /// <summary>
    /// 擲金幣
    /// </summary>
    private void ThrowCoin()
    {
        coin.AddForce(0, Random.Range(200, 500), 0);     // 推力
        coin.AddTorque(Random.Range(200, 500), 0, 0);    // 旋轉

        Invoke("CheckCoin", 3);                          // 延遲呼叫檢查方法
    }

    /// <summary>
    /// 檢查金幣正反面
    /// rotation.x 為 -1 - 背面
    /// rotation.x 為 0  - 正面
    /// </summary>
    private void CheckCoin()
    {
        // 三元運算子
        // 先後攻 = 布林運算 ? 成立 : 不成立
        // 錢號 Y > 0.25f 正面
        //print(coin.transform.GetChild(0).position.y);

        firstAttack = coin.transform.GetChild(0).position.y > 0.25f ? true : false;

        coinView.SetActive(false);      // 隱藏金幣畫面

        // 如果 先攻 水晶 1 顆，卡牌 4 張
        int card = 3;

        if (firstAttack)
        {
            crystal = 1;
            card = 4;
        }

        Crystal();

        StartCoroutine(GetCard(card));
    }
    /// <summary>
    /// 處理水晶數量
    /// </summary>
    private void Crystal()
    {
        // 顯示目前有幾顆水晶
        for (int i = 0; i < crystal; i++)
        {
            crystalObject[i].SetActive(true);
        }

        textCrystal.text = crystal + " /10";
    }

    /// <summary>
    /// 更新水晶介面與圖片
    /// </summary>
    public void UpdateCrystal()
    {
        for (int i = 0; i < crystalObject.Length; i++)
        {
            if (i < crystal) continue;       // 如果 迴圈編號 < 目前水晶數量 就繼續 (跳過此次)

            crystalObject[i].SetActive(false);
        }

        textCrystal.text = crystal + " /10";
    }

    /// <summary>
    /// 抽牌組卡牌到手上牌組
    /// </summary>
    private IEnumerator GetCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 抽牌組第一張 放到 手牌 第一張
            battleDeck.Add(DeckManager.instance.deck[0]);
            // 刪除 牌組第一張
            DeckManager.instance.deck.RemoveAt(0);
            // 抽牌組第一張卡牌物件 放到 手牌 第一張
            handGameObject.Add(DeckManager.instance.deckGameObject[0]);
            // 刪除 牌組第一張遊戲物件
            DeckManager.instance.deckGameObject.RemoveAt(0);

            // 等待協程執行結束
            yield return StartCoroutine(MoveCard());
        }
    }

    /// <summary>
    /// 手牌數量
    /// </summary>
    private int handCardCount;

    /// <summary>
    /// 顯示卡牌再移動到手上
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCard()
    {
        RectTransform card = handGameObject[handGameObject.Count - 1].GetComponent<RectTransform>();     // 取得手牌最後一張[數量 -1]

        // 進入右手邊中間位置
        card.SetParent(canvas);                    // 將父物件設為畫布
        card.anchorMin = Vector2.one * 0.5f;       // 設定中心點
        card.anchorMax = Vector2.one * 0.5f;       // 設定中心點

        while (card.anchoredPosition.x > 501)      // 當 X > 500 執行移動
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 0), 0.5f * Time.deltaTime * 50);  // 卡片位置前往 500, 0

            yield return null;                     // 等待一個影格
        }

        yield return new WaitForSeconds(0.35f);    // 停留 0.35秒

        if (handCardCount == 10)
        {
            print("爆掉手牌");
        }
        else
        {
            // 進入手牌
            card.localScale = Vector3.one * 0.5f;      // 縮小

            while (card.anchoredPosition.y > -274)     // 當 Y > -275 執行移動
            {
                card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -275), 0.5f * Time.deltaTime * 50);  // 卡片位置前往 500, 0

                yield return null;                     // 等待一個影格
            }

            card.SetParent(handArea);                  // 設定父物件為手牌區域
            card.gameObject.AddComponent<HandCard>();  // 添加手牌腳本 - 可拖拉
            handCardCount++;                           // 手牌數量遞增
        }
    }
}
