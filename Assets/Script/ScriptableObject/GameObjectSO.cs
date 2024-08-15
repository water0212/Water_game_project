using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/GameObjectEventSO")]
public class GameObjectSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;
    public void RaiseEvent(GameObject obj){
        OnEventRaised?.Invoke(obj);
    }
}
