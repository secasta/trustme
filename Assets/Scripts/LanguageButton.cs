using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour {

    public SystemLanguage _thisLanguage;
    public Sprite _idleSprite;
    public Sprite _pressedSprite;

    private Image _buttonImage;

    public delegate void LanguageChanged();
    public static event LanguageChanged OnLanguageChanged;


    void Awake()
    {
        _buttonImage = GetComponent<Image>();
        if (!_buttonImage) { Debug.LogError("Image not found in button", this); }
    }

    void Start ()
    {
        if (CurrentSettings.Language == _thisLanguage)
        {
            SwapToPressed();
        }
	}

    void OnEnable()
    {
        LanguageButton.OnLanguageChanged += CheckLanguageAndSwap;
    }

    void OnDisable()
    {
        LanguageButton.OnLanguageChanged -= CheckLanguageAndSwap;
    }

    public void OnButtonPressed()
    {
        CurrentSettings.Language = _thisLanguage;
        
        if (OnLanguageChanged != null)
        {
            OnLanguageChanged();
        }
    }

    void SwapToPressed()
    {
        _buttonImage.sprite = _pressedSprite;
    }

    void SwapToIdle()
    {
        _buttonImage.sprite = _idleSprite;
    }

    void CheckLanguageAndSwap()
    {
        Debug.Log("event arrived");
        if (CurrentSettings.Language == _thisLanguage)
        {
            SwapToPressed();
        }
        else
        {
            SwapToIdle();
        }
    }
}
