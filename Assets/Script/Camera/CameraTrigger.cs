using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEditor;

public class CameraTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;
    public Collider2D _coll;

    private void Start() {
        _coll = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            if(customInspectorObjects.panCameraOnContact){
                CameraManager.instence.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")) {
            Vector2 exitDirection = (other.transform.position - _coll.bounds.center).normalized;
            if(customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)   {
                CameraManager.instence.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight,exitDirection);
            }
            if(customInspectorObjects.panCameraOnContact){
                CameraManager.instence.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}
[System.Serializable]
public class CustomInspectorObjects{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}
public enum PanDirection{
    Up,Down,Left,Right,
}
[CustomEditor(typeof(CameraTrigger))]
public class MyScriptEditor : Editor{
    CameraTrigger cameraTrigger;
    private void OnEnable() {
        cameraTrigger = (CameraTrigger)target;
    }
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if(cameraTrigger.customInspectorObjects.swapCameras){
            cameraTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraTrigger.customInspectorObjects.cameraOnLeft, 
            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            cameraTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraTrigger.customInspectorObjects.cameraOnRight, 
            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if(cameraTrigger.customInspectorObjects.panCameraOnContact){
            cameraTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", cameraTrigger.customInspectorObjects.panDirection);
            cameraTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraTrigger.customInspectorObjects.panDistance);
            cameraTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraTrigger.customInspectorObjects.panTime);
        }
        
        if(GUI.changed){
            EditorUtility.SetDirty(cameraTrigger);
        }   
    }
}
