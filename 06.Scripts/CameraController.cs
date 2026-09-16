using UnityEngine;

/// <summary>
/// シーン上のプレイヤーを追従するようなカメラ
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] PlayerManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerManagerが「Player」の情報を持っていれば
        if(playerManager.Player != null)
        {
            //プレイヤーの座標を取得
            Vector3 targetPos = playerManager.Player.transform.position;

            //カメラの位置を追従対象から引いて映す為にZ軸の座標を調整
            Vector3 offset = new Vector3(0f, 0f, -10f);

            //プレイヤーを追跡するようにカメラの座標を更新
            cam.transform.position = targetPos + offset;
        }
    }
}
