using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamaeraControl : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    private CinemachineConfiner2D Confiner2D;
    public CinemachineImpulseSource impulseSource;
    private static CamaeraControl instance;
     private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Found more than one AttackScene in the Scene");
        }
        instance = this;   
    }
    public static CamaeraControl GetInstance() {
        return instance;
    }
    private void Start() {
        GetNewBound();
    }
    private void GetNewBound(){
        currentCamera = CameraManager.instence.currentCamera;
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null){
            Debug.Log("沒有找到地形限制");
            return;
        }
        if(obj.GetComponent<Collider2D>() != null){
            CinemachineConfiner2D Confiner2D = currentCamera.GetComponent<CinemachineConfiner2D>();
            Confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
            Confiner2D.InvalidateCache();
        }
        
        
    }
    public void CameraShake(Vector2 AttackDisplace){
        impulseSource.m_DefaultVelocity = AttackDisplace/40;
        impulseSource.GenerateImpulse();
    }
}
