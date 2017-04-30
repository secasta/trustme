using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveStoryButton : MonoBehaviour {

    private CanvasManager _canvasManager;

	void Awake () {
        _canvasManager = FindObjectOfType<CanvasManager>();
        if (!_canvasManager) { Debug.LogError("No Canvas Manager found!", this); }
    }

    public void LeaveStory()
    {
        _canvasManager.AbortStory();
    }
}
