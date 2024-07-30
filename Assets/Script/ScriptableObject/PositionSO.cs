using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/PositionEvnetSO")]
public class PositionEventSO : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised;
    
    public void RaiseEvent(Vector3 num ){
        OnEventRaised?.Invoke(num);   
    }
}
