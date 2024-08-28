using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SignSpriteF : MonoBehaviour
{
    protected string pressKey;
    public PlayerInputAction inputActions;
    public Transform playerTrans;
    [SerializeField]protected IInteractable targetItem;
    //private PlayerInputAction inputActions;
    [SerializeField]protected bool canPress;
    protected Animator anim;
    private void Awake() {
        anim = GetComponent<Animator>();
        inputActions= new PlayerInputAction();
        
        if (inputActions != null){
            Debug.Log(inputActions);
        }
    }
    private void Update() {
        this.GetComponent<SpriteRenderer>().enabled = canPress;
        transform.localScale = playerTrans.localScale;
        
    }
        private void OnEnable() {
        inputActions.Enable();
        inputActions.GamePlayer.Interact.started += OnComfirm;
    }
    private void OnDisable() {
        inputActions.GamePlayer.Interact.started -= OnComfirm;
    }

    // Start is called before the first frame update
    public void OnComfirm(InputAction.CallbackContext context)
    {
        //Debug.Log("swswswsw");
        if(canPress&&targetItem!=null&&context.control.displayName=="F"){
         //   Debug.LogWarning("weqweq");
            targetItem.TriggerAction();
            
        }
    }
    public void OnSignEvent(GameObject obj)
    {
        if(obj.CompareTag(this.tag)){
        anim.SetBool("CanPress",true);
        canPress = true;
        }
        targetItem = obj.GetComponent<IInteractable>();
        //Debug.Log(targetItem);
        
        
    }
    public void OnSignEventEnd(GameObject obj)
    {
        
        anim.SetBool("CanPress",false);
        canPress = false; 
        
        
    }
}
