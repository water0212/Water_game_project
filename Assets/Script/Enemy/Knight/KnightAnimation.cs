using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private PhysicCheck physicCheck;
    private Rigidbody2D rb;
    private knightEnemy knight;
    private void Awake() {
        anim = GetComponent<Animator>();
        physicCheck = GetComponent<PhysicCheck>();
        rb = GetComponent<Rigidbody2D>();
        knight = GetComponent<knightEnemy>();
    }
    private void Update() {
        anim.SetFloat("VelocityY",rb.velocity.y);
        anim.SetFloat("VelocityX",Math.Abs(rb.velocity.x));
        anim.SetBool("IsGround",physicCheck.isGround);
        anim.SetBool("IsHurt",knight.wasHited);
    }
    public void Hurt(){
        anim.Play("Hurt");
    }
    public void InAttacking(){
        knight.attacking = true;
    }
    public void ExitAttacking(){
        knight.attacking =false;
    }
}
