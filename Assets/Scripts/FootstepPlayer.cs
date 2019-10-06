using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public PlayerController Controller;
    
    public void Footstep()
    {
        this.Controller.PlayFootStep();
    }
}
