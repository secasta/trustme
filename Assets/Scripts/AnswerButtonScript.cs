using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerButtonScript : MonoBehaviour {

    public Sprite _greenButtonSprite;
    public Sprite _redButtonSprite;
    public Sprite _normalButtonSprite;
    public Sprite _pressedButtonSprite;

    private Button _answerButton;
    private Image _answerButtonImage;
    private Text _answerButtonText;

    void Awake () {
        _answerButton = GetComponent<Button>();
        if (!_answerButton) { Debug.LogWarning("No button found", this); }
        _answerButtonImage = GetComponent<Image>();
        if (!_answerButtonImage) { Debug.LogWarning("No image found", this); }
        _answerButtonText = GetComponentInChildren<Text>();
        if (!_answerButtonText) { Debug.LogWarning("No text found in child", this); }
	}

    public void DisableButton()
    {
        _answerButton.enabled = false;
    }

    public void EnableButton()
    {
        _answerButton.enabled = true;
    }

    public IEnumerator SwapSpriteToRed(float time)
    {
        yield return new WaitForSeconds(time);
        _answerButtonImage.sprite = _redButtonSprite;
    }

    public IEnumerator SwapSpriteToGreen(float time)
    {
        yield return new WaitForSeconds(time);
        _answerButtonImage.sprite = _greenButtonSprite;
    }

    public void SwapSpriteToPressed()
    {
        _answerButtonImage.sprite = _pressedButtonSprite;
    }

    public void SwapSpriteToNormal()
    {
        _answerButtonImage.sprite = _normalButtonSprite;
    }

    public void SetText(string text)
    {
        _answerButtonText.text = text;
    }
}
