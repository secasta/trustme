﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : MonoBehaviour {

	void Start () {
        SystemLanguage language = Application.systemLanguage;
        switch (language)
        {
            case SystemLanguage.Spanish:
                CurrentSettings.Language = language;
                break;
            default:
                CurrentSettings.Language = SystemLanguage.English;
                break;
        }
        Debug.Log(CurrentSettings.Language);
	}
	

}
