using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParser : MonoBehaviour {

    public TextAsset _spanishTextFile;
    public TextAsset _englishTextFile;

    public struct Answer
    {
        public string answer;
        public bool isTrusted;
        public string reaction;
    }

    private TextAsset _fileToParse;
    private string _title;
    private List<string> _introSentence = new List<string>();//hacemos listas en caso de que haya más de una opción que se elija de forma aleatoria
    private List<string> _firstReaction = new List<string>();
    private Answer[,] _answers = new Answer[5, 3];//5 rondas de 3
    private List<string> _outroSentenceGood = new List<string>();
    private List<string> _outroSentenceBad = new List<string>();

    private Answer _currentAnswer1;
    private Answer _currentAnswer2;
    private Answer _currentAnswer3;

    private int _round = 0;


    void Awake()
    {
        switch (CurrentSettings.Language)
        {
            case SystemLanguage.Spanish:
                _fileToParse = _spanishTextFile;
                break;
            default:
                _fileToParse = _englishTextFile;
                break;
        }
        
    
        ParseFile();
	}

    public int GetStoryID()
    {
        int id = int.Parse(_fileToParse.name.Substring(0, 3));
        return id;
    }

    public int GetRound()
    {
        return _round;
    }

    public void PopulateCurrentAnswers()
    {
        int firstIndex = Random.Range(0, 3);
        int secondIndex = firstIndex;
        int thirdIndex = firstIndex;
        while (secondIndex == firstIndex) { secondIndex = Random.Range(0, 3); }
        while (thirdIndex == firstIndex || thirdIndex == secondIndex) { thirdIndex = Random.Range(0, 3); }

        _currentAnswer1 = _answers[_round, firstIndex];
        _currentAnswer2 = _answers[_round, secondIndex];
        _currentAnswer3 = _answers[_round, thirdIndex];

        _round++;
    }

    public string GetTitle()
    {
        return _title;
    }

    public string GetIntroSentence()
    {
        return PickOneRandomString(_introSentence);
    }

    public string GetFirstReaction()
    {
        return PickOneRandomString(_firstReaction);
    }

    public string GetOutroSentenceGood()
    {
        return PickOneRandomString(_outroSentenceGood);
    }

    public string GetOutroSentenceBad()
    {
        return PickOneRandomString(_outroSentenceBad);
    }

    public string GetCurrentAnswer(int answerPosition)
    {
        switch (answerPosition)
        {
            case 1:
                return _currentAnswer1.answer;
            case 2:
                return _currentAnswer2.answer;
            case 3:
                return _currentAnswer3.answer;
        }
        Debug.LogWarning("answer position must be between 1 and 3");
        return _currentAnswer1.answer;
    }

    public bool GetCurrentTrustBoolean(int answerPosition)
    {
        switch (answerPosition)
        {
            case 1:
                return _currentAnswer1.isTrusted;
            case 2:
                return _currentAnswer2.isTrusted;
            case 3:
                return _currentAnswer3.isTrusted;
        }
        Debug.LogWarning("answer position must be between 1 and 3");
        return _currentAnswer1.isTrusted;
    }

    public string GetCurrentReaction(int answerPosition)
    {
        switch (answerPosition)
        {
            case 1:
                return _currentAnswer1.reaction;
            case 2:
                return _currentAnswer2.reaction;
            case 3:
                return _currentAnswer3.reaction;
        }
        Debug.LogWarning("answer position must be between 1 and 3");
        return _currentAnswer1.reaction;
    }

    string PickOneRandomString(List<string> list)
    {
        int numElements = list.Count;
        int randomIndex = Random.Range(0, numElements);
        return list[randomIndex];
    }

    void ParseFile()
    {
        string[] rawTextLines = _fileToParse.text.Split('\n');
        foreach (string rawTextLine in rawTextLines)
        {
            ParseLine(rawTextLine);
        }
    }

    void ParseLine(string textLine)
    {
        if (textLine.StartsWith("//")) { return; }//ignore
        else if (textLine.StartsWith("title")) { AssignTitle(textLine); }
        else if (textLine.StartsWith("intro sentence")) { AddIntroSentence(textLine); }
        else if (textLine.StartsWith("first reaction")) { AddFirstReaction(textLine); }
        else if (textLine.StartsWith("outro sentence good")) { AddOutroSentenceGood(textLine); }
        else if (textLine.StartsWith("outro sentence bad")) { AddOutroSentenceBad(textLine); }
        else if (textLine.StartsWith("answer")) { AssignAnswer(textLine); }
        else if (textLine.StartsWith("is trusted")) { AssignBoolean(textLine); }
        else if (textLine.StartsWith("reaction")) { AssignReaction(textLine); }
        else { Debug.LogWarning("Careful! There's a line on the text file with a wrong format."); }
    }

    void AssignTitle(string textLine)
    {
        int index = textLine.IndexOf("=");
        _title = textLine.Substring(index + 2);
    }

    void AddIntroSentence(string textLine)
    {
        int index = textLine.IndexOf("=");
        _introSentence.Add(textLine.Substring(index + 2));
    }

    void AddFirstReaction(string textLine)
    {
        int index = textLine.IndexOf("=");
        _firstReaction.Add(textLine.Substring(index + 2));
    }

    void AddOutroSentenceGood(string textLine)
    {
        int index = textLine.IndexOf("=");
        _outroSentenceGood.Add(textLine.Substring(index + 2));
    }

    void AddOutroSentenceBad(string textLine)
    {
        int index = textLine.IndexOf("=");
        _outroSentenceBad.Add(textLine.Substring(index + 2));
    }

    void AssignAnswer(string textLine)
    {
        int indexOfEqualSign = textLine.IndexOf("=");
        int indexOfOpenBracket = textLine.IndexOf("[");
        int indexOfCloseBracket = textLine.IndexOf("]");
        int roundIndex = int.Parse(textLine.Substring(indexOfOpenBracket + 1, 1));
        int positionIndex = int.Parse(textLine.Substring(indexOfCloseBracket - 1, 1));
        _answers[roundIndex, positionIndex].answer = textLine.Substring(indexOfEqualSign + 2);
    }

    void AssignBoolean(string textLine)
    {
        int indexOfEqualSign = textLine.IndexOf("=");
        int indexOfOpenBracket = textLine.IndexOf("[");
        int indexOfCloseBracket = textLine.IndexOf("]");
        int roundIndex = int.Parse(textLine.Substring(indexOfOpenBracket + 1, 1));
        int positionIndex = int.Parse(textLine.Substring(indexOfCloseBracket - 1, 1));
        string word = textLine.Substring(indexOfEqualSign + 2);
        
        if (word.Contains("true"))
        {
            _answers[roundIndex, positionIndex].isTrusted = true;
        }
        else if(word.Contains("false"))
        {
            _answers[roundIndex, positionIndex].isTrusted = false;
        }
    }

    void AssignReaction(string textLine)
    {
        int indexOfEqualSign = textLine.IndexOf("=");
        int indexOfOpenBracket = textLine.IndexOf("[");
        int indexOfCloseBracket = textLine.IndexOf("]");
        int roundIndex = int.Parse(textLine.Substring(indexOfOpenBracket + 1, 1));
        int positionIndex = int.Parse(textLine.Substring(indexOfCloseBracket - 1, 1));
        _answers[roundIndex, positionIndex].reaction = textLine.Substring(indexOfEqualSign + 2);
    }
}
