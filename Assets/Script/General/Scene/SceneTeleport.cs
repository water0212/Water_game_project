using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Vector2 _TeleportPosition;
    [SerializeField]private SceneRefenceSO Scene;
    /// <summary>
    /// 準備加載場景並傳送
    /// </summary>
    /// <param name="isActivePosition">是否使用傳入的傳送座標</param>
    /// <param name="position">傳送座標</param>
    public void TeleportStart(bool isActivePosition, Vector2 position){
        if(isActivePosition){
            //TODO:加載場景並傳送
            SceneLaodManager.LoadScene(Scene, position);
        }else{
            SceneLaodManager.LoadScene(Scene, _TeleportPosition);
        }
    }
}
