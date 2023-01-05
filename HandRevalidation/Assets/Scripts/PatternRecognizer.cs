using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SG;
using SG.Examples;

public class PatternRecognizer : MonoBehaviour
{

    public SG_GestureLayer gestureLayer;
    public SG_CalibrationSequence Calibration;
    public HandRecognitionLayer handLayer;

    public GameObject SuccesionParticle;
    public SG_Waveform waveform;

    public string CurrentGesture { get; private set; }
    private int index;
    private int listIndex;
    private float elapsedSec;
    private bool isResting = false;
    private int SetsRemaining;

    [System.Serializable]
    public class GestureList
    {
        public string ListName;
        public int NrOfSets;
  

        public float RestBetweenSet = 20.0f;
        public List<SG_BasicGesture> list;
    }



    public List<GestureList> GesturesLists;
    public GestureList SelectedGestureList;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        if (SuccesionParticle != null)
            SuccesionParticle.SetActive(false);

        SelectedGestureList = GesturesLists[0];
        CurrentGesture = SelectedGestureList.list[index].name;

        if (SelectedGestureList.NrOfSets == 0)
            SetsRemaining = 1;

        SetsRemaining = SelectedGestureList.NrOfSets;
    }

    // Update is called once per frame
    void Update()
    {
        if (gestureLayer)
        {

            if (Input.GetKeyUp("space"))
            {
                ChangeExercise();
            }

            HandleGestures();
        }
    }

    private void ChangeExercise()
    {
        ++listIndex;
        listIndex = listIndex % GesturesLists.Count;
        SelectedGestureList = GesturesLists[listIndex];
        index = 0;
        elapsedSec = 0;
        CurrentGesture = SelectedGestureList.list[index].name;

        if (SelectedGestureList.NrOfSets == 0)
            SetsRemaining = 1;
        else
            SetsRemaining = SelectedGestureList.NrOfSets;

        if (SuccesionParticle != null)
            SuccesionParticle.SetActive(false);
    }

    private void HandleGestures()
    {
        if (SelectedGestureList.list[index].IsGesturing && isResting == false)
        {
            elapsedSec += Time.deltaTime;

            if (elapsedSec >= 5.0f)
            {
                if (index < SelectedGestureList.list.Count - 1)
                {
                    index++;
                    CurrentGesture = SelectedGestureList.list[index].name;
                    elapsedSec = 0;
                }
                else
                {
                    SetsRemaining--;
                    if (SetsRemaining !=0)
                    {
                        isResting = true;
                        index = 0;
                        CurrentGesture = "Resting";
                        return;
                    }

                    CurrentGesture = "Sequence is Done";
                    //playing particle and haptic Feedback
                    PlayRewards();
                }
            }
        }

        HandleRestingPhase();
    }

    private void HandleRestingPhase()
    {
        if (isResting == true)
        {
            elapsedSec += Time.deltaTime;

            if (elapsedSec >= SelectedGestureList.RestBetweenSet)
            {
                isResting = false;
                CurrentGesture = SelectedGestureList.list[index].name;
                elapsedSec = 0.0f;
            }
        }
    }

    private void PlayRewards()
    {
        if (handLayer)
        {
            handLayer.GetActiveHand().SendCmd(waveform);

            if (SuccesionParticle != null)
                SuccesionParticle.SetActive(true);

            elapsedSec = 0.0f;
        }
    }

    public int GetRemainingSets()
    {
        return SetsRemaining;
    }
}

