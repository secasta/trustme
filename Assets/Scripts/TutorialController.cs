using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextParser))]
public class TutorialController : MonoBehaviour {

    public Image _chapterBackground;
    public Image _winOutroBackground;
    public GameObject _upperPanel;
    public GameObject _middlePanel;
    public GameObject _lowerPanel;
    public GameObject _gaugePrefab;
    public Text _midPanelText;
    public Button _midPanelButton;
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
    public GameObject _trustedVerdict;
    public Text _outroText;
    public GameObject _finishButton;

    private Image _currentGuy;//Para saber cuál desactivar
    private float _timeForReaction = 1.1f;
    private float _timeUntilCheckResponse = 0.2f;
    private float _deadTimeBetweenTransitions = 0.4f;
    private GaugeManager _gaugeManager;
    private TextParser _parser;
    private CanvasManager _canvasManager;
    private bool _isLevelBeaten = false;
    private LeaveStoryButton _leaveStoryButton;
    private AndroidBackButton _androidBackButton;
    private GameObject _gauge;
    private bool _isResponsePressed = false;
    private bool _isReactionPressed = false;



    void Awake()//En vez de hacerlo manualmente habría que pasar por el parser del txt correspondiente al idioma
    {
        _parser = GetComponent<TextParser>();
        if (!_parser) { Debug.LogError("Couldn't find text parser!"); }

        _gauge = Instantiate(_gaugePrefab);
        _gauge.transform.SetParent(_upperPanel.transform, false);
        _gaugeManager = _gauge.GetComponent<GaugeManager>();
        if (!_gaugeManager) { Debug.LogError("Couldn't find gauge manager!"); }

        _canvasManager = FindObjectOfType<CanvasManager>();
        if (!_canvasManager) { Debug.LogError("No Canvas Manager found!", this); }
        _androidBackButton = FindObjectOfType<AndroidBackButton>();
        if (!_androidBackButton) { Debug.LogError("No android back button found", this); }
    }

    void Start()
    {
        //Remove waiting times when testing
        if (_canvasManager.testing)
        {
            _timeForReaction = 0;
            _timeUntilCheckResponse = 0;
            _deadTimeBetweenTransitions = 0;
        }

        _midPanelButton.enabled = false;
        StartCoroutine(EnableMidPanelButton());
        _chapterBackground.enabled = true;
        _lowerPanel.SetActive(false);
        _middlePanel.SetActive(true);
        _gauge.SetActive(true);
        _guyWaiting.enabled = true;
        _currentGuy = _guyWaiting;
        _midPanelText.text = _parser.GetFirstReaction();

        _androidBackButton.SetToInactive();
    }

    public void OnFinishButtonPressed()
    {
        _canvasManager.FinishStory(_isLevelBeaten, _parser.GetStoryID());
    }

    public void OnReactionButtonPressed()
    {
        if (!_isReactionPressed)
        {
            _isReactionPressed = true;
            if (_parser.GetRound() > 4) { return; }
            _parser.PopulateCurrentAnswers();
            _middlePanel.SetActive(false);
            _lowerPanel.SetActive(true);
            _answerOptions.SetActive(true);
            _answerButton1.SetText(_parser.GetCurrentAnswer(1));
            _answerButton2.SetText(_parser.GetCurrentAnswer(2));
            _answerButton3.SetText(_parser.GetCurrentAnswer(3));

            _isResponsePressed = false;
            StartCoroutine(EnableAnswerButtons());
        }
        else
        {
            Debug.Log("More than one reaction clicks");
        }
    }

    public void OnFirstResponsePressed()
    {
        if (!_isResponsePressed)//nos aseguramos que no se entre dos veces
        {
            _isResponsePressed = true;
            DisableAllResponseButtons();
            _answerButton1.SwapSpriteToPressed();
            if (_parser.GetCurrentTrustBoolean(1))
            {
                StartCoroutine(_answerButton1.SwapSpriteToGreen(_timeUntilCheckResponse));
                StartCoroutine(CorrectAnswerBehavior(_parser.GetCurrentReaction(1)));
            }
            else
            {
                StartCoroutine(_answerButton1.SwapSpriteToRed(_timeUntilCheckResponse));
                StartCoroutine(WrongAnswerBehavior(_parser.GetCurrentReaction(1)));
            }
        }
        else
        {
            Debug.Log("Capturing double click bug");
        }
    }

    public void OnSecondResponsePressed()
    {
        if (!_isResponsePressed)
        {
            _isResponsePressed = true;
            DisableAllResponseButtons();
            _answerButton2.SwapSpriteToPressed();
            if (_parser.GetCurrentTrustBoolean(2))
            {
                StartCoroutine(_answerButton2.SwapSpriteToGreen(_timeUntilCheckResponse));
                StartCoroutine(CorrectAnswerBehavior(_parser.GetCurrentReaction(2)));
            }
            else
            {
                StartCoroutine(_answerButton2.SwapSpriteToRed(_timeUntilCheckResponse));
                StartCoroutine(WrongAnswerBehavior(_parser.GetCurrentReaction(2)));
            }
        }
        else
        {
            Debug.Log("Capturing double click bug");
        }
    }

    public void OnThirdResponsePressed()
    {
        if (!_isResponsePressed)
        {
            _isResponsePressed = true;
            DisableAllResponseButtons();
            _answerButton3.SwapSpriteToPressed();
            if (_parser.GetCurrentTrustBoolean(3))
            {
                StartCoroutine(_answerButton3.SwapSpriteToGreen(_timeUntilCheckResponse));
                StartCoroutine(CorrectAnswerBehavior(_parser.GetCurrentReaction(3)));
            }
            else
            {
                StartCoroutine(_answerButton3.SwapSpriteToRed(_timeUntilCheckResponse));
                StartCoroutine(WrongAnswerBehavior(_parser.GetCurrentReaction(3)));
            }
        }
        else
        {
            Debug.Log("Capturing double click bug");
        }
    }

    private void DisableAllResponseButtons()
    {
        _answerButton1.DisableButton();
        _answerButton2.DisableButton();
        _answerButton3.DisableButton();
    }

    IEnumerator CorrectAnswerBehavior(string reaction)
    {
        yield return new WaitForSeconds(_timeUntilCheckResponse);

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

    IEnumerator WrongAnswerBehavior(string reaction)
    {
        yield return new WaitForSeconds(_timeUntilCheckResponse);

        _currentGuy.enabled = false;
        _currentGuy = _guyAngry;
        _currentGuy.enabled = true;
        _redFilter.enabled = true;

        if (_gaugeManager.AddRedBarAndContinue())//in this case we'll always activate the next reaction even if all red bars are active (which shouldn't happen)
        {
            StartCoroutine(ActivateReaction(reaction));
        }
        else
        {
            StartCoroutine(ActivateReaction(reaction));
        }
    }

    IEnumerator ActivateReaction(string reaction)
    {
        _isReactionPressed = false;
        _midPanelButton.enabled = false;
        yield return new WaitForSeconds(_timeForReaction);
        StartCoroutine(EnableMidPanelButton());
        _answerButton1.SwapSpriteToNormal();
        _answerButton2.SwapSpriteToNormal();
        _answerButton3.SwapSpriteToNormal();
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

    IEnumerator EnableAnswerButtons()
    {
        yield return new WaitForSeconds(_deadTimeBetweenTransitions);
        _answerButton1.EnableButton();
        _answerButton2.EnableButton();
        _answerButton3.EnableButton();
    }

    IEnumerator EnableMidPanelButton()
    {
        yield return new WaitForSeconds(_deadTimeBetweenTransitions);
        _midPanelButton.enabled = true;
    }

    IEnumerator WinOutro()
    {
        _isLevelBeaten = true;
        yield return new WaitForSeconds(_timeForReaction);
        _trustedVerdict.SetActive(true);
        _winOutroBackground.enabled = true;
        _chapterBackground.enabled = false;
        _currentGuy.enabled = false;
        _answerOptions.SetActive(false);
        _outroText.text = _parser.GetOutroSentenceGood();
        _outroText.enabled = true;
        _finishButton.SetActive(true);

        //modificar clase a guardar (la partida se guarda automáticamente desde Game)
        Game.current.AddToUnlocked(_parser.GetStoryID());
    }
}
