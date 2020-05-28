using UnityEngine;
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

    /// <summary>
    /// 先後攻
    /// true 先
    /// false 後
    /// </summary>
    private bool firstAttack;

    /// <summary>
    /// 對戰用牌組 : 手牌
    /// </summary>
    public List<CardData> battleDeck = new List<CardData>();

    private void Start()
    {
        instance = this;
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
        coin.AddForce(0, Random.Range(300, 500), 0);     // 推力
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
        firstAttack = coin.rotation.x < 0 ? false : true;

        print("先後攻 :" + firstAttack);

        GetCard();
    }

    /// <summary>
    /// 抽牌組卡牌到手上牌組
    /// </summary>
    private void GetCard()
    {
       // 抽牌組第一張 放到 手牌 第一張
       battleDeck.Add(DeckManager.instance.deck[0]);
       // 刪除 牌組第一張
       DeckManager.instance.deck.RemoveAt(0);
    }
}
