using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

    //public static Game _savedGame;

    public static void Save()
    {
        //savedGame = Game.current; //static instance in a non-static class
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, "savefile.trust"));
        //bf.Serialize(file, SaveLoad._savedGame);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "savefile.trust")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "savefile.trust"), FileMode.Open);
            //SaveLoad._savedGame = (Game)bf.Deserialize(file);
            file.Close();
        }
    }

    //Poner esto en el awake del monobehavior que llame a SaveLoad para que funciones bien en iOS:
    //Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");//Si no lo reconoce añadir "System." al principio 
}
