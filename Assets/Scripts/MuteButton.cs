﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

    public Sprite _speakerIdle;
    public Sprite _speakerPressed;
    public Sprite _speakerCrossedIdle;
    public Sprite _speakerCrossedPressed;

    private enum State
    {
        Muted,
        Unmuted
    }

    private State _state = State.Unmuted;
    private Button _muteButton;
    private AudioController _audioController;

    void Awake () {
        _muteButton = GetComponent<Button>();
        if (!_muteButton) { Debug.LogError("No button component found!", this); }
        _audioController = FindObjectOfType<AudioController>();
        if (!_audioController) { Debug.LogError("No audio controller found!", this); }
    }

    void Start()
    {
        float volume = CurrentSettings.MasterVolume;
        if (volume == 0)
        {
            SpriteState spriteState = new SpriteState();
            spriteState = _muteButton.spriteState;
            _muteButton.image.sprite = _speakerCrossedIdle;
            spriteState.pressedSprite = _speakerCrossedPressed;
            _state = State.Muted;
            _muteButton.spriteState = spriteState;
        }
    }

    public void OnButtonPressed()
    {
        SpriteState spriteState = new SpriteState();
        spriteState = _muteButton.spriteState;
        switch (_state)
        {
            case State.Unmuted:
                _muteButton.image.sprite = _speakerCrossedIdle;
                spriteState.pressedSprite = _speakerCrossedPressed;
                CurrentSettings.SetMasterVolume(0f);
                _state = State.Muted;
                break;
            case State.Muted:
                _muteButton.image.sprite = _speakerIdle;
                spriteState.pressedSprite = _speakerPressed;
                CurrentSettings.SetMasterVolume(1f);
                _state = State.Unmuted;
                break;
        }
        _muteButton.spriteState = spriteState;
        _audioController.ChangeVolume();
    }
}
