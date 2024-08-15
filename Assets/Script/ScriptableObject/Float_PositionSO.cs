using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/Float_PositionEvnetSO")]
public class Float_PositionEventSO : ScriptableObject
{
    public UnityAction<float,Vector3> OnEventRaised;
    
    public void RaiseEvent(float num,Vector3 position ){
        OnEventRaised?.Invoke(num,position);   
    }
}