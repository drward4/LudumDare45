using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraRig CameraRig;
    public Rigidbody Rigidbody;

    public float MovementSpeed = 10f;
    public float TurnSpeed = 5f;
    public bool FreezeMotion;

    private Vector3 RigPositionDelta;

    private void Start()
    {
        this.RigPositionDelta = this.transform.position - this.CameraRig.transform.position;    
    }


    private void FixedUpdate()
    {
        if (this.FreezeMotion == false)
        {
            if (Input.GetAxis("Vertical") > 0f)
            {
                this.Rigidbody.MovePosition(this.Rigidbody.position + this.transform.forward * this.MovementSpeed * Time.fixedDeltaTime);
            }


            if (Input.GetAxis("Horizontal") != 0f)
            {
                this.Rigidbody.MoveRotation(this.Rigidbody.rotation * Quaternion.Euler(new Vector3(0f, Input.GetAxis("Horizontal") * this.TurnSpeed * Time.fixedDeltaTime)));
            }
        }
    }


    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        //this.CameraRig.transform.position = this.transform.position + this.RigPositionDelta;
    }
}
