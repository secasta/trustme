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
    public Canvas _languageSelectionCanvas;

    public bool testing = false;

    [HideInInspector]
    public bool _isStoryAbortable = false;

    private int _currentStoryIndex = 0;
    private Canvas _currentCanvas;
    private List<Canvas> _unbeatenStoryCanvases;
    private List<Sprite> _unbeatenBackgroundSprites;
    private bool _alreadyBeatenLevel = false;
    private VerticalScrollRect _storiesScrollRect;
    private Canvas _randomBeatenCanvas;
    private AndroidBackButton _androidBackButton;
    private int _nextIndexToAvoid = -1;

    public delegate void StoryCompleted(int id);
    public static event StoryCompleted OnStoryCompleted;

    void Awake()
    {
        //Get references
        _storiesScrollRect = _storySelectCanvas.GetComponentInChildren<VerticalScrollRect>();
        if (!_storiesScrollRect) { Debug.LogError("No scroll rect found on children", this); }
        _androidBackButton = FindObjectOfType<AndroidBackButton>();
        if (!_androidBackButton) { Debug.LogError("No android back button script found", this); }

        //Reunlock stories on the album
        if (Game.current != null)
        {
            Game.current.ReUnlock();
        }
        else
        {
            Debug.LogError("There's no instance of Game");
        }

        //Copy whole lists into unbeaten lists
        _unbeatenStoryCanvases = new List<Canvas>(_storyCanvases);
        _unbeatenBackgroundSprites = new List<Sprite>(_backgroundSprites);

        //Remove previously beaten stories (indexes) from unbeaten lists
        foreach (int storyId in Game.current._unlockedStories)
        {
            Debug.Log("Removing story " + storyId + " from unbeaten lists.");
            _unbeatenBackgroundSprites.RemoveAt(storyId);
            _unbeatenStoryCanvases.RemoveAt(storyId);
        }        
    }

    void Start()
    {
        _currentCanvas = _mainMenuCanvas;
        SelectNextStory();
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

            _isStoryAbortable = true;
            _androidBackButton.SetToGoToAlbumAborting();
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
            _currentCanvas = Instantiate(_unbeatenStoryCanvases[_currentStoryIndex]);
            EnableCanvas();

            _isStoryAbortable = false;
            _androidBackButton.SetToInactive();
        }
        else
        {
            _alreadyBeatenLevel = true;
            DisableCanvas();
            _currentCanvas = Instantiate(_randomBeatenCanvas);
            EnableCanvas();

            _isStoryAbortable = false;//it could be the other way around
            _androidBackButton.SetToInactive();
        }
    }

    public void GoToSettings()
    {
        DisableCanvas();
        _currentCanvas = _settingsCanvas;
        EnableCanvas();
        _androidBackButton.SetToGoToMain();
    }

    public void GoToAlbum()
    {
        DisableCanvas();
        _currentCanvas = _storySelectCanvas;
        _storiesScrollRect.ResetPosition();
        EnableCanvas();
        _androidBackButton.SetToGoToMain();
    }

    public void GoToMain()
    {
        DisableCanvas();
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
        _androidBackButton.SetToInactive();
    }

    public void GoToLanguageSelector()
    {
        DisableCanvas();
        _currentCanvas = _languageSelectionCanvas;
        EnableCanvas();
        _androidBackButton.SetToGoToSettings();
    }

    public void FinishStory(bool isBeaten, int finishedStoryId)
    {
        Canvas auxCanvas = _currentCanvas;
        DisableCanvas();
        
        if (!_alreadyBeatenLevel)
        {
            if (isBeaten)
            {
                _unbeatenBackgroundSprites.RemoveAt(_currentStoryIndex);
                _unbeatenStoryCanvases.RemoveAt(_currentStoryIndex);
                _currentCanvas = _storySelectCanvas;
                if (OnStoryCompleted != null)
                {
                    OnStoryCompleted(finishedStoryId);
                }
                _nextIndexToAvoid = -1;
            }
            else
            {
                _currentCanvas = _mainMenuCanvas;
                _nextIndexToAvoid = _currentStoryIndex;
            }
        }
        else
        {
            _currentCanvas = _mainMenuCanvas;
            _nextIndexToAvoid = _currentStoryIndex;

        }
        EnableCanvas();
        _androidBackButton.SetToInactive();
        Destroy(auxCanvas.gameObject);
        _alreadyBeatenLevel = false;
        SelectNextStory();
    }

    public void AbortStory()
    {
        Canvas auxCanvas = _currentCanvas;
        DisableCanvas();
        _currentCanvas = _storySelectCanvas;
        EnableCanvas();
        _androidBackButton.SetToGoToMain();
        Destroy(auxCanvas.gameObject);
        _alreadyBeatenLevel = false;
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
            if (_unbeatenStoryCanvases.Count == 1) { _nextIndexToAvoid = -1; }
            int rand = _currentStoryIndex;
            do
            {
                //Debug.Log("Current index: " + rand + ", index to avoid: " + _nextIndexToAvoid);
                rand = Random.Range(0, _unbeatenStoryCanvases.Count);
            } while (rand == _nextIndexToAvoid);
            //Debug.Log("Final next index: " + rand);
            _currentStoryIndex = rand;
            _backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        }
        else
        {
            //TODO Quitar los botones de la vista
            //_backgroundImage.sprite = null;
            //_backgroundImage.enabled = false;

            //De momento, si se acaban las historias damos una aleatoria
            int rand = Random.Range(0, _storyCanvases.Count);
            _randomBeatenCanvas = _storyCanvases[rand];
            _backgroundImage.sprite = _backgroundSprites[rand];
        }
    }

}
