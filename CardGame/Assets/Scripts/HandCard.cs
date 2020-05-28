using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 手牌 : 選取後拖拉
/// </summary>
/// IBeginDragHandler 開始拖拉
/// IDragHandler      拖拉中
/// IEndDragHandler   拖拉結束
public class HandCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 原始座標
    /// </summary>
    private Vector3 original;

    /// <summary>
    /// 開始拖拉
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 原始做標 = 卡牌.座標
        original = transform.position;
    }

    /// <summary>
    /// 拖拉中
    /// </summary>
    /// <param name="eventData">拖拉資訊</param>
    public void OnDrag(PointerEventData eventData)
    {
        // 此卡牌.座標 = 拖拉.座標
        transform.position = eventData.position;
    }

    /// <summary>
    /// 結束拖拉
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 卡牌.座標 = 原始座標
        transform.position = original;
    }
}
