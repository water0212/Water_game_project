using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSummonManager : MonoBehaviour
{
    public List<GameObject> gameObjectsInScene;
    private void Start()
    {
        var interactable = GameObject.FindGameObjectsWithTag("unknown");
        foreach (var GameObject in interactable)
        {
        gameObjectsInScene.Add(GameObject); 
        }
           
    }
    public void SummonLoot(){
        
    }
    
}
