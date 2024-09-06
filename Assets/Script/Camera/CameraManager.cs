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
    private Coroutine _panCameraCoroutine;

    public CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;
    private Vector2 _startingTrackedObjectOffset;

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

        _normYPanAmount = _framingTransposer.m_YDamping;

        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }
    #region Pan Camera 
            public void PanCameraOnContact(float panDistance, float panTime , PanDirection panDirection,bool panToStartingPos){
                _panCameraCoroutine = StartCoroutine(PanCamera(panDistance ,panTime ,panDirection ,panToStartingPos));
            }
            private IEnumerator PanCamera(float panDistance, float panTime , PanDirection panDirection,bool panToStartingPos){
                Vector2 endPos = Vector2.zero;
                Vector2 startingPos = Vector2.zero;

                if(!panToStartingPos){

                    switch(panDirection){
                        case PanDirection.Up:
                        endPos = Vector2.up;
                        break;
                        case PanDirection.Down:
                        endPos = Vector2.down;
                        break;
                        case PanDirection.Right:
                        endPos = Vector2.right;
                        break;
                        case PanDirection.Left:
                        endPos = Vector2.left;
                        break;
                        default:
                        break;
                    }
                    endPos *= panDistance;

                    startingPos = _startingTrackedObjectOffset;

                    endPos += startingPos;
                }
                else{
                    startingPos = _framingTransposer.m_TrackedObjectOffset;
                    endPos = _startingTrackedObjectOffset;
                }

                float elapsedTime = 0f;
                while(elapsedTime < panTime){
                    elapsedTime += Time.deltaTime;

                    Vector3 panLerp = Vector3.Lerp(startingPos, endPos , (elapsedTime/panTime));
                    _framingTransposer.m_TrackedObjectOffset = panLerp;

                    yield return null;
                }
            }
    #endregion

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
    #region Swap Camera
        public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection){
            if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0f){
                Debug.Log("從左至右，當前攝像機=左攝像機");
                //激活新相機
                cameraFromRight.enabled = true;
                //關閉舊相機
                cameraFromLeft.enabled = false;
                //設定相機為目前
                currentCamera = cameraFromRight;

                _framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            }
            else if(currentCamera == cameraFromRight && triggerExitDirection.x < 0f){
                Debug.Log("從右至左，當前攝像機=右攝像機") ;
                //激活新相機
                cameraFromRight.enabled = false;
                //關閉舊相機
                cameraFromLeft.enabled = true;
                //設定相機為目前
                currentCamera = cameraFromLeft;

                _framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            }
        }
    #endregion
}
