using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Copyright Sami S.

// use of any kind without a written permission 
// from the author is not allowed.

// DO NOT:
// Fork, clone, copy or use in any shape or form.


public class GameOfLifeUI : MonoBehaviour
{
    
    public Text modeText;
    public Text frameText;

    public void UpdateModeText(string newText)
    {
        modeText.text = newText;
    }

    public void UpdateFrameText(string newText)
    {
        frameText.text = newText;
    }

}
