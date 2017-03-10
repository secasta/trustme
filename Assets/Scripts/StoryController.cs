using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoryController : MonoBehaviour {

    public Image _introBackground;
    public Image _chapterBackground;
    public Image _outroBackground;
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

    private bool _isResponse1Correct;//Sacar
    private bool _isResponse2Correct;//
    private bool _isResponse3Correct;//
    private Image _currentGuy;//Para saber cuál desactivar

    //Pedir a un parser las frases
    private string _title;
    private List<string> _introSentence = new List<string>();//hacemos listas en caso de que haya más de una opción que se elija de forma aleatoria
    private List<string> _firstReaction = new List<string>();
    private Answer[,] _answers = new Answer[5,3];//5 rondas de 3, para desordenarlas en cada ronda que pasen por una variable intermedia
    private List<string> _outroSentenceGood = new List<string>();
    private List<string> _outroSentenceBad = new List<string>();
    //

    private struct Answer
    {
        string answer;
        bool isTrusted;
        string reaction;
    }

    private Answer _currentAnswer1;//variables intermedias
    private Answer _currentAnswer2;
    private Answer _currentAnswer3;

    void Awake()//En vez de hacerlo manualmente habría que pasar por el parser del txt correspondiente al idioma
    {
        _title = "LA RATA";
        _introSentence.Add("Un día más el chef no hace más que gritarte por no dejar los platos relucientes. Para darle su merecido decides meter un ratón en la olla de su plato estrella.");
        _firstReaction.Add("¿¡Qué le has hecho a mi sopa!?");
        PopulateAnswersStructs();
        _outroSentenceGood.Add("Siento haber dudado de ti. Has demostrado ser un trabajador fiel. Te has ganado un ascenso.");
        _outroSentenceBad.Add("Parece ser que en mi cocina había dos ratas.");
    }

    void Start ()
    {
        _introBackground.enabled = true;
        _chapterTitle.text = _title;
        _chapterTitle.enabled = true;
        _chapterIntroText.text = _introSentence[0];
        _chapterIntroText.enabled = true;
        _startButton.SetActive(true);
	}

    public void OnStartButtonPressed()
    {
        _chapterBackground.enabled = true;
        _introBackground.enabled = false;
        _lowerPanel.SetActive(false);
        _middlePanel.SetActive(true);
        _chapterTitle.enabled = false;
        _gauge.SetActive(true);
        _guyAngry.enabled = true;
        _currentGuy = _guyAngry;
        _midPanelText.text = _firstReaction[0];//Se tendría que escoger aleatoriamente si se quiere hacer general  
    }

    public void OnFinishButtonPressed()
    {

    }

    public void OnReactionButtonPressed()
    {
        Debug.Log("middle button pressed");
    }

    public void OnFirstResponsePressed()
    {
        if (_isResponse1Correct)
        {
            CorrectAnswerBehavior();
        }
        else
        {
            WrongAnswerBehavior();
        }
    }

    public void OnSecondResponsePressed()
    {
        if (_isResponse2Correct)
        {
            CorrectAnswerBehavior();
        }
        else
        {
            WrongAnswerBehavior();
        }
    }

    public void OnThirdResponsePressed()
    {
        if (_isResponse3Correct)
        {
            CorrectAnswerBehavior();
        }
        else
        {
            WrongAnswerBehavior();
        }
    }

    void CorrectAnswerBehavior()
    {

    }

    void WrongAnswerBehavior()
    {

    }

    void PopulateAnswersStructs()
    {
        //TODO Tocho para popular todos los structs, cuando todas las transiciones funcionen guay habrá que hacer esto bien
    }
}
