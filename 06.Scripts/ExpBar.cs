using UnityEngine;
using UnityEngine.UI;   //UI関連のコンポーネントを操作するために必要

public class ExpBar : MonoBehaviour
{
    [Header("バーとなるスライダー"), SerializeField] Slider expSlider;

    /// <summary>
    /// 獲得経験値の表示を設定する
    /// </summary>
    /// <param name="currentExp">現在の経験値</param>
    /// <param name="requiredExp">レベルアップに必要な経験値</param>
    public void SetExpValue(int currentExp, int requiredExp)
    {
        //レベルアップに必要な経験値に対しての現在の経験値の割合を取得する
        //int型同士だと小数点以下が計算できず、割合にならないためfloat型に型変換して計算する
        float ratio = (float)currentExp / (float)requiredExp;

        //スライダーの表示が0～1fの数字の間で変化するため、割合の値をそのまま設定
        expSlider.value = ratio;
    }
}
