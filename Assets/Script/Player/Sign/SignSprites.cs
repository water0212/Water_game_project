using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class SignSprites : MonoBehaviour
{
    private static PlayerInputAction inputActions;
    protected string pressKey;
    protected string pressTag;
    protected IInteractable targetItem;
 //   private PlayerInputAction inputActions;
    public Transform playerTrans;
    [SerializeField]protected bool canPress;
    protected Animator anim;
    private void Awake() {
        anim = GetComponent<Animator>();
        inputActions= new PlayerInputAction();
    }
    private void Update() {
        this.GetComponent<SpriteRenderer>().enabled = canPress;
        
    }
    [Header("監聽")]
    public GameObjectSO tagSignEvent;
    public GameObjectSO tagSignEventEnd;
    private void OnEnable() {
        inputActions.Enable();
        inputActions.GamePlayer.Interact.started += OnComfirm;
        //inputActions.GamePlayer.Block.started += OnComfirm;
        tagSignEvent.OnEventRaised += OnSignEvent;
        tagSignEventEnd.OnEventRaised += OnSignEventEnd;
    }
    private void OnDisable() {
        inputActions.Disable();
        tagSignEvent.OnEventRaised -= OnSignEvent;
        tagSignEventEnd.OnEventRaised -= OnSignEventEnd;
    }


    protected virtual void OnComfirm(InputAction.CallbackContext context)
    {
        pressKey= context.control.displayName;
    //    Debug.Log("wwww" + pressKey);
    }

    protected virtual void OnSignEvent(GameObject obj)
    {
        targetItem = obj.GetComponent<IInteractable>();
        Debug.Log(targetItem);
    }

    protected virtual void OnSignEventEnd(GameObject obj)
    {
        
    }
}