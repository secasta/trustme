using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class SocialPlatforms : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject);

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

		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
    }

    void ReportAchievement()
    {
        Debug.Log("Authenticated, checking achievements");
		Social.LoadAchievementDescriptions(ProcessLoadedAchievements);
    }

	void ProcessLoadedAchievements(IAchievementDescription[] achievements)
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
