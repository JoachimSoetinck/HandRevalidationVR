
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG.Examples
{
    public class HandRecognitionLayer : MonoBehaviour
    {
        public SG_TrackedHand leftHand, rightHand;
        protected SG_TrackedHand activeHand = null;

   
        public SG_TrackedHand GetActiveHand() { return activeHand; }
        public KeyCode calibrateKey = KeyCode.C;

        public Button calibrateBtn;

        public GameObject[] ffbObjects = new GameObject[0];
        // protected SG_SimpleTracking[] trackScripts = new SG_SimpleTracking[0];
        

        protected int objIndex = -1;

        protected bool allowedSwap = false;
        protected float openTime = 0.2f;
        protected float openedTimer = 0;

 

        public Text objectText;

        private static readonly float tOpen = 0.5f, iOpen = 0.5f, mOpen = 0.5f;

        protected void SetRelevantScripts(SG_TrackedHand hand, bool active)
        {
            if (hand != null)
            {
                hand.HandModelEnabled = active;
                hand.SetLayer(SG_TrackedHand.HandLayer.Animation, active);
                hand.SetLayer(SG_TrackedHand.HandLayer.Calibration, true);
                hand.SetLayer(SG_TrackedHand.HandLayer.Feedback, false);
                hand.SetLayer(SG_TrackedHand.HandLayer.Gestures, true);
                hand.SetLayer(SG_TrackedHand.HandLayer.Grab, true);
                // hand.SetLayer(SG_TrackedHand.HandLayer.HandModel, active);
                hand.SetLayer(SG_TrackedHand.HandLayer.Passthrough, false);
                hand.SetLayer(SG_TrackedHand.HandLayer.Physics, false);


            }
        }


        public void CalibrateWrist()
        {
            if (this.activeHand != null && this.activeHand.handAnimation != null)
            {
                if (this.activeHand.RealHandSource != null && this.activeHand.RealHandSource is SG.SG_HapticGlove)
                {
                    Quaternion imu;
                    if (((SG.SG_HapticGlove)this.activeHand.RealHandSource).GetIMURotation(out imu))
                    {
                        this.activeHand.handAnimation.CalibrateWrist(imu);
                    }
                }
            }
        }


        public static bool CheckHandOpen(SG_TrackedHand hand)
        {
            float[] flexions;
            if (hand.GetNormalizedFlexion(out flexions))
            {
                bool res = flexions.Length > 3 && flexions[0] < tOpen && flexions[1] < iOpen && flexions[2] < mOpen;
                //Debug.Log(((int)flexions[0]).ToString() + " / " + tOpen.ToString()
                //        + "\t"
                //        + ((int)flexions[1]).ToString() + " / " + iOpen.ToString()
                //        + "\t"
                //        + ((int)flexions[2]).ToString() + " / " + mOpen.ToString()
                //        + "\t" + res
                //     );
                return res;
            }
            return true;
        }


        protected void SetObject(int index, bool active)
        {
            if (index > -1 && index < ffbObjects.Length)
            {
                ffbObjects[index].SetActive(active);
               
                if (active && objectText != null) { objectText.text = ffbObjects[index].name; }
            }
            else if (active && objectText != null) { objectText.text = ""; }
        }

        protected int WrapIndex(int newIndex)
        {
            if (newIndex < -1) { return ffbObjects.Length - 1; }
            else if (newIndex >= ffbObjects.Length) { return -1; }
            return newIndex;
        }


        void ConnectObjects(SG_TrackedHand hand)
        {
            for (int i = 0; i < ffbObjects.Length; i++)
            {
                ffbObjects[i].SetActive(true);

                SG_SimpleTracking trackingScript = ffbObjects[i].GetComponent<SG_SimpleTracking>();
                if (trackingScript == null) { trackingScript = ffbObjects[i].AddComponent<SG_SimpleTracking>(); }
                if (trackingScript != null) { trackingScript.SetTrackingTarget(hand.handModel.wristTransform, true); }

                ffbObjects[i].SetActive(false);
            }
        }


        public void ResetCalibration()
        {
            if (activeHand != null && activeHand.calibration != null)
            {
                activeHand.calibration.StartCalibration(true);
            }
        }




        void Awake()
        {

        }

        // Use this for initialization
        void Start()
        {
            //SG.Util.SG_Util.SetChildren(leftHand.transform, false);
            //SG.Util.SG_Util.SetChildren(rightHand.transform, false);
            SetRelevantScripts(leftHand, false);
            SetRelevantScripts(rightHand, false);

            if (leftHand.calibration != null)
            {
                //leftHand.calibration.gameObject.SetActive(true);
                leftHand.calibration.startCondition = SG_CalibrationSequence.StartCondition.WhenNeeded;
            }
            if (rightHand.calibration != null)
            {
                //rightHand.calibration.gameObject.SetActive(true);
                rightHand.calibration.startCondition = SG_CalibrationSequence.StartCondition.WhenNeeded;
            }

            if (objectText != null) { objectText.text = ""; }

            if (calibrateBtn != null)
            {
                Text btnText = calibrateBtn.GetComponentInChildren<Text>();
                if (btnText != null) { btnText.text = btnText.text + "\r\n(" + this.calibrateKey.ToString() + ")"; }
            }


        }

        // Update is called once per frame
        void Update()
        {
            if (activeHand == null)
            {
                if (rightHand.IsConnected() || leftHand.IsConnected())
                {
                    activeHand = rightHand.IsConnected() ? rightHand : leftHand;
                    SetRelevantScripts(activeHand, true);
                    ConnectObjects(activeHand);
                    
                }
            }
         
        }
    }
}
