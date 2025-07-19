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
   private bool isGround;
    private bool touchWall;
    private bool isOnTheFloor;
    private bool canGrabTheEdge;

    public bool IsGround { get => isGround; set => isGround = value; }
    public bool TouchWall { get => touchWall; set => touchWall = value; }
    public bool IsOnTheFloor { get => isOnTheFloor; set => isOnTheFloor = value; }
    public bool CanGrabTheEdge { get => canGrabTheEdge; set => canGrabTheEdge = value; }

    /* [Header("穿越地板修正")]
public float checkDistance;
public Vector2 checkoffset;*/
    private void FixedUpdate() {
    Check();
   }

    private void Check()
    {
        //檢測
       IsOnTheFloor = Physics2D.OverlapCircle((Vector2)transform.position + flooroffset*transform.localScale.x,checkRadious,groundLayer);
       IsGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset*transform.localScale.x,checkRadious,groundLayer);
       TouchWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+wallOffset.x*transform.localScale.x,transform.position.y + wallOffset.y),checkRadious,groundLayer);
       CanGrabTheEdge =  Physics2D.OverlapCircle(new Vector2(transform.position.x+grabEdgeOffset.x*transform.localScale.x,transform.position.y + grabEdgeOffset.y),checkRadious,groundLayer);
        //牆體判斷自己做的
        //下面是穿越地面的判斷 看情況保留
        /*RaycastHit2D checkhit = Physics2D.Raycast((Vector2)transform.position+checkoffset, Vector2.down, checkDistance, groundLayer);
                    if (checkhit&&isGround){
                        this.transform.position = checkhit.point+new Vector2(0,0.2f);
                    }*/
    }

    private void OnDrawGizmosSelected() {
       Gizmos.DrawWireSphere((Vector2)transform.position + flooroffset*transform.localScale.x,checkRadious);
       Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset*transform.localScale.x,checkRadious);
       Gizmos.DrawWireSphere(new Vector2(transform.position.x+wallOffset.x*transform.localScale.x,transform.position.y + wallOffset.y),checkRadious);
       Gizmos.DrawWireSphere(new Vector2(transform.position.x+grabEdgeOffset.x*transform.localScale.x,transform.position.y + grabEdgeOffset.y),checkRadious);
       //牆體判斷自己做的
       //下面是穿越地面的判斷 看情況保留
       //Gizmos.DrawLine((Vector2)transform.position+checkoffset,(Vector2)transform.position+checkoffset-new Vector2(0,checkDistance));

        
    }
}
