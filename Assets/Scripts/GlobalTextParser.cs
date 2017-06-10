using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTextParser : MonoBehaviour {

    public TextAsset _spanishTextFile;
    public TextAsset _englishTextFile;


    void Start ()
    {
        ParseSpanish();
        ParseEnglish();
        GlobalText.PopulateDictionary();
	}

    void ParseSpanish()
    {
        string[] rawTextLines = _spanishTextFile.text.Split('\n');
        foreach (string rawTextLine in rawTextLines)
        {
            if (rawTextLine.StartsWith("play"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.PLAY_Spanish = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("start"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.START_Spanish = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("liar"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.LIAR_Spanish = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("trusted"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.TRUSTED_Spanish = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("next"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.NEXT_Spanish = rawTextLine.Substring(index + 2);
            }
            else if(rawTextLine.StartsWith("understood"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.UNDERSTOOD_Spanish = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("all unlocked"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.ALL_UNLOCKED_Spanish = rawTextLine.Substring(index + 2);
            }
        }
    }

    void ParseEnglish()
    {
        string[] rawTextLines = _englishTextFile.text.Split('\n');
        foreach (string rawTextLine in rawTextLines)
        {
            if (rawTextLine.StartsWith("play"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.PLAY_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("start"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.START_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("liar"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.LIAR_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("trusted"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.TRUSTED_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("next"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.NEXT_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("understood"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.UNDERSTOOD_English = rawTextLine.Substring(index + 2);
            }
            else if (rawTextLine.StartsWith("all unlocked"))
            {
                int index = rawTextLine.IndexOf("=");
                GlobalText.ALL_UNLOCKED_English = rawTextLine.Substring(index + 2);
            }

        }
    }


}
