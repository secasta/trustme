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

    void Awake()
    {
        _unbeatenStoryCanvases = _storyCanvases;
        _unbeatenBackgroundSprites = _backgroundSprites;
        _currentCanvas = _mainMenuCanvas;
        _backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        EnableCanvas();
    }

    public void StartBeatenStory(int storyId)
    {
            DisableCanvas();
            _currentStoryIndex = storyId;
            _currentCanvas = Instantiate(_storyCanvases[storyId]);
            EnableCanvas();
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
            int rand = Random.Range(0, _unbeatenStoryCanvases.Count - 1);
            _currentStoryIndex = rand;
        }
        else
        {
            //Quitar los botones de la vista
            _backgroundImage.sprite = null;
        }
    }

}
