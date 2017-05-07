using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game{

    public static Game current;
    public List<int> _unlockedStories;
    public List<string> _achievementsWaitingList;
    //variable para el tutorial (bool?)

    public delegate void ReUnlocking(int id);
    public static event ReUnlocking OnReUnlocking;

    public Game()
    {
        _unlockedStories = new List<int>();
        _achievementsWaitingList = new List<string>();
    }

    public void ReUnlock()
    {
        foreach (int id in _unlockedStories)
        {
            if (OnReUnlocking != null)
            {
                OnReUnlocking(id);
            }
        }
    }

    public void AddToUnlocked(int id)
    {
        if (!_unlockedStories.Contains(id))
        {
            if (_unlockedStories.Count < 1)//Si és el primer desbloqueig
            {
                _unlockedStories.Add(id);
                //Debug.Log("Adding Id for the first time.");
                //PrintListDebug();
            }
            else if (id < _unlockedStories[_unlockedStories.Count - 1])//Si es més petit que qualsevol id de la llista
            {
                _unlockedStories.Add(id);
                //Debug.Log("Adding Id at the end of the list.");
                //PrintListDebug();
            }
            else //Si s'ha d'insertar entremig de la llista
            {
                foreach (int alreadyUnlockedId in _unlockedStories)
                {
                    if (id > alreadyUnlockedId)
                    {
                        //Insert in this position
                        _unlockedStories.Insert(_unlockedStories.IndexOf(alreadyUnlockedId), id);
                        //Debug.Log("Id inserted in another's position.");
                        //PrintListDebug();
                        break;
                    }
                }
            }

            SaveLoad.Save();

        }
        else
        {
            Debug.Log("This story has been already unlocked.");
        }
    }

    public void AddToWaitingList(string achievementId)
    {
        _achievementsWaitingList.Add(achievementId);
    }

    public void ReportWaitingAchievements()
    {
        foreach (string achievementId in _achievementsWaitingList)
        {
            Social.ReportProgress(achievementId, 100.0, complete =>
            {
                if (complete)
                {
                    Debug.Log("Progress reported for id: " + achievementId);
                    _achievementsWaitingList.Remove(achievementId);
                }
                else
                {
                    Debug.Log("Could not report progress");
                }
            });
        }
    }

    //private void PrintListDebug()
    //{
    //    Debug.Log("Layout:");
    //    foreach (int alreadyUnlockedId in _unlockedStories)
    //    {
    //        Debug.Log("Index " + _unlockedStories.IndexOf(alreadyUnlockedId) + ": " + alreadyUnlockedId);
    //    }
    //}
}
