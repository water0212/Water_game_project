using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using Cainos.LucidEditor;
using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [FoldoutGroup("TriggerIn")]public bool triggerIn;
    [FoldoutGroup("TriggerOut")]public bool triggerOut;
    private Elevator elevator;
    private void Awake() {
        elevator= GetComponentInParent<Elevator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(triggerIn && collision.tag == "Player")elevator.ElevatorTrigger();
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(triggerOut && collision.tag == "Player")elevator.ElevatorTrigger();
    }
}
