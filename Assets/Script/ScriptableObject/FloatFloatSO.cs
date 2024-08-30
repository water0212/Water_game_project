using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/FloatFloatEvnetSO")]
public class FloatFloatEventSO : ScriptableObject
{
    public UnityAction<float,float> OnEventRaised;
    
    public void RaiseEvent(float num,float num2 ){
        OnEventRaised?.Invoke(num,num2);   
    }
}
