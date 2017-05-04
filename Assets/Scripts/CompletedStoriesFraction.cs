using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletedStoriesFraction : MonoBehaviour {

    public int _totalStories;

    private Text _fractionText;
    private int _numberOfUnlockedStories;

    void Awake()
    {
        _fractionText = GetComponent<Text>();
        if (!_fractionText) { Debug.LogError("No text component found!", this); }
    }

	void Start ()
    {
        _numberOfUnlockedStories = Game.current._unlockedStories.Count;
        _fractionText.text = _numberOfUnlockedStories + "/" + _totalStories;
	}

    void OnEnable()
    {
        CanvasManager.OnStoryCompleted += IncrementNumberOfUnlockedStories;
    }

    void OnDisable()
    {
        CanvasManager.OnStoryCompleted -= IncrementNumberOfUnlockedStories;
    }

    void IncrementNumberOfUnlockedStories(int id)//In this case we don't care about the id, but we need to get it to handle this event
    {
        _numberOfUnlockedStories++;
        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText()
    {
        yield return new WaitForSeconds(2f);//meh, a ojo
        _fractionText.text = _numberOfUnlockedStories + "/" + _totalStories;
    }
}
