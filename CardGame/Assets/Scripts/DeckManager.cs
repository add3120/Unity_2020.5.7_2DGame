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
        instance = this;
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
            // 尋找要增加卡牌在清單內的資料
            // => 黏巴達 (Lambda C# 7)
            // 相同卡牌 = 牌組.尋找全部(卡牌 => 卡牌.等於(目前點選的卡牌資訊))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(GetCard.instance.cards[index - 1]));
            
            // 如果 相同卡牌 < 2 才能新增
            if (sameCard.Count < 2)
            {
                // 牌組.增加(取得卡牌.實體物件.卡牌資料[編號])
                deck.Add(GetCard.instance.cards[index - 1]);

                // 取得卡牌資訊
                CardData card = GetCard.instance.cards[index - 1];

                Transform temp;

                if (sameCard.Count < 1)
                {
                    // 生成 牌組卡牌資訊物件 到 牌組內容
                    temp = Instantiate(DeckObject, contentDeck).transform;
                    temp.name = "牌組卡牌資訊 : " + card.name;
                }
                else
                {
                    temp = GameObject.Find("牌組卡牌資訊 : " + card.name).transform;
                }

                // 更新卡牌數量
                textDeckCount.text = "卡牌數量 :" + deck.Count + " / 30";
                // 更新牌組卡牌資訊
                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
                temp.Find("名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();
            }            
        }
    }

    /// <summary>
    /// 刪除牌組內的卡牌
    /// </summary>
    /// <param name="index">要從排組內刪除的卡牌編號</param>
    public void DeleteCard(int index)
    {
        
    }
}
