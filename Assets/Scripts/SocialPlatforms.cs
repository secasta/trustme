using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class SocialPlatforms : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject);

        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);

        Social.localUser.Authenticate(success => {
            if (success)
            {
                ReportAchievement();
            }
            else
            {
                Debug.Log("Failed to authenticate");
            }
        });
    }

    void ReportAchievement()
    {
        Debug.Log("Authenticated, checking achievements");
        Social.LoadAchievements(ProcessLoadedAchievements);
    }

    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
        {
            Debug.Log("Error: no achievements found");
        }
        else
        {
            Debug.Log("Got " + achievements.Length + " achievements");
        }

    }
}
