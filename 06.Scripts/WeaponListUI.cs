using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponListUI : MonoBehaviour
{
    [SerializeField] List<Image> weaponSlotList = new List<Image>();

    public void UpdateWeaponIcon(WeaponController weaponController)
    {
        //所持している武器リストを取得
        var dataList = weaponController.DataList;

        //所持している武器の数で繰り返し処理を行う
        for(int i = 0; i < dataList.Count; i++)
        {
            //「i」番目の武器データのアイコン画像を取得する
            Sprite icon = dataList[i].Icon;

            //「i」番目のUI画像を武器アイコン画像に変更する
            weaponSlotList[i].sprite = icon;
        }
    }
}
