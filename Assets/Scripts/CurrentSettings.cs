using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentSettings{

    public static SystemLanguage Language = SystemLanguage.English;
    public static float MasterVolume = 1f;

    const string MASTER_VOLUME_KEY = "master_volume";
    const string LANGUAGE_KEY = "language";

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
        else
        {
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
        }
        return MasterVolume;
    }

    public static bool HasPreferredLanguage()
    {
        if (PlayerPrefs.HasKey(LANGUAGE_KEY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetLanguage(SystemLanguage newLanguage)
    {
        switch (newLanguage.ToString())
        {
            case "Spanish":
                PlayerPrefs.SetString(LANGUAGE_KEY, "Spanish");
                Language = newLanguage;
                break;
            case "English":
                PlayerPrefs.SetString(LANGUAGE_KEY, "English");
                Language = newLanguage;
                break;
            default:
                PlayerPrefs.SetString(LANGUAGE_KEY, "English");
                Language = SystemLanguage.English;
                break;
        }
    }

    public static void InitializeLanguage()
    {
        string languageToInitialize = PlayerPrefs.GetString(LANGUAGE_KEY);

        switch (languageToInitialize)
        {
            case "Spanish":
                Language = SystemLanguage.Spanish;
                break;
            case "English":
                Language = SystemLanguage.English;
                break;
            default:
                Language = SystemLanguage.English;
                break;
        }
    }
}
