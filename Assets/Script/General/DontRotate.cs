using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotate : MonoBehaviour
{
    public void RotateLock(int FaceOn){
        if(FaceOn == 1 && transform.localScale.x == -1){
            transform.localScale = Vector3.one;
        }else if(FaceOn == -1 && transform.localScale.x == 1){
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
