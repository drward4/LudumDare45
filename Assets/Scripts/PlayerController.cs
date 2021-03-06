﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraRig CameraRig;
    public Rigidbody Rigidbody;
    public AudioSource FootstepsAudio;
    public AudioClip FootstepClip;

    public float MovementSpeed = 10f;
    public float TurnSpeed = 5f;
    public bool FreezeMotion;
    //public bool IsMoving;
    public Animator Animator;

    // In case the player should have to go back to the piano to write in, TBD
    public List<CandlePuzzleController> SolvedPuzzlesInHand;

    private Vector3 RigPositionDelta;


    private void Awake()
    {
        this.SolvedPuzzlesInHand = new List<CandlePuzzleController>();
        this.Animator = this.GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        this.RigPositionDelta = this.transform.position - this.CameraRig.transform.position;    
    }


    private void FixedUpdate()
    {
        if (this.FreezeMotion == false)
        {
            if (Input.GetAxis("Vertical") != 0f)
            {
                this.Animator.SetBool("IsRunning", true);

                // Make going backwards slower
                Vector3 direction = this.transform.forward * Input.GetAxis("Vertical") * (Input.GetAxis("Vertical") < 0f ? 0.5f : 1f);
                this.Rigidbody.MovePosition(this.Rigidbody.position + direction * this.MovementSpeed * Time.fixedDeltaTime);
            }
            else
            {
                this.Animator.SetBool("IsRunning", false);
            }

            if (Input.GetAxis("Horizontal") != 0f)
            {
                this.Rigidbody.MoveRotation(this.Rigidbody.rotation * Quaternion.Euler(new Vector3(0f, Input.GetAxis("Horizontal") * this.TurnSpeed * Time.fixedDeltaTime)));
            }
        }
        else
        {
            this.Animator.SetBool("IsRunning", false);
        }
    }


    public void PlayFootStep()
    {
        this.FootstepsAudio.PlayOneShot(this.FootstepClip);
    }


    private void Update()
    {
        // TODO REMOVE (USE ANIMATOR EVENT FOR FOOTSTEP
        //if (this.IsMoving == true && this.FootstepsAudio.mute == true)
        //{
        //    this.FootstepsAudio.mute = false;
        //}
        //else if (this.IsMoving == false && this.FootstepsAudio.mute == false)
        //{
        //    this.FootstepsAudio.mute = true;
        //}
            

    }

    private void LateUpdate()
    {
        //this.CameraRig.transform.position = this.transform.position + this.RigPositionDelta;
    }
}
