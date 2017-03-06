using UnityEngine;
using System.Collections;

public static class UnlocksManager {

    private static bool[] _isStageUnlocked;

    public static bool GetUnlockedBoolean(int stage)
    {
        if (_isStageUnlocked == null) { InitializeArray(); }
        return _isStageUnlocked[stage];
    }

    public static void UnlockStage(int stage)
    {
        if (_isStageUnlocked == null) { InitializeArray(); }
        _isStageUnlocked[stage] = true;
    }

    private static void InitializeArray()
    {
        _isStageUnlocked = new bool[100];
        for (int i = 0; i < 100; i++)
        {
            _isStageUnlocked[i] = false;
        }
        Debug.Log("Unlocks array initialized!");
    }
}
