using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : MonoBehaviour {

    public Sprite _storyIcon;
    public GameObject _blockingPanel;

    private int _storyId;
    private Button _button;
    private Image _image;
    private CanvasManager _canvasManager;
    private Animator _animator;
    private VerticalScrollRect _gridScrollRect;
    private RectTransform _thisRectTransform;


	void Awake () {
        _button = GetComponent<Button>();
        if (!_button) { Debug.LogError("No button component found", this); }
        _image = GetComponent<Image>();
        if (!_image) { Debug.LogError("No image component found", this); }
        _canvasManager = FindObjectOfType<CanvasManager>();
        if (!_canvasManager) { Debug.LogError("Canvas Manager not found", this); }
        _animator = GetComponent<Animator>();
        if (!_animator) { Debug.LogError("Animator not found", this); }
        _gridScrollRect = GetComponentInParent<VerticalScrollRect>();
        if (!_gridScrollRect) { Debug.LogError("Scroll rect not found in parent", this); }
        _thisRectTransform = GetComponentInParent<RectTransform>();
        if (!_thisRectTransform) { Debug.LogError("Rect transform not found", this); }

        _storyId = int.Parse(gameObject.name.Substring(0, 3));

        if (!_storyIcon) { Debug.LogWarning("Remember to assign a story icon to this button!", this); }
    }

    void OnEnable()
    {
        CanvasManager.OnStoryCompleted += CheckIfButtonShouldBeUnlocked;
        Game.OnReUnlocking += CheckIfButtonShouldBeActivated;
    }

    void OnDisable()
    {
        CanvasManager.OnStoryCompleted -= CheckIfButtonShouldBeUnlocked;
        Game.OnReUnlocking -= CheckIfButtonShouldBeActivated;
    }

    void CheckIfButtonShouldBeUnlocked(int id)
    {
        if (id == _storyId)
        {
            UnlockButton();

        }
    }

    void CheckIfButtonShouldBeActivated(int id)
    {
        if (id == _storyId)
        {
            Debug.Log("Button should be activated, proceeding. Id: " + id);
            ActivateButton();

        }
    }

    public void StartStory()
    {
        _canvasManager.StartBeatenStory(_storyId);
    }

    private void UnlockButton()
    {
        _blockingPanel.SetActive(true);
        _gridScrollRect.ScrollToShow(_thisRectTransform);
        _animator.SetTrigger("Unlock Trigger");
        _button.enabled = true;

        //report social achievement

		string achievementId = "story_";
        achievementId = string.Concat(achievementId, _storyId.ToString());

        Social.ReportProgress(achievementId, 100.0, complete =>
        {
            if (complete)
            {
                Debug.Log("Progress reported");
            }
            else
            {
                Debug.Log("Could not report progress");
            }
        });
    }

    private void ActivateButton()
    {
        _button.image.sprite = _storyIcon;
        _button.enabled = true;
    }

    public void ChangeSprite()
    {
        _image.sprite = _storyIcon;
    }

    public void AnimationFinished()
    {
        StartCoroutine(FinishUnlockingSequence(0.9f));
    }

    IEnumerator FinishUnlockingSequence(float time)
    {
        yield return new WaitForSeconds(time);
        _canvasManager.GoToMain();
        _blockingPanel.SetActive(false);
        //_gridScrollRect.ResetPosition();
    }
}
