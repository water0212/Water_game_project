using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhysicCheck : MonoBehaviour
{


   public float checkRadious;
   
   public Vector2 flooroffset;
   public Vector2 bottomOffset;
   public Vector2 wallOffset;
   public Vector2 grabEdgeOffset;
   public LayerMask groundLayer;
   [Header("狀態")]
   public bool isGround;
   public bool touchWall;
   public bool isOnTheFloor;
   public bool canGrabTheEdge;
   private void FixedUpdate() {
    Check();
   }

    private void Check()
    {
        //檢測
       isOnTheFloor = Physics2D.OverlapCircle((Vector2)transform.position + flooroffset*transform.localScale.x,checkRadious,groundLayer);
       isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset*transform.localScale.x,checkRadious,groundLayer);
       touchWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+wallOffset.x*transform.localScale.x,transform.position.y + wallOffset.y),checkRadious,groundLayer);
       canGrabTheEdge =  Physics2D.OverlapCircle(new Vector2(transform.position.x+grabEdgeOffset.x*transform.localScale.x,transform.position.y + grabEdgeOffset.y),checkRadious,groundLayer);
        //牆體判斷自己做的
    }

    private void OnDrawGizmosSelected() {
       Gizmos.DrawWireSphere((Vector2)transform.position + flooroffset*transform.localScale.x,checkRadious);
       Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset*transform.localScale.x,checkRadious);
       Gizmos.DrawWireSphere(new Vector2(transform.position.x+wallOffset.x*transform.localScale.x,transform.position.y + wallOffset.y),checkRadious);
       Gizmos.DrawWireSphere(new Vector2(transform.position.x+grabEdgeOffset.x*transform.localScale.x,transform.position.y + grabEdgeOffset.y),checkRadious);
       //牆體判斷自己做的
       

        
    }
}
