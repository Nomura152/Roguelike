using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour
{
    [Header("移動先のシーン名"), SerializeField] string loadSceneName;

    /// <summary>
    /// ボタンに設定するシーン読み込みメソッド
    /// </summary>
    public void LoadScene()
    {
        //対象のシーンをロードする
        SceneManager.LoadScene(loadSceneName);
    }
}
