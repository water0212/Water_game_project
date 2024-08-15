using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraP;
    public float xMoveRate = 1;
    private Vector2 startPoint;
    public Vector2 camStartPoint;
    public float yMoveRate = 1;
    public bool YMove;
    int myInt;
    void Start()
    {
        GameObject myObject = GameObject.FindGameObjectWithTag("MainCamera");
        cameraP = myObject.transform;
        startPoint = transform.position;
        camStartPoint = cameraP.position;
        myInt = YMove ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(startPoint.x + (cameraP.position.x-camStartPoint.x)*xMoveRate, startPoint.y+(cameraP.position.y-camStartPoint.y)*yMoveRate*myInt);
    }
}
