using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class DeleteButton : MonoBehaviour {

    public void DeleteSavedGame()
    {
        Debug.Log("Deleting data");
        Game.current = new Game();
        SaveLoad.Save();

        GameCenterPlatform.ResetAllAchievements(HandleResetAchievementsDebug);
    }

    void HandleResetAchievementsDebug(bool success)
    {
        Debug.Log("Achievements have been reset: " + success);
    }
}
