using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float cameraMinZoom = 8.0f;
    private float cameraMaxZoom = 25.0f;

    private float cameraZoomSpeed = 1.2f;
    private float cameraMovementSpeed = 0.2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0.0f)
	    {
	        if (scroll > 0.0f && this.gameObject.transform.position.y >= cameraMinZoom)
	        {
	            this.gameObject.transform.Translate(Vector3.forward * cameraZoomSpeed);
	        } else if (scroll < 0.0f && this.gameObject.transform.position.y <= cameraMaxZoom)
	        {
                this.gameObject.transform.Translate(-Vector3.forward * cameraZoomSpeed);
            }
	    }

	    Vector3 vectorForward = new Vector3(this.gameObject.transform.forward.x, 0, this.gameObject.transform.forward.z).normalized;
	    Vector3 vectorRight = new Vector3(this.gameObject.transform.right.x, 0, this.gameObject.transform.right.z).normalized;


        if (Input.GetKey(KeyCode.W))
	    {
            this.gameObject.transform.Translate(vectorForward * cameraMovementSpeed, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(-vectorForward * cameraMovementSpeed, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(-vectorRight * cameraMovementSpeed, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(vectorRight * cameraMovementSpeed, Space.World);
        }
    }
}
