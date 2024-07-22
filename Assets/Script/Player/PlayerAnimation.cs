using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public VoidEventSO DeadAnimation;
    private Animator anim;
    private PhysicCheck physicCheck;
    private Rigidbody2D rb;
    private Character character;
    private void OnEnable() {
        DeadAnimation.OnEventRaised += Deadanim;
    }

    private void Deadanim()
    {
        anim.Play("Dead");
        anim.SetBool("Dead", true);
    }

    private void Awake() {
        anim = GetComponent<Animator>();
        physicCheck = GetComponent<PhysicCheck>();
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }
    private void Update() {
        UpdateAnimator();
        BlockAnimator();
    }

    private void BlockAnimator()
    {
        anim.SetBool("isBlock", character.isBlock);
    }
    public void PerfectBlockAnimator()
    {
        anim.Play("PerfectBlock");
    }

    public void UpdateAnimator() {
        anim.SetFloat("VelocityX",Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGround",physicCheck.isGround);
        anim.SetFloat("VelocityY",rb.velocity.y);
    }
    public void HurtAnimation(){
        anim.SetBool("isHurt",true);
    }
}
