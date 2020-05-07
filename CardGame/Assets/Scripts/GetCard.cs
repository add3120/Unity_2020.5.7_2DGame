using UnityEngine;
using UnityEngine.Networking;  // 引用 網路連線 API
using System.Collections;

public class GetCard : MonoBehaviour
{
    private IEnumerator GetCardData()
    {
        // 引用 (網路要求 www = 網路要求.Post("網址", ""))
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxQnC1WpDe1Zm-Zl5d2LofGji8okAl3fwhZO7nmZhw0ASqNcvE/exec", ""))
        {
            // 等待 網路要求時間
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                print("連線錯誤 :" + www.error);
            }
            else
            {
                print(www.downloadHandler.text);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(GetCardData());
    }
}
