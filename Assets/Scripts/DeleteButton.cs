using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour {

    public void DeleteSavedGame()
    {
        Debug.Log("Deleting data");
        Game.current = new Game();
        SaveLoad.Save();
    }
}
