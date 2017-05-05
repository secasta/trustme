using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalText{

    public static string PLAY_English = "PLAY";
    public static string START_English = "START";
    public static string LIAR_English = "LIAR";
    public static string TRUSTED_English = "TRUSTED";
    public static string NEXT_English = "NEXT";
    public static string PLAY_Spanish = "PLAY";
    public static string START_Spanish = "START";
    public static string LIAR_Spanish = "LIAR";
    public static string TRUSTED_Spanish = "TRUSTED";
    public static string NEXT_Spanish = "NEXT";

    public static Dictionary<string, string> DICTIONARY = new Dictionary<string, string>();

    public static void PopulateDictionary()
    {
        DICTIONARY.Clear();
        DICTIONARY.Add("PLAY_English", PLAY_English);
        DICTIONARY.Add("START_English", START_English);
        DICTIONARY.Add("LIAR_English", LIAR_English);
        DICTIONARY.Add("TRUSTED_English", TRUSTED_English);
        DICTIONARY.Add("NEXT_English", NEXT_English);
        DICTIONARY.Add("PLAY_Spanish", PLAY_Spanish);
        DICTIONARY.Add("START_Spanish", START_Spanish);
        DICTIONARY.Add("LIAR_Spanish", LIAR_Spanish);
        DICTIONARY.Add("TRUSTED_Spanish", TRUSTED_Spanish);
        DICTIONARY.Add("NEXT_Spanish", NEXT_Spanish);
    }

}
