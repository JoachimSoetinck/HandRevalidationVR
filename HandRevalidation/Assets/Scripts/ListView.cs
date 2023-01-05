using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour
{
    // Start is called before the first frame update
    public PatternRecognizer patternRecognizer;
    public Text exampleText;

    public string lStr = "";

    // Update is called once per frame
    void Update()
    {

        exampleText.text = "Selected List: " + patternRecognizer.SelectedGestureList.ListName;
        exampleText.text += "\r\n" + "Remaining sets: " + patternRecognizer.GetRemainingSets();
        exampleText.text += "\r\n" + "Current Gesture: " + patternRecognizer.CurrentGesture;
    }
}
