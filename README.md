# WeNeedToModDeeper-Engine
Mod engine core for We need to go deeper, it is installed by my other repo, WeNeedToModDeeper-Engine, which pulls the DLL to merge with the game code from the sources here.

[Link to installer](https://github.com/NateKomodo/WeNeedToModDeeper-installer)

# Making a plugin
A plugin/mod is a way to modify the way the game works

## Making the project
Before you start, use the installer to make sure your game code has the engine installed.
Simply open visual studio and use the class library with .NET framework, then in the soloution explorer, right click dependencies/refrences and press add refrence, it should open a menu, press browse and browse to your game install directory, go into WeNeedToGoDeeper data, then managed and select Assembly-CSharp. You should then have access to the mod engine and the game code.
You may be asked to add other DLLs in the folder, to do this just repeat the above method except select them instead of Assembly-CSharp

## Using the engine
The engine has 4 main classes, ModEngineEventHandler, ModEngineVariables, ModEngineComponents and ModEngineChatMessages (WIP), you can use these to control you plugin.
Make sure your main class implements the IPlugin interface, as it will not be loaded if it does not. This interface comes with methods for you already made, such as getting information about the plugin and a run function, where you put your code in.
If you want to use other clases, make sure you instanciate them before or instead of calling methods in there

### Events
To use events, you need to subscribe to one, type ModEngineEventHandler.(Event you want to use) += (A function you want to be called when the event is triggered), when the event is triggered, your function you specified will be called as well.
Example: ModEngineEventHandler.OnBossKilled += bosskilled(); (Where bosskilled() is a void)

### Variables
To use game variables, you can use the ModEngineVariables class, by typing ModEngineVariables.(variable) you can fetch the data from the game code without much knowledge of the game code. Some functions may return custom data types, which you can use as abridge to the class.
Example 1: ModEngineVariables.Gold = 1000; (To set, note you can also use -=, += etc) int gold = ModEngineVariables.Gold; (To get)
Example 2: ModEngineVariables.Substats.boostJuice = 100; (Substats has muliple variables in it)

### Components
Components are the most powerful part, they allows you to get game objects and classes attached to them, or attach your own. the ModEngineComponent class supports getting components attached to objects, and objects themselves, as well as a list of all objects, and all components for the objects, this allows you a high degree of control over the game code.
Example 1: ModEngineComponents.GetComponent("Player", <HealthController>); returns the health controller class
Example 2: ModEngineComponents.GetAllComponents("Player"); returns all components attached to the player
Example 3: ModEngineComponents.GetObject("Player"); returns a game object of the player
Example 4: ModEngineComponents.GetAllGameObjects(); returns all the game objects
Example 5: ModEngineComponents.AddComponent("Player", <MyLogicClass>); Adds a class to the player object
