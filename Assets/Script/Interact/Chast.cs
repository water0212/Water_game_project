using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite OpenSprite;
    public Sprite CloseSprite;
    public bool isDone;
    public FloatEventSO GainSkillEvent;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable() {
        spriteRenderer.sprite = isDone? OpenSprite : CloseSprite;
    }
    public void TriggerAction()
    {
        
        if(!isDone)
        {
            Debug.Log("開啟這王八蛋箱子");
            GainSkillEvent.RaiseEvent(1);
            OpenTheChest();
        }
    }
    public void OpenTheChest(){
        spriteRenderer.sprite = OpenSprite;
        isDone = true;
        gameObject.tag = "Untagged";
    }
}
