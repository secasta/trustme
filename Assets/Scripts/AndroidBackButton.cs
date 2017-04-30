using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour {

    private enum State
    {
        Inactive,
        ToMainMenu,
        ToAlbumAbortingStory
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
                case State.ToAlbumAbortingStory:
                    _canvasManager.AbortStory();
                    _state = State.ToMainMenu;
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

    public void SetToGoToAlbumAborting()
    {
        _state = State.ToAlbumAbortingStory;
    }
}
