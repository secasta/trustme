using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour {

    public List<Canvas> _storyCanvases;
    public Canvas _firstTutorialCanvas;
    public Image _randomButtonImage;
    public List<Sprite> _randomStorySprites;
    public Canvas _mainMenuCanvas;
    public Canvas _settingsCanvas;
    public Canvas _storySelectCanvas;
    public Canvas _languageSelectionCanvas;
    public GameObject _mainBlockingPanel;
    public GameObject _playButtonObject;
    public GameObject _notificationPanel;

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
    private int _numberOfFakeStories = 4;//Maximum is number of stories minus the tutorial and minus one
    private Sprite[] _fakeStoryOptions;
    private Button _randomButton;
    private bool _breakAndGoToTutorial = false;
    private float _timeBetweenRandomPics = 0.2f;

    public delegate void StoryCompleted(int id);
    public static event StoryCompleted OnStoryCompleted;
    public delegate void GoingToMain();
    public static event GoingToMain OnGoingToMain;

    void Awake()
    {
        //Get references
        _storiesScrollRect = _storySelectCanvas.GetComponentInChildren<VerticalScrollRect>();
        if (!_storiesScrollRect) { Debug.LogError("No scroll rect found on children", this); }
        _androidBackButton = FindObjectOfType<AndroidBackButton>();
        if (!_androidBackButton) { Debug.LogError("No android back button script found", this); }
        _randomButton = _randomButtonImage.GetComponent<Button>();
        if (!_randomButton) { Debug.LogError("No button found on random button", this); }

        _fakeStoryOptions = new Sprite[_numberOfFakeStories];
    }

    void Start()
    {
        //Reunlock stories on the album
        if (Game.current != null)
        {
            Game.current.ReportWaitingAchievements();
            Game.current.ReUnlock();
        }
        else
        {
            Debug.LogError("There's no instance of Game");
        }

        //Copy whole lists into unbeaten lists
        _unbeatenStoryCanvases = new List<Canvas>(_storyCanvases);
        _unbeatenBackgroundSprites = new List<Sprite>(_randomStorySprites);

        //Remove previously beaten stories (indexes) from unbeaten lists
        foreach (int storyId in Game.current._unlockedStories)
        {
            //Debug.Log("Removing story " + storyId + " from unbeaten lists.");
            _unbeatenBackgroundSprites.RemoveAt(storyId);
            _unbeatenStoryCanvases.RemoveAt(storyId);
        }

        if (OnGoingToMain != null)
        {
            OnGoingToMain();
        }

        _currentCanvas = _mainMenuCanvas;
        //_mainBlockingPanel.SetActive(true);
        if (Game.current._unlockedStories.Contains(0))//if tutorial has been beaten
        {
            SelectNextStory();
        }
        else
        {
            //Next story will be the tutorial
            _currentStoryIndex = 0;
            //_backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
            SelectFakeOptions();
            _breakAndGoToTutorial = true;
        }
        //StartCoroutine(RollStories());
        
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
            if (_breakAndGoToTutorial)
            {
                _currentCanvas = Instantiate(_firstTutorialCanvas);
                _breakAndGoToTutorial = false;
            }
            else
            {
                _currentCanvas = Instantiate(_unbeatenStoryCanvases[_currentStoryIndex]);
            }
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

            _isStoryAbortable = false;//it could be the other way around, but now it wouldn't return to the main menu
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
        if (OnGoingToMain != null)
        {
            OnGoingToMain();
        }
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
        ResetRandomButton();
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
                if (OnGoingToMain != null)
                {
                    OnGoingToMain();
                }
            }
        }
        else
        {
            _currentCanvas = _mainMenuCanvas;
            _nextIndexToAvoid = _currentStoryIndex;
            if (OnGoingToMain != null)
            {
                OnGoingToMain();
            }
        }
        EnableCanvas();
        _androidBackButton.SetToInactive();
        Destroy(auxCanvas.gameObject);
        _alreadyBeatenLevel = false;
        SelectNextStory();
        
        //StartCoroutine(RollStories());
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
        //_mainBlockingPanel.SetActive(true);
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
            //_backgroundImage.sprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        }
        else
        {
            //De momento, si se acaban las historias damos una aleatoria
            int rand = Random.Range(1, _storyCanvases.Count);//Evitamos que vuelva a salir el tutorial
            _currentStoryIndex = rand;
            _randomBeatenCanvas = _storyCanvases[rand];
            //_backgroundImage.sprite = _backgroundSprites[rand];
        }
        SelectFakeOptions();
    }

    void SelectFakeOptions()
    {
        //0 is not an option
        int rand = 1;
        int backgroundsVectorIndex = 0;
        List<int> storyIndexesUsed = new List<int>();
        bool isIndexAccepted = true;

        Sprite currentStorySprite;
        if (_unbeatenBackgroundSprites.Count > 0)
        {
            currentStorySprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        }
        else
        {
            currentStorySprite = _randomStorySprites[_currentStoryIndex];
        }

        int testLoops = 0;

        while (backgroundsVectorIndex < _numberOfFakeStories)
        {
            isIndexAccepted = true;
            rand = Random.Range(1, _randomStorySprites.Count);

            if (_randomStorySprites[rand] != currentStorySprite)
            {
                foreach (int index in storyIndexesUsed)
                {
                    if (rand == index)
                    {
                        isIndexAccepted = false;
                        break;
                    }
                }

                if (isIndexAccepted)
                {
                    _fakeStoryOptions[backgroundsVectorIndex] = _randomStorySprites[rand];
                    storyIndexesUsed.Add(rand);
                    backgroundsVectorIndex++;
                }
            }
            testLoops++;
            if (testLoops > 25)
            {
                Debug.Log("Too many loops, get the f*** out of here");
                break;
            }
        }
    }

    public void OnRandomStoryButtonPressed()
    {
        StartCoroutine(RollStories());
    }

    IEnumerator RollStories()
    {
        _mainBlockingPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        Sprite currentStorySprite;
        if (_unbeatenBackgroundSprites.Count > 0)
        {
            currentStorySprite = _unbeatenBackgroundSprites[_currentStoryIndex];
        }
        else
        {
            currentStorySprite = _randomStorySprites[_currentStoryIndex];
        }

        _randomButtonImage.sprite = _fakeStoryOptions[0];

        for (int j = 0; j < 3; j++)
        {
            for (int i = 1; i < _fakeStoryOptions.Length; i++)
            {
                yield return new WaitForSeconds(_timeBetweenRandomPics);
                _randomButtonImage.sprite = _fakeStoryOptions[i];
            }
            yield return new WaitForSeconds(_timeBetweenRandomPics);
            if (_breakAndGoToTutorial)
            {
                NextUnbeatenStory();
                break;
            }
            _randomButtonImage.sprite = currentStorySprite;
        }

        if (!_breakAndGoToTutorial)
        {
            //yield return new WaitForSeconds(_timeBetweenRandomPics);
            //_randomButtonImage.sprite = currentStorySprite;

            _randomButton.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _playButtonObject.SetActive(true);    
        }

        _mainBlockingPanel.SetActive(false);
    }

    void ResetRandomButton()
    {
        _randomButtonImage.sprite = _randomStorySprites[0];
        _randomButton.enabled = true;
        _playButtonObject.SetActive(false);
    }

    public void CheckIfAllStoriesUnlocked()
    {
        if (_unbeatenStoryCanvases.Count < 1)
        {
            _notificationPanel.SetActive(true);
        }
    }
}
