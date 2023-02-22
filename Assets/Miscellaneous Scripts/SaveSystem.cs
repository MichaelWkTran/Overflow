using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using static ShopItem;

public static class SaveSystem
{
    const string m_fileName = "/Save.bb";

    [Serializable] public class SaveData
    {
        //Game Variables
        public int m_highScore;
        public float m_carrots;

        public Dictionary<string, bool> m_purchasedItems = new Dictionary<string /*Name of shop item*/, bool /*Whether the item is purchased*/>();
        public string m_currentSkinName;
        public string m_currentHatName;
        public Dictionary<string, bool> m_enabledFurniture = new Dictionary<string /*Name of furniture*/, bool /*Whether the item is enabled*/>();

        //Settings
        public float m_sfxVolume;
        public float m_musicVolume;
        public int m_qualityLevel;
    }
    public static SaveData m_data = new SaveData();

    public static void Save()
    {
        foreach(ShopItem shopItem in MonoBehaviour.FindObjectsOfType<ShopItem>())
        {
            m_data.m_purchasedItems[shopItem.m_name] = shopItem.m_purchased;
            switch (shopItem.m_itemDataType)
            {
                case ItemDataType.Skin: 
                    
                    break;
                case ItemDataType.Hat: 
                    
                    break;
                case ItemDataType.Furniture: 
                    
                    break;
            }

        }
        SkinData.m_currentSkin;


        //Get the file directory of the save data
        string path = Application.persistentDataPath + m_fileName;
        
        //Create and open file stream
        FileStream stream = new FileStream(path, FileMode.Create);

        //Create binary formatter and serialize the game data
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, m_data);

        //Close the file stream
        stream.Close();
    }

    public static void Load()
    {


        //Get the file directory of the save data
        string path = Application.persistentDataPath + m_fileName;

        //Check whether the file in the searched path exists, if not then exit the function
        if (!File.Exists(path)) { Debug.LogError("Save file not found in " + path); return; }

        //Create and open file stream
        FileStream stream = new FileStream(path, FileMode.Open);

        //Create binary formatter and deserialize the game data
        BinaryFormatter formatter = new BinaryFormatter();
        m_data = formatter.Deserialize(stream) as SaveData;

        //Close the file stream
        stream.Close();
    }
}
