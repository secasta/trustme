using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {

    public Canvas[] _storyCanvases;
    public Canvas _mainMenuCanvas;
    public Canvas _settingsCanvas;
    public Canvas _storySelectCanvas;

    private Canvas _currentCanvas;

    void Awake()
    {
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
    }

    public void StartStory(int storyId)
    {
        DisableCanvas();
        _currentCanvas = Instantiate(_storyCanvases[storyId]);
        EnableCanvas();
    }

    public void GoToSettings()
    {
        DisableCanvas();
        _currentCanvas = _settingsCanvas;
        EnableCanvas();
    }

    public void GoToAlbum()
    {
        DisableCanvas();
        _currentCanvas = _storySelectCanvas;
        EnableCanvas();
    }

    public void GoToMain()
    {
        DisableCanvas();
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
    }

    public void FinishStory()//nombre provisional
    {
        Destroy(_currentCanvas);
        _currentCanvas = _mainMenuCanvas;
        EnableCanvas();
    }

    private void DisableCanvas()
    {
        _currentCanvas.enabled = false;
    }

    private void EnableCanvas()
    {
        _currentCanvas.enabled = true;
    }

}
