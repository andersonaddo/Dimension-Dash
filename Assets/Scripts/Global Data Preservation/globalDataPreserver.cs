using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This is a very important singleton class that holds data and logic for classes and objects across the game
/// It also initializes the game every startup.
/// </summary>
public class globalDataPreserver : MonoBehaviour {

    public static globalDataPreserver Instance;

    public int currentHighScore;
    int leaderboardHiScore = 0;

    public float playerScore;
    public int previousScore = 0; //Used when a player goes to a boss

    //Coin variables
    public int coinCount;
    internalMemoryManager internalMemory = new internalMemoryManager();

    //Boss variables
    public int encounters; //The number of times the player has encountered the boss level
    ArrayList scoreLimits = new ArrayList(); //These are the player scores that would case the boss to move up the difficulty curve
    int currentIndex = 0;
    public bool canBoss, canBossPermanent; //Added to enable boss fights

    //Shop Variables
    [HideInInspector]
    public Dictionary<int, CharacterOptionsForDimensions> characterChoices = new Dictionary<int, CharacterOptionsForDimensions>();

    
    public const string KEY_FOR_LANGUAGE = "language"; //Localization Variable
    public const string KEY_FOR_OPACITY = "buttonOpacity";

    // Use this for initialization
    void Awake () {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            scoreLimits.Add(2500);
            scoreLimits.Add(3500);
            scoreLimits.Add(4500);
            scoreLimits.Add(5500);

            // Loading the coin count to allow easy incrementation and saving
            coinCount = internalMemory.loadCoinCount();

            //Doing the same for high score
            currentHighScore = internalMemory.loadHiScore();

            //Getting the current shop data from internal memory
            if (System.IO.File.Exists(Application.persistentDataPath + internalMemory.shopFileName))
            {
                cacheShop();
            }else //Setting up the default shop data if this is the first play...
            {
                setUpShop();
            }


        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
	}


    //Tracks the player's score to determine when they are viable for a boss battle
    //Once the player's scores reach a certain limit, they become viable to enter the boss' dimension
    void Update()
    {
        if (playerScore > (int)scoreLimits[currentIndex] && currentIndex != scoreLimits.Count - 1)
        {
            currentIndex++;
            canBoss = true;
            encounters++;
        }else if (currentIndex == scoreLimits.Count - 1 && !canBossPermanent)
        {
            canBossPermanent = true;
        }
    }

    //Called whenever a player gets a coin
    public void incrementCoinCount(int number)
    {
        coinCount+= number;
        internalMemory.saveCoinCount(currentHighScore, coinCount);
    }


    public void reset()
    {
        encounters = 0;
        playerScore = 0;
        previousScore = 0;
        currentIndex = 0;
        canBoss = false;
        canBossPermanent = false;
    }

    //Used to initialize the shop the first time the game is opened
    void setUpShop()
    {
        //With the default dimensions with default character choices
        characterChoices[1] = new CharacterOptionsForDimensions();
        characterChoices[2] = new CharacterOptionsForDimensions();
        characterChoices[3] = new CharacterOptionsForDimensions();

        saveShopData();
    }

    void saveShopData()
    {
        internalMemory.saveShopDetails(characterChoices);
    }

    //If a new dimension is bought
    public void addNewDimension(int sceneIndex)
    {
        characterChoices[sceneIndex] = new CharacterOptionsForDimensions();
        saveShopData();
    }

    //Changing the user's prefered character for a dimension
    public void updateSelectedCharacter (int dimensionSceneIndex, int choice)
    {
        characterChoices[dimensionSceneIndex].selectedCharacter = choice;
        saveShopData();
    }

    //if a new character is bought
    public void addCharacter(int dimensionSceneIndex, int characterIndex)
    {
        characterChoices[dimensionSceneIndex].availableCharacterIndexes.Add(characterIndex);
        updateSelectedCharacter(dimensionSceneIndex, characterIndex);
        saveShopData();
    }

    //Saving the shop's data into this class for easy access
    public void cacheShop()
    {
        ShopData savedShopData = internalMemory.loadShopDetails();
        characterChoices = savedShopData.characterChoices;
    }

    //This method is called by portals when they are deciding which dimension to send the player
    public List<int> getAvailableDimensions()
    {
        Dictionary<int, CharacterOptionsForDimensions>.KeyCollection keyCollection = characterChoices.Keys;
        List<int> availableDimensions = new List<int>();

        foreach (int i in keyCollection) availableDimensions.Add(i);
        return availableDimensions;
    }

    //Localization Methods
    public string getCurrentLanguage()
    {
        return (PlayerPrefs.HasKey(KEY_FOR_LANGUAGE)) ? PlayerPrefs.GetString(KEY_FOR_LANGUAGE) : "EN";
    }


    
    public float getButtonOpacity()
    {
        return (PlayerPrefs.HasKey(KEY_FOR_OPACITY)) ? PlayerPrefs.GetFloat(KEY_FOR_OPACITY) : 116f;
    }



    //Called externally when player dies
    public void calculateHighScore(int endgameScore)
    {

        if (!GooglePlayServiceManager.hasTriedToSignIn) GooglePlayServiceManager.signIn();
        if (!GooglePlayServiceManager.hasCachedHiScore) leaderboardHiScore = GooglePlayServiceManager.retrieveHighScore();

        /*
        This was done to make sure that the highest score among the local and leaderboard high scores is the score being considered
        Because sometimes the player may make a new high score while not connected to the internet
        This also allows high scores to remain synched when two people are playing with two different devices with the same account
        */
        currentHighScore = Mathf.Max(currentHighScore, leaderboardHiScore);
        if (endgameScore > currentHighScore) currentHighScore = endgameScore;
        internalMemory.saveHiScore(currentHighScore);
        if (leaderboardHiScore != currentHighScore)
        {
            GooglePlayServiceManager.postHighScore(currentHighScore);
            leaderboardHiScore = currentHighScore; //That may not seem necessary, but it ensures that it only posts when there is a new high score
        }
    }

}
