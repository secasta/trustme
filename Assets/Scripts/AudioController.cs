using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    private AudioSource _audioSource;
    private float _originalVolume;

	void Awake ()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource) { Debug.LogError("No audio source found", this); }
        _originalVolume = _audioSource.volume;
        _audioSource.volume = _originalVolume * CurrentSettings.GetMasterVolumeOnLaunch();
        _audioSource.Play();

	}

    public void ChangeVolume()
    {
        _audioSource.volume = _originalVolume * CurrentSettings.MasterVolume;
    } 
}
