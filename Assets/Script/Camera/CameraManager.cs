using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instence;

    [SerializeField] private CinemachineVirtualCamera[] _allVirualCamera;

    [Header("控制跳躍與墜落時的Y軸")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping {get ; private set; }
    public bool LerpedFromPlayerFalling {get ; set ;}

    private Coroutine _leapYPanCorutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;

    private void Awake(){
        if(instence == null){
            instence = this;
        }else {
            Debug.LogWarning("有多個CameraManager");
        }
        for(int i = 0; i< _allVirualCamera.Length; i++){
            if( _allVirualCamera[i].enabled){
                currentCamera = _allVirualCamera[i];
            }

            _framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #region 對Y軸的阻力
        public void LerpYDamping(bool isPlayerFalling){
            _leapYPanCorutine = StartCoroutine(LerpYAction(isPlayerFalling));
        }
        private IEnumerator LerpYAction(bool isPlayerFalling){
            IsLerpingYDamping = true;
            
            float startDampAmount = _framingTransposer.m_YDamping;
            float endDampAmount = 0f;
            if(isPlayerFalling){
                endDampAmount = _fallPanAmount;
                LerpedFromPlayerFalling = true;
            }
            else{
                endDampAmount = _normYPanAmount ;
            }

            float elapsedTime = 0f;
            while(elapsedTime < _fallYPanTime){
                elapsedTime += Time.deltaTime;
                float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
                _framingTransposer.m_YDamping = lerpedPanAmount;
                yield return null;
            }
            IsLerpingYDamping = false;
        }
    #endregion

}
