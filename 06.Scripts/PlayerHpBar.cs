using UnityEngine;
using UnityEngine.UI;   //UI関連のコンポーネントを操作するために必要

public class PlayerHpBar : MonoBehaviour
{
    [Header("バーとなるスライダー"), SerializeField] Slider hpSlider;

    /// <summary>
    /// HPの表示を設定する
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    public void SetHpValue(float currentHp, float maxHp)
    {
        //最大HPに対しての現在のHPの割合を取得する
        float ratio = currentHp / maxHp;

        //スライダーの表示が0～1fの数字の間で変化するため、割合の値をそのまま設定
        hpSlider.value = ratio;
    }
}
