using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Camera cam;
    public GameObject player, playerHead;
    public bool isRotate = true;
    public float rotateSpeed = 5.0f;
    private Vector3 offset;
    RaycastHit oldHit;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        float characterDistance = Vector3.Distance(transform.position, playerHead.transform.position);
        Debug.DrawRay(this.transform.position, (playerHead.transform.position - this.transform.position));
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (playerHead.transform.position - this.transform.position), out hit, characterDistance))
        {
            if (oldHit.transform && hit.transform.name != "Player")
            {
                //oldHit.collider.GetComponent<Renderer>().enabled = true;
                Renderer rend = oldHit.collider.GetComponent<Renderer>();
                rend.sharedMaterial.SetFloat("_Mode", 0);
                rend.sharedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                rend.sharedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                rend.sharedMaterial.SetInt("_ZWrite", 1);
                rend.sharedMaterial.DisableKeyword("_ALPHATEST_ON");
                rend.sharedMaterial.DisableKeyword("_ALPHABLEND_ON");
                rend.sharedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                rend.sharedMaterial.renderQueue = -1;
                Color col = rend.sharedMaterial.color;
                col.a = 1;
                rend.sharedMaterial.SetColor("_Color", col);
                Debug.Log("Enabled");
            }
            
            if (hit.transform.name != "Player")
            {
                //hit.collider.GetComponent<Renderer>().enabled = false;
                Renderer rend = hit.collider.GetComponent<Renderer>();
                rend.sharedMaterial.SetFloat("_Mode", 3);
                rend.sharedMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                rend.sharedMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                rend.sharedMaterial.SetInt("_ZWrite", 0);
                rend.sharedMaterial.DisableKeyword("_ALPHATEST_ON");
                rend.sharedMaterial.DisableKeyword("_ALPHABLEND_ON");
                rend.sharedMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                rend.sharedMaterial.renderQueue = 3000;
                Color col = rend.sharedMaterial.color;
                col.a = 0.1f;
                rend.sharedMaterial.SetColor("_Color", col);
                Debug.Log("Disabled");
                oldHit = hit;
            }
        }
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
