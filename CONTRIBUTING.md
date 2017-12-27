# Guidelines **(read everything here)**
While we welcome pull requests for dimension Dash, we have to lay some ground rules to protect the integrity of the game.

 1. **[Learn the basics](https://github.com/features)** of GitHub. Also read this [article](http://www.studica.com/blog/how-to-setup-github-with-unity-step-by-step-instructions). Know about what git is, and about branches and forks, and commits and pull requests.
 2. **Try, don't just suggest.** If you have a new system or idea that you'd like to see in the game, you can request for it over GitHub, sure, but that's *no guarantee* that it will be implemented. If you *really* want to see it done, try and to it yourself and contribute the addition. **Download** the project using the Github Desktop Application, and work on it locally on your computer. When you're done, **request for a pull request** to have it fully integrated into the game. If you're working on a large feature with others, you can request for a **branch** to be made for your work.
 3. **Keep the Asset folder organized.** When you met the project, everything was orderly and neat. Keep it that way. **Note: don't ever move these folders:
	 - Plugins 
	 - TextMesh Pro
	 - Google Play Assets
	 - GooglePlayGames 
	 - PlayServicesResolver 
	 
	Moving them will cause errors. Don't tamper with these folders either, unless you know what you're doing.

 4. **[Comment and use documentation](https://www.loadingdeveloper.com/writing-cleaner-code-part-one/)** If you look at the code in the project, you'll realize that it's been well documented and commented. Anything that wan't immediately obvious was explained. Follow suit. Here is a good example:
```
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
```
	

 5. **Don't change any of the classes in the internalMemoryManager script!** Doing so will cause all our players to have their play data erased due to the fact that the updated game would have a different format for saved data. If you want to add more things for persistent data, make another file and another class in the internalMemoryManager for it.
 
 7. **Reference where reference is due.** If you haven't already noticed,  Dimension Dash has a pretty long credits list. Those are attributions to the creators of the assets used. Please, please and please again — attribute to the creators of the assets you use. And make sure you're even allowed to use them!
 
 6. **Be awesome.** Just...don't compromise quality for laziness. Nobody likes that. Good luck!

