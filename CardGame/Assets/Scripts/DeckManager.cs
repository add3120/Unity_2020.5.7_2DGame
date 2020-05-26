using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;  // 系統.集合.一般

public class DeckManager : MonoBehaviour
{
    [Header("牌組卡牌資訊")]
    public GameObject DeckObject;
    [Header("牌組內容")]
    public Transform contentDeck;
    [Header("牌組卡牌數量")]
    public Text textDeckCount;
    [Header("開始遊戲按鈕")]
    public Button btnStart;

    /// <summary>
    /// 牌組清單
    /// </summary>
    public List<CardData> deck = new List<CardData>();

    /// <summary>
    /// 牌組管理器實體物件
    /// </summary>
    public static DeckManager instance;

    private void Awake()
    {
        // 牌組管理器實體物件 = 此腳本
        instance = this;
        // 取消開始遊戲按鈕 互動
        btnStart.interactable = false;
    }

    /// <summary>
    /// 添加卡牌至排組內
    /// </summary>
    /// <param name="index">要添加到牌組的卡牌編號</param>
    public void AddCard(int index)
    {
        // 如果 牌組.數量 < 30 - List 長度 Count
        if (deck.Count < 30)
        {
            // 選取的卡牌
            CardData card = GetCard.instance.cards[index - 1];

            // => 黏巴達 (Lambda C# 7) 新版符號
            // 牌組.尋找全部(卡牌 => 卡牌.等於(選取的卡牌))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(card));
            
            // 如果 相同卡牌 < 2 才能新增
            if (sameCard.Count < 2)
            {
                // 牌組.添加卡牌(選取的卡牌)
                deck.Add(card);

                Transform temp;

                if (sameCard.Count < 1)
                {
                    // 生成 牌組卡牌資訊物件 到 牌組內容
                    temp = Instantiate(DeckObject, contentDeck).transform;
                    // 添加牌組物件腳本，讓按鈕有功能
                    temp.gameObject.AddComponent<DeckObject>().index = card.index;
                    temp.name = "牌組卡牌資訊 : " + card.name;
                }
                else
                {
                    temp = GameObject.Find("牌組卡牌資訊 : " + card.name).transform;
                }

                // 更新牌組卡牌資訊
                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
                temp.Find("名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();
                // 更新卡牌數量
                textDeckCount.text = "卡牌數量 :" + deck.Count + " / 30";
            }            
        }

        // 如果卡牌 等於 30 張 啟動開始遊戲按鈕 互動
        if(deck.Count == 30) btnStart.interactable = true;
    }

    /// <summary>
    /// 刪除牌組內的卡牌
    /// </summary>
    /// <param name="index">要從排組內刪除的卡牌編號</param>
    public void DeleteCard(int index)
    {
        // 選取的卡牌
        CardData card = GetCard.instance.cards[index - 1];

        // 牌組.尋找全部(卡牌 => 卡牌.等於(選取的卡牌))
        List<CardData> sameCard = deck.FindAll(c => c.Equals(card));

        // 牌組.刪除(卡牌)
        deck.Remove(card);

        // 牌組物件
        Transform temp = GameObject.Find("牌組卡牌資訊 : " + card.name).transform;

        // 相同卡牌 > 1
        if (sameCard.Count > 1)
        {
            // 更新 牌組物件 數量
            temp.Find("數量").GetComponent<Text>().text = (sameCard.Count - 1).ToString();
        }
        // 相同卡牌 < 1
        else
        {
            // 刪除 牌組物件
            Destroy(temp.gameObject);
        }

        textDeckCount.text = "卡牌數量 :" + deck.Count + " / 30";

        // 取消開始遊戲按鈕 互動
        btnStart.interactable = false;
    }
}
