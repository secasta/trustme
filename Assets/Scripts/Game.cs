using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game{

    public static Game current;
    public List<int> _unlockedStories;
    //public List<int> _achievementsWaitingList;

    public delegate void ReUnlocking(int id);
    public static event ReUnlocking OnReUnlocking;

    public Game()
    {
        _unlockedStories = new List<int>();
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
            _unlockedStories.Add(id);
            SaveLoad.Save();
        }
    }
}
