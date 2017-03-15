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
    public GameObject _answerOptions;
    public AnswerButtonScript _answerButton1;
    public AnswerButtonScript _answerButton2;
    public AnswerButtonScript _answerButton3;
    public GameObject _liarVerdict;
    public GameObject _trustedVerdict;
    public Text _outroText;
    public GameObject _finishButton;

    private Image _currentGuy;//Para saber cuál desactivar
    private int _round = 0;
    private float _timeForReaction = 1;
    private GaugeManager _gaugeManager;

    public struct Answer
    {
        public string answer;
        public bool isTrusted;
        public string reaction;
    }

    //Pedir a un parser las frases
    private string _title;
    private List<string> _introSentence = new List<string>();//hacemos listas en caso de que haya más de una opción que se elija de forma aleatoria
    private List<string> _firstReaction = new List<string>();
    private Answer[,] _answers = new Answer[5,3];//5 rondas de 3, para desordenarlas en cada ronda que pasen por una variable intermedia
    private List<string> _outroSentenceGood = new List<string>();
    private List<string> _outroSentenceBad = new List<string>();
    //

    private Answer _currentAnswer1;//variables intermedias
    private Answer _currentAnswer2;
    private Answer _currentAnswer3;

    void Awake()//En vez de hacerlo manualmente habría que pasar por el parser del txt correspondiente al idioma
    {
        _gaugeManager = _gauge.GetComponent<GaugeManager>();
        if (!_gaugeManager) { Debug.LogWarning("Couldn't find gauge manager!"); }

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
        _startButton.SetActive(false);
        _chapterBackground.enabled = true;
        _introBackground.enabled = false;
        _lowerPanel.SetActive(false);
        _middlePanel.SetActive(true);
        _chapterTitle.enabled = false;
        _gauge.SetActive(true);
        _guyAngry.enabled = true;
        _currentGuy = _guyAngry;
        _chapterIntroText.enabled = false;
        _midPanelText.text = _firstReaction[0];//Se tendría que escoger aleatoriamente si se quiere hacer general 
    }

    public void OnFinishButtonPressed()
    {
        FindObjectOfType<CanvasManager>().FinishStory();
    }

    public void OnReactionButtonPressed()
    {
        if (_round > 4) { return; }
        PopulateCurrentAnswers();
        _currentGuy.enabled = false;
        if (_currentGuy == _guyAngry) { _currentGuy = _guyWaiting; }
        if (_currentGuy == _guyThinking) { _currentGuy = _guyIdle; }
        _currentGuy.enabled = true;
        _middlePanel.SetActive(false);
        _lowerPanel.SetActive(true);
        _answerOptions.SetActive(true);
        _answerButton1.SetText(_currentAnswer1.answer);
        _answerButton2.SetText(_currentAnswer2.answer);
        _answerButton3.SetText(_currentAnswer3.answer);

    }

    public void OnFirstResponsePressed()
    {
        if (_currentAnswer1.isTrusted)
        {
            _answerButton1.SwapSpriteToGreen();
            CorrectAnswerBehavior(_currentAnswer1.reaction);
        }
        else
        {
            _answerButton1.SwapSpriteToRed();
            WrongAnswerBehavior(_currentAnswer1.reaction);
        }
    }

    public void OnSecondResponsePressed()
    {
        if (_currentAnswer2.isTrusted)
        {
            _answerButton2.SwapSpriteToGreen();
            CorrectAnswerBehavior(_currentAnswer2.reaction);
        }
        else
        {
            _answerButton2.SwapSpriteToRed();
            WrongAnswerBehavior(_currentAnswer2.reaction);
        }
    }

    public void OnThirdResponsePressed()
    {
        if (_currentAnswer3.isTrusted)
        {
            _answerButton3.SwapSpriteToGreen();
            CorrectAnswerBehavior(_currentAnswer3.reaction);
        }
        else
        {
            _answerButton3.SwapSpriteToRed();
            WrongAnswerBehavior(_currentAnswer3.reaction);
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

    }

    IEnumerator WinOutro()
    {
        yield return new WaitForSeconds(_timeForReaction);
        _trustedVerdict.SetActive(true);
        _outroBackground.enabled = true;
        _chapterBackground.enabled = false;
        _currentGuy.enabled = false;
        _answerOptions.SetActive(false);
        _outroText.text = _outroSentenceGood[0];
        _outroText.enabled = true;
        _finishButton.SetActive(true);

        //TODO desbloquear pantalla y guardar partida
    }

    IEnumerator LoseOutro()
    {
        yield return new WaitForSeconds(_timeForReaction);
        _liarVerdict.SetActive(true);
        _outroBackground.enabled = true;
        _chapterBackground.enabled = false;
        _currentGuy.enabled = false;
        _answerOptions.SetActive(false);
        _outroText.text = _outroSentenceBad[0];
        _outroText.enabled = true;
        _finishButton.SetActive(true);
    }

    void PopulateCurrentAnswers()
    {
        //TODO Hacer aleatorio
        _currentAnswer1 = _answers[_round, 0];
        _currentAnswer2 = _answers[_round, 1];
        _currentAnswer3 = _answers[_round, 2];
        _round++;
    }

    void PopulateAnswersStructs()
    {
        //Tocho para poblar todos los structs, cuando todas las transiciones funcionen guay habrá que hacer esto bien
        _answers[0, 0].answer = "¡Yo no he sido!";
        _answers[0, 0].isTrusted = true;
        _answers[0, 0].reaction = "Mmmm... Pues hay testigos que aseguran haberte visto merodeando por aquí.";

        _answers[0, 1].answer = "He visto a Norman merodeando por la zona.";
        _answers[0, 1].isTrusted = false;
        _answers[0, 1].reaction = "Pero si Norman hoy no trabaja. Además, hay testigos que aseguran haberte visto merodeando por aquí.";

        _answers[0, 2].answer = "¡Has sido tú!";
        _answers[0, 2].isTrusted = false;
        _answers[0, 2].reaction = "¿¡Cómo quieres que estropee mi propia receta!? Además, hay testigos que aseguran haberte visto merodeando por aquí.";


        _answers[1, 0].answer = "¿Quién te lo ha dicho?";
        _answers[1, 0].isTrusted = true;
        _answers[1, 0].reaction = "Jaime, pero es cierto que es bastante despistado. ¡No me confundas! Por tu culpa sanidad nos va a cerrar el restaurante.";

        _answers[1, 1].answer = "Mi cara es muy común. Alguien se habrá confundido.";
        _answers[1, 1].isTrusted = false;
        _answers[1, 1].reaction = "Eres el más alto de la cocina. Es fácil reconocerte... Por tu culpa sanidad nos va a cerrar el restaurante.";

        _answers[1, 2].answer = "¡No era yo! Ha sido mi hermano gemelo.";
        _answers[1, 2].isTrusted = false;
        _answers[1, 2].reaction = "¿Me tomas por tonto? Por tu culpa sanidad nos va a cerrar el restaurante.";


        _answers[2, 0].answer = "¡Ha sido Jaime!";
        _answers[2, 0].isTrusted = true;
        _answers[2, 0].reaction = "¿Tienes pruebas? Creo que sólo quieres escaquearte... ¡Confiesa!";

        _answers[2, 1].answer = "¿Por qué no hacemos como si no hubiera pasado nada?";
        _answers[2, 1].isTrusted = false;
        _answers[2, 1].reaction = "No te vas a escapar tan fácilmente de ésta. ¡Confiesa!";

        _answers[2, 2].answer = "Sanidad no tiene por qué enterarse.";
        _answers[2, 2].isTrusted = false;
        _answers[2, 2].reaction = "Pero yo sí que me he enterado. ¡Confiesa!";


        _answers[3, 0].answer = "Jaime te tiene envidia. Siempre ha querido tu puesto.";
        _answers[3, 0].isTrusted = true;
        _answers[3, 0].reaction = "¿Cómo sabes tú eso? Quiero saber la verdad.";

        _answers[3, 1].answer = "Quizá ha sido un accidente.";
        _answers[3, 1].isTrusted = false;
        _answers[3, 1].reaction = "El accidente fue contratarte... Quiero saber la verdad.";

        _answers[3, 2].answer = "Tienes enemigos en esta cocina.";
        _answers[3, 2].isTrusted = false;
        _answers[3, 2].reaction = "Y tú parece que eres uno de ellos. Quiero saber la verdad.";


        _answers[4, 0].answer = "Me dijo que estaba cansado de tus gritos. ¡Yo no haría algo así!";
        _answers[4, 0].isTrusted = true;
        _answers[4, 0].reaction = "Este mensaje no debería estar apareciendo en pantalla. ¡Ups!";

        _answers[4, 1].answer = "De todos modos, tu receta apesta.";
        _answers[4, 1].isTrusted = false;
        _answers[4, 1].reaction = "Este mensaje no debería estar apareciendo en pantalla. ¡Ups!";

        _answers[4, 2].answer = "Creo que te precipitas asumiendo que he sido yo.";
        _answers[4, 2].isTrusted = false;
        _answers[4, 2].reaction = "Este mensaje no debería estar apareciendo en pantalla. ¡Ups!";
    }
}
