using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour {

    private enum State
    {
        Inactive,
        ToMainMenu,
        ToMainMenuPopUp
    }

    private State _state = State.Inactive;
    private CanvasManager _canvasManager;

	void Awake ()
    {
        _canvasManager = FindObjectOfType<CanvasManager>();
        if (!_canvasManager) { Debug.LogError("No canvas manager found!", this); }	
	}

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (_state)
            {
                case State.ToMainMenu:
                    _canvasManager.GoToMain();
                    _state = State.Inactive;
                    break;
                case State.ToMainMenuPopUp:
                    break;
            }
        }
	}

    public void SetToInactive()
    {
        _state = State.Inactive;
    }

    public void SetToGoToMain()
    {
        _state = State.ToMainMenu;
    }

    public void SetToGoToMainPop()
    {
        _state = State.ToMainMenuPopUp;
    }
}
