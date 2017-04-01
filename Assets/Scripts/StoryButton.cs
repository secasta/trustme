using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour {

    public Sprite _storyIcon;

    private int _storyId;
    private Button _button;
    private Image _image;
    private CanvasManager _canvasManager;


	void Awake () {
        _button = GetComponent<Button>();
        if (!_button) { Debug.LogError("No button component found", this); }
        _image = GetComponent<Image>();
        if (!_image) { Debug.LogError("No image component found", this); }
        _canvasManager = FindObjectOfType<CanvasManager>();
        if (!_canvasManager) { Debug.LogError("Canvas Manager not found", this); }

        _storyId = int.Parse(gameObject.name.Substring(0, 3));

        if (!_storyIcon) { Debug.LogWarning("Remember to assign a story icon to this button!", this); }
    }

    void OnEnable()
    {
        StoryController.OnStoryCompleted += CheckIfButtonShouldBeUnlocked;
    }

    void OnDisable()
    {
        StoryController.OnStoryCompleted -= CheckIfButtonShouldBeUnlocked;
    }

    void CheckIfButtonShouldBeUnlocked(int id)
    {
        if (id == _storyId)
        {
            _image.sprite = _storyIcon;
            _button.enabled = true;
        }
    }

    public void StartStory()
    {
        _canvasManager.StartBeatenStory(_storyId);
    }
}
