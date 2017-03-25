using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoryController : MonoBehaviour {

    public Image _introBackground;
    public Image _chapterBackground;
    public Image _winOutroBackground;
    public Image[] _loseOutroBackgrounds;
    public Text _chapterTitle;
    public Text _chapterIntroText;
    public GameObject _startButton;
    public GameObject _middlePanel;
    public GameObject _lowerPanel;
    public GameObject _gauge;
    public Text _midPanelText;
    public Image _guyIdle;
    public Image _guyWaiting;
    public Image _guyThinking;
    public Image _guyAngry;
    public Image _redFilter;
    public Image _greenFilter;
    public GameObject _answerOptions;
    public AnswerButtonScript _answerButton1;
    public AnswerButtonScript _answerButton2;
    public AnswerButtonScript _answerButton3;
    public GameObject _liarVerdict;
    public GameObject _trustedVerdict;
    public Text _outroText;
    public GameObject _finishButton;

    private Image _currentGuy;//Para saber cuál desactivar
    private float _timeForReaction = 1.1f;
    private GaugeManager _gaugeManager;
    private TextParser _parser;


    void Awake()//En vez de hacerlo manualmente habría que pasar por el parser del txt correspondiente al idioma
    {
        _parser = GetComponent<TextParser>();
        if (!_parser) { Debug.LogWarning("Couldn't find text parser!"); }
        _gaugeManager = _gauge.GetComponent<GaugeManager>();
        if (!_gaugeManager) { Debug.LogWarning("Couldn't find gauge manager!"); }
    }

    void Start ()
    {
        _introBackground.enabled = true;
        _chapterTitle.text = _parser.GetTitle();
        _chapterTitle.enabled = true;
        _chapterIntroText.text = _parser.GetIntroSentence();
        _chapterIntroText.enabled = true;
        _startButton.SetActive(true);
	}

    public void OnStartButtonPressed()
    {
        _startButton.SetActive(false);
        _chapterBackground.enabled = true;
        _introBackground.enabled = false;
        _lowerPanel.SetActive(false);
        _middlePanel.SetActive(true);
        _chapterTitle.enabled = false;
        _gauge.SetActive(true);
        _guyWaiting.enabled = true;
        _currentGuy = _guyWaiting;
        _chapterIntroText.enabled = false;
        _midPanelText.text = _parser.GetFirstReaction(); 
    }

    public void OnFinishButtonPressed()
    {
        CanvasManager canvasManager = FindObjectOfType<CanvasManager>();
        if (!canvasManager)
        {
            Debug.LogError("No Canvas Manager found!");
        }
        else
        {
            canvasManager.FinishStory();
        }
    }

    public void OnReactionButtonPressed()
    {
        if (_parser.GetRound() > 4) { return; }
        _parser.PopulateCurrentAnswers();
        _middlePanel.SetActive(false);
        _lowerPanel.SetActive(true);
        _answerOptions.SetActive(true);
        _answerButton1.SetText(_parser.GetCurrentAnswer(1));
        _answerButton2.SetText(_parser.GetCurrentAnswer(2));
        _answerButton3.SetText(_parser.GetCurrentAnswer(3));

    }

    public void OnFirstResponsePressed()
    {
        if (_parser.GetCurrentTrustBoolean(1))
        {
            _answerButton1.SwapSpriteToGreen();//Dar un tiempo antes de mostrar si se ha acertado o no
            CorrectAnswerBehavior(_parser.GetCurrentReaction(1));
        }
        else
        {
            _answerButton1.SwapSpriteToRed();
            WrongAnswerBehavior(_parser.GetCurrentReaction(1));
        }
    }

    public void OnSecondResponsePressed()
    {
        if (_parser.GetCurrentTrustBoolean(2))
        {
            _answerButton2.SwapSpriteToGreen();
            CorrectAnswerBehavior(_parser.GetCurrentReaction(2));
        }
        else
        {
            _answerButton2.SwapSpriteToRed();
            WrongAnswerBehavior(_parser.GetCurrentReaction(2));
        }
    }

    public void OnThirdResponsePressed()
    {
        if (_parser.GetCurrentTrustBoolean(3))
        {
            _answerButton3.SwapSpriteToGreen();
            CorrectAnswerBehavior(_parser.GetCurrentReaction(3));
        }
        else
        {
            _answerButton3.SwapSpriteToRed();
            WrongAnswerBehavior(_parser.GetCurrentReaction(3));
        }
    }

    void CorrectAnswerBehavior(string reaction)
    {
        _answerButton1.DisableButton();
        _answerButton2.DisableButton();
        _answerButton3.DisableButton();

        _currentGuy.enabled = false;
        _currentGuy = _guyThinking;
        _currentGuy.enabled = true;
        _greenFilter.enabled = true;

        if (_gaugeManager.AddGreenBarAndContinue())
        {
            StartCoroutine(ActivateReaction(reaction));
        }
        else
        {
            StartCoroutine(WinOutro());
        }
    }

    void WrongAnswerBehavior(string reaction)
    {
        _answerButton1.DisableButton();
        _answerButton2.DisableButton();
        _answerButton3.DisableButton();

        _currentGuy.enabled = false;
        _currentGuy = _guyAngry;
        _currentGuy.enabled = true;
        _redFilter.enabled = true;

        if (_gaugeManager.AddRedBarAndContinue())
        {
            StartCoroutine(ActivateReaction(reaction));
        }
        else
        {
            StartCoroutine(LoseOutro());
        }
    }

    IEnumerator ActivateReaction(string reaction)
    {
        yield return new WaitForSeconds(_timeForReaction);
        _answerButton1.SwapSpriteToNormal();
        _answerButton2.SwapSpriteToNormal();
        _answerButton3.SwapSpriteToNormal();
        _answerButton1.EnableButton();
        _answerButton2.EnableButton();
        _answerButton3.EnableButton();
        _redFilter.enabled = false;
        _greenFilter.enabled = false;
        _lowerPanel.SetActive(false);
        _middlePanel.SetActive(true);
        _midPanelText.text = reaction;
        _currentGuy.enabled = false;
        if (_currentGuy == _guyAngry) { _currentGuy = _guyWaiting; }
        if (_currentGuy == _guyThinking) { _currentGuy = _guyIdle; }
        _currentGuy.enabled = true;
    }

    IEnumerator WinOutro()
    {
        yield return new WaitForSeconds(_timeForReaction);
        _trustedVerdict.SetActive(true);
        _winOutroBackground.enabled = true;
        _chapterBackground.enabled = false;
        _currentGuy.enabled = false;
        _answerOptions.SetActive(false);
        _outroText.text = _parser.GetOutroSentenceGood();
        _outroText.enabled = true;
        _finishButton.SetActive(true);

        //TODO desbloquear pantalla y guardar partida
    }

    IEnumerator LoseOutro()
    {
        yield return new WaitForSeconds(_timeForReaction);
        _liarVerdict.SetActive(true);
        PickOneRandomImage(_loseOutroBackgrounds).enabled = true;
        _chapterBackground.enabled = false;
        _currentGuy.enabled = false;
        _answerOptions.SetActive(false);
        _outroText.text = _parser.GetOutroSentenceBad();
        _outroText.enabled = true;
        _finishButton.SetActive(true);
    }

    Image PickOneRandomImage(Image[] images)
    {
        int numElements = images.Length;
        int randomIndex = Random.Range(0, numElements);
        return images[randomIndex];
    }   
}
