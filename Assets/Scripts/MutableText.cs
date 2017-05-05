using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutableText : MonoBehaviour {

    public enum LookupWord
    {
        PLAY_,
        START_,
        LIAR_,
        TRUSTED_,
        NEXT_
    }

    public LookupWord _textDisplayed = LookupWord.PLAY_;

    private Text _thisText;

    void Awake()
    {
        _thisText = GetComponent<Text>();
        if (!_thisText) { Debug.LogError("No Text component found", this); }
    }

	void Start ()
    {
        DisplayNewText();
    }

    void OnEnable()
    {
        LanguageButton.OnLanguageChanged += DisplayNewText;
    }

    void OnDisable()
    {
        LanguageButton.OnLanguageChanged -= DisplayNewText;
    }

    void DisplayNewText()
    {
        string wordToLookup = _textDisplayed.ToString() + CurrentSettings.Language.ToString();
        //Debug.Log(wordToLookup);
        _thisText.text = GlobalText.DICTIONARY[wordToLookup];
    }
}
