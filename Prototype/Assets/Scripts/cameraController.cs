using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject player;
    public bool isRotate = true;
    public float rotateSpeed = 5.0f;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (isRotate == true)
        {
            Quaternion camTurnAngleHor = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up);
            offset = camTurnAngleHor *  offset;
        }
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
