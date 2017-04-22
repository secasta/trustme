using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentSettings{

    public static SystemLanguage Language = SystemLanguage.English;
    public static float MasterVolume = 1f;

    const string MASTER_VOLUME_KEY = "master_volume";

    public static void SetMasterVolume(float volume)
    {
        Debug.Log("Setting master volume on player prefs: " + volume);
        MasterVolume = volume;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
    }

    public static float GetMasterVolumeOnLaunch()
    {
        if (!PlayerPrefs.HasKey(MASTER_VOLUME_KEY))//First time 
        {
            MasterVolume = 1f;
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
        }
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }
}
