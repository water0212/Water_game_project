using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_NPC : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    private Dialog_Trigger dialogTrigger;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dialogTrigger = GetComponentInChildren<Dialog_Trigger>();
    }
    public void TriggerAction()
    {
        //dialogTrigger.PlayDialog();
    }

}
