using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour {

    public List<Canvas> _storyCanvases;
    public Image _backgroundImage;
    public List<Sprite> _backgroundSprites;
    public Canvas _mainMenuCanvas;
    public Canvas _settingsCanvas;
    public Canvas _storySelectCanvas;

    private int _currentStoryIndex = 0;
    private Canvas _currentCanvas;
    private List<Canvas> _unbeatenStoryCanvases;
    private List<Sprite> _unbeatenBackgroundSprites;
    private bool _alreadyBeatenLevel = false;

    void Awake()
    {
        _unbeatenStoryCanvases = new List<Canvas>(_storyCanvases);
        _unbeatenBackgroundSprites = new List<Sprite>(_backgroundSprites);//we're not really using _backgroundSprites at all, but just in case
        _currentCanvas = _mainMenuCanvas;
        SelectNextStory();
        _backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        EnableCanvas();
    }

    public void StartBeatenStory(int storyId)
    {
        if ((storyId >= 0) && (storyId < _storyCanvases.Count))
        {
            _alreadyBeatenLevel = true;
            DisableCanvas();
            _currentCanvas = Instantiate(_storyCanvases[storyId]);
            EnableCanvas();
        }
        else
        {
            Debug.LogError("Story Id out of range. Index " + storyId + " is not on my list!");
        }
    }

    public void NextUnbeatenStory()
    {
        if (_unbeatenStoryCanvases.Count > 0)
        {
            DisableCanvas();
            _currentCanvas = Instantiate(_storyCanvases[_currentStoryIndex]);
            EnableCanvas();
        }
    }

    public void GoToSettings()
    {
        DisableCanvas();
        _currentCanvas = _settingsCanvas;
        EnableCanvas();
    }

    public void GoToAlbum()
    {
        DisableCanvas();
        _currentCanvas = _storySelectCanvas;
        EnableCanvas();
    }

    public void GoToMain()
    {
        DisableCanvas();
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
    }

    public void FinishStory(bool isBeaten)
    {
        Canvas auxCanvas = _currentCanvas;
        DisableCanvas();
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
        Destroy(auxCanvas.gameObject);
        if (!_alreadyBeatenLevel)
        {
            if (isBeaten)
            {
                //TODO Show you've unlocked the story, although the unlocking is done from StoryController

                _unbeatenBackgroundSprites.RemoveAt(_currentStoryIndex);
                _unbeatenStoryCanvases.RemoveAt(_currentStoryIndex);
            }
        }
        _alreadyBeatenLevel = false;
        SelectNextStory();
    }

    private void DisableCanvas()
    {
        _currentCanvas.enabled = false;
    }

    private void EnableCanvas()
    {
        _currentCanvas.enabled = true;
    }

    private void SelectNextStory()
    {
        if (_unbeatenStoryCanvases.Count > 0)
        {
            int rand = Random.Range(0, _unbeatenStoryCanvases.Count);
            _currentStoryIndex = rand;
            _backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        }
        else
        {
            //TODO Quitar los botones de la vista
            _backgroundImage.sprite = null;
            _backgroundImage.enabled = false;
        }
    }

}
