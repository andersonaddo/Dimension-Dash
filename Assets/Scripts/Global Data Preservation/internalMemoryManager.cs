using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class internalMemoryManager
{

    const string fileName = "/savedData.dat";

    const string shopFileName = "/shopData.dat";


    //Score and Coins ------------------------------------------------------------------------------------------------------------------------------
    public void saveHiScore(int hiScore)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);

        //Formatting the hiScore into the serializable PlayerData class
        PlayerData data = new PlayerData(hiScore, globalDataPreserver.Instance.coinCount);

        //Storing the data
        binaryFormatter.Serialize(file, data);
        file.Close();

        //Giving the high score to the global data manager too
        globalDataPreserver.Instance.currentHighScore = hiScore;
    }



    public int loadHiScore()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

            //Retrieving the saved data in the fomr of a PlayerData class
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();

            return data.hiScore;
        }
        else
        {
            return 0;
        }
    }

    public int loadCoinCount()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

            //Retrieving the saved data in the fomr of a PlayerData class
            PlayerData data = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();

            return data.coinCount;
        }
        else
        {
            return 0;
        }
    }

    public void saveCoinCount(int currentHighScore, int coins)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);

        //Formatting the hiScore into the serializable PlayerData class
        PlayerData data = new PlayerData(currentHighScore, coins);

        //Storing the data
        binaryFormatter.Serialize(file, data);
        file.Close();
    }


    //Shop and Characters ------------------------------------------------------------------------------------------------------------------------------

    //This method was needed since files can be corrupted in devices sometimes (it's rare).
    //In such cases, the game gets messed up and even the default characters are locked.
    public bool isShopDataValid()
    {
        string shopFilePath = Application.persistentDataPath + shopFileName;

        if (!File.Exists(shopFilePath)) return false;
        if (new FileInfo(shopFilePath).Length == 0) return false;

        //To see if the data in the file is valid
        if (isShopCorrupted())
        {
            File.Delete(shopFilePath);
            return false;
        }
        return true;
    }


    bool isShopCorrupted()
    {
        FileStream file = File.Open(Application.persistentDataPath + shopFileName, FileMode.Open);

        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            ShopData storedData = binaryFormatter.Deserialize(file) as ShopData;
            file.Close();
            if (storedData == null) return true;
        }
        catch (System.Exception)
        {
            file.Close();
            return true;
        }


        return false;
    }


    public void saveShopDetails(Dictionary<int, CharacterOptionsForDimensions> choices)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + shopFileName);

        //Zipping all the data into the Shop Data class
        ShopData newData = new ShopData(choices);

        //Storing the data
        binaryFormatter.Serialize(file, newData);
        file.Close();
    }

    public ShopData loadShopDetails()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + shopFileName, FileMode.Open);

        //Retrieving the saved data in the form of a ShopData class
        ShopData storedData = (ShopData)binaryFormatter.Deserialize(file);
        file.Close();

        return storedData;
    }


}


//For holding score and coin data
[System.Serializable]
class PlayerData
{
    public int hiScore;
    public int coinCount;

    public PlayerData(int score, int coinCount)
    {
        hiScore = score;
        this.coinCount = coinCount;
    }
}


//These classes are for holding the character selection data
[System.Serializable]
public class ShopData
{
    //This dictionary has dimension scene index keys and CharacterOptionsForDimensions values 
    public Dictionary<int, CharacterOptionsForDimensions> characterChoices = new Dictionary<int, CharacterOptionsForDimensions>();

    public ShopData(Dictionary<int, CharacterOptionsForDimensions> choices)
    {
        characterChoices = choices;
    }

}

[System.Serializable]
public class CharacterOptionsForDimensions
{
    //Built in with default values
    public int selectedCharacter = 0;
    public List<int> availableCharacterIndexes = new List<int>(new int[] { 0 });
}
