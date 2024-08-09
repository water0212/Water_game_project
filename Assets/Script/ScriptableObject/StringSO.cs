using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/StringEvnetSO")]
public class StringEventSO : ScriptableObject
{
    public UnityAction<string> OnEventRaised;
    
    public void RaiseEvent(string str ){
        OnEventRaised?.Invoke(str);   
    }
}
