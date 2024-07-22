using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/VoidEvnetSO")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;
    
    public void RaiseEvent( ){
        OnEventRaised?.Invoke();   
    }
}
