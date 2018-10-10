# Dimension Dash 
 [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)
 [![Codacy Badge](https://api.codacy.com/project/badge/Grade/9434a23e5da743ddb1f10657beb3e568)](https://www.codacy.com/app/niiaddo.andy/Dimension-Dash?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=andersonaddo/Dimension-Dash&amp;utm_campaign=Badge_Grade)
 [![CodeFactor](https://www.codefactor.io/repository/github/andersonaddo/dimension-dash/badge)](https://www.codefactor.io/repository/github/andersonaddo/dimension-dash)

![The game's banner](https://i0.wp.com/www.loadingdeveloper.com/wp-content/uploads/2017/10/Cover-Image.png?w=1024&ssl=1)
Thanks for caring about Dimension Dash and wanting to improve it! We'd love to so see what you're bringing to the game! 
Want to add a new character? Or a combat system? Or perhaps a system to allow players to make custom characters? Make it and contribute! 

#### You can also check our [projects page](https://github.com/andersonaddo/Dimension-Dash/projects) to see some features that you might want to add!

😔**Important Notice**😔

_In September 2018, SEGA filed a DMCA complaint to Google Play about Dimension Dash, so it was removed from Google Play. I've attempted to get SEGA to reconsider, but no luck. Please, feel free to contribute to this repo though. It could be a great way to learn programming and how to use Unity.
Sorry to those who had plans to see this game published with your changes, luck wasn't on our side.
To those interested in playing the game, you should clone this repo and build the .apk file on your computer using Unity._



### Some things to note before use **(read everything here!)**
----------

- Before you open this project into Unity, you have to make sure that you've linked Unity to your copy of the **Android SDK** and the **JDK** on your computer. If you don't already have those, go and download them fore you continue with anything else!
You can link Unity to the SDK and JDK by going to *Edit > Preferences > External tools*.

- This project was uploaded using Unity Unity 2018.1.3f1. If you're also using that version of Unity, no problem! However, if you are using a different version, here are some steps you might to follow:
	- Get the latest **version of Unity**
	- **Open** that version of Unity (not Dimension Dash though. Make an empty project or use a placeholder project)
	- **Download and install that version's Android compatibility module** by going to the Build settings and attempting to Switch the platform to Android. Once installed, switch the platform to Android.
	- Since the project uses [TextMeshPro](https://www.assetstore.unity3d.com/en/#!/content/84126) , you might have to update it to match your Unity version. **Delete the current TextMeshPro** folder in the Assets folder, and go to the Assets Store (or the Unity Package manager, if you're using 2018+) to download and install the newest version. **Restart Unity**.
	
### When making changes

----------
Here are some important things to know when you're working on Dimension Dash.

 - **Be careful when you're renaming objects that you did not make yourself** in a scene. Some scripts reference to objects by their names, so renaming them will obviously cause problems. To be sure, search for that name across the C# project (in something like Visual Studio) and see if there are any references to it.
 
 - **Don't rearrange any of the scenes** in the scene index manager! If you're making a new scene, add it to the bottom.
 - If you're going through a piece if code and you want to know more about how a variable or method is used, remember to use the "Find all references" tool that most modern IDEs have.
 
 - **Read the [Contribution Guildelines](CONTRIBUTING.md) if you want to contribute!.** If you make a contribution that shows that you haven't read the Code, you'll be sent back to read it and make changes.

