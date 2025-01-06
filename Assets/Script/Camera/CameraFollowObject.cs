    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("參考")]
    [SerializeField] private Transform _PlayerTransform;
    [Header("轉向狀態")]
    [SerializeField] private float _flipYRotationTimes;
    private Coroutine coroutine;
    private PlayerControler playerControler;
    private bool _isFaceRight;
    private static CameraFollowObject instance;

    private void Awake(){
        _PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerControler = _PlayerTransform.GetComponent<PlayerControler>();
        if(instance != null) {
            Debug.LogWarning("Found more than one AttackScene in the Scene");
        }
        instance = this;
    }
    public static CameraFollowObject GetInstance() {
        return instance;
    }
    private void Update() {
        transform.position = _PlayerTransform.position;
    }
    public void CallTurn(){ 
        coroutine = StartCoroutine(FilpYLerp());
    }
    private IEnumerator FilpYLerp(){
        float startRotation = transform.localEulerAngles.y; 
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTimes){
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime/ _flipYRotationTimes));
            transform.rotation = Quaternion.Euler( 0f ,yRotation, 0f );

            yield return null;
        }
    }
    private float DetermineEndRotation(){
        _isFaceRight = !_isFaceRight;
        if(_isFaceRight){
            return 180f;
        }else
        return 0;
    }
}
