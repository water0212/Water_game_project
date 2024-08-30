using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dialog_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("視覺提示")]
    [SerializeField] private GameObject visualCue;
    public bool isPlayerInRange;
    private Animator animator;
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private void Awake() {
        isPlayerInRange = false;
        visualCue.SetActive(false);
        animator = GetComponentInParent<Animator>();
    }
    private void Update() {
        if(isPlayerInRange&&!DialogManager.GetInstance().dialogueIsPlaying) {
            visualCue.SetActive(true);
            if(InputManager.GetInstance().GetInteractPressed()){
                PlayDialog();
            }
        }else{
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag ==  "Player") {
        isPlayerInRange = true;  
        animator.SetBool("PlayerInRange", isPlayerInRange);
        }
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag ==  "Player") {
        isPlayerInRange = false; 
        animator.SetBool("PlayerInRange", isPlayerInRange); 
        }
    }
    public void PlayDialog(){
        DialogManager.GetInstance().EnterDialogMode(inkJSON);
    }
}
