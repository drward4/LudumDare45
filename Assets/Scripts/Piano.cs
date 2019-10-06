using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PianoEvent();

public class Piano : MonoBehaviour
{
    public PianoEvent PlayerEnteredArea;
    public PianoEvent PlayerLeftArea;

    private void OnTriggerEnter(Collider other)
    {
        if (GameController.IsPlayerCollider(other))
        {
            if (this.PlayerEnteredArea != null)
            {
                this.PlayerEnteredArea();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (GameController.IsPlayerCollider(other))
        {
            if (this.PlayerLeftArea != null)
            {
                this.PlayerLeftArea();
            }
        }
    }
}
