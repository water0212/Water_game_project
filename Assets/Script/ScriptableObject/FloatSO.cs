using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/FloatEvnetSO")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised;
    
    public void RaiseEvent(float num ){
        OnEventRaised?.Invoke(num);   
    }
}
