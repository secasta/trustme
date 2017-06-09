using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityBackground : MonoBehaviour {

    public Sprite _morningSprite;
    public Sprite _afternoonSprite;
    public Sprite _nightSprite;

    private Image _thisImage;

    void Awake()
    {
        _thisImage = GetComponent<Image>();
        if (!_thisImage) { Debug.LogError("Image not found on city background", this); }
    }

	void Start ()
    {
        SetSpriteToTimeOfTheDay();
	}

    void SetSpriteToTimeOfTheDay()
    {
        int currentHour = System.DateTime.Now.Hour;
        if (currentHour < 6 || currentHour >= 21)
        {
            //Debug.Log("Night time! " + currentHour + " h");
            _thisImage.sprite = _nightSprite;
        }
        else
        {
            if (currentHour < 17)
            {
                //Debug.Log("Day time! " + currentHour + " h");
                _thisImage.sprite = _morningSprite;
            }
            else
            {
                //Debug.Log("Afternoon! " + currentHour + " h");
                _thisImage.sprite = _afternoonSprite;
            }
        }
    }

    void OnEnable()
    {
        CanvasManager.OnGoingToMain += SetSpriteToTimeOfTheDay;
    }

    void OnDisable()
    {
        CanvasManager.OnGoingToMain -= SetSpriteToTimeOfTheDay;
    }
	
}
