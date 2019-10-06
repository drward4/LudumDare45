using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void CandlePuzzleEvent(CandlePuzzleController candlePuzzle);

public class CandlePuzzleController : MonoBehaviour
{
    public CandleSlot CandleSlotPrefab;
    public Candle CandlePrefab;
    public List<CandleSlot> CandleSlots;
    public int[] CorrectSequence;
    public int[] StartingSequence;
    public CameraRig CameraRig;
    public AudioClip SolvedClip;
    public AudioSource PuzzleAudio;
    public Image SheetMusicPiece;
    public bool IsSolved;
    public bool IsActivePuzzle;
    public bool ShowSpecialInfo;

    public CandlePuzzleEvent PlayerEnteredArea;
    public CandlePuzzleEvent PlayerLeftArea;
    public CandlePuzzleEvent PuzzleWasSolved;

    private CandleSlot CurrentlySelectedSlot;
   // private Vector3 SelectedCandlePositionDelta;

    private void Start()
    {
        if (this.CorrectSequence.Length != this.CandleSlots.Count)
        {
            Debug.LogError("Mismatch between sequence and slots");
        }

        for (int i = 0; i < this.CandleSlots.Count; i++)
        {
            this.CandleSlots[i].WasSelected += this.HandleCandleSelected;
            Candle candle = Instantiate<Candle>(this.CandlePrefab);
            candle.NoteID = this.StartingSequence[i];
            this.CandleSlots[i].SetCurrentCandle(candle);

            float size = ((float)this.StartingSequence[i] / 8f) * 2.5f;  // Adjust candle size according to note
            this.CandleSlots[i].CurrentCandle.SetSize(size);
        }
    }


    private void HandleCandleSelected(CandleSlot candleSlot)
    {
        if (this.IsActivePuzzle == false)
            return;

        if (this.CurrentlySelectedSlot == null)
        {
            this.CurrentlySelectedSlot = candleSlot;
            this.CurrentlySelectedSlot.CurrentCandle.transform.position = candleSlot.transform.position + Vector3.up * 0.5f; // GameController.GetCursorWorldPosition();
        }
        else
        {
            if (candleSlot == this.CurrentlySelectedSlot)
            {
                candleSlot.SetCurrentCandle(candleSlot.CurrentCandle);
            }
            else
            {
                Candle temp = this.CurrentlySelectedSlot.CurrentCandle;
                this.CurrentlySelectedSlot.SetCurrentCandle(candleSlot.CurrentCandle);
                candleSlot.SetCurrentCandle(temp);
            }

            this.CurrentlySelectedSlot = null;

            this.CheckSolution();
        }
    }


    public void CheckSolution()
    {
        bool isCorrect = true;

        for (int i = 0; i < this.CandleSlots.Count; i++)
        {
            if (this.CandleSlots[i].CurrentCandle.NoteID != this.CorrectSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect == true)
        {
            this.IsSolved = true;

            for (int i = 0; i < this.CandleSlots.Count; i++)
            {
                this.CandleSlots[i].CurrentCandle.FlameParticles.Play();
            }

            if (this.PuzzleWasSolved != null)
            {
                this.PuzzleWasSolved(this);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (GameController.IsPlayerCollider(other))
        {
            if (this.PlayerEnteredArea != null)
            {
                this.PlayerEnteredArea(this);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (GameController.IsPlayerCollider(other))
        {
            if (this.PlayerLeftArea != null)
            {
                this.PlayerLeftArea(this);
            }
        }
    }


    private void Update()
    {
        if (this.CurrentlySelectedSlot != null)
        {
            //this.CurrentlySelectedSlot.CurrentCandle.transform.position = GameController.GetCursorWorldPosition() + this.SelectedCandlePositionDelta;

            // Cancel with Right Mouse button?
            //if (Input.GetMouseButtonUp(1) == true)
            //{

            //}
        }
    }
}
