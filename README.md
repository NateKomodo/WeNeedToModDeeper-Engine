# WeNeedToModDeeper-Engine
Mod engine core for We need to go deeper, it is installed by my other repo, WeNeedToModDeeper-Engine, which pulls the DLL to merge with the game code from the sources here.

[Link to installer](https://github.com/NateKomodo/WeNeedToModDeeper-installer)

**As we now use IPA, this engine is used as a framework or utility for IPA plugins**

# Making a plugin
A plugin/mod is a way to modify the way the game works

## Making the project
Before you start, use the installer to make sure your game code has the engine installed.
Simply open visual studio and use the class library with .NET framework, then in the soloution explorer, right click dependencies/refrences and press add refrence, it should open a menu, press browse and browse to your game install directory, go into WeNeedToGoDeeper data, then managed and select Assembly-CSharp, ModEngine and IllusionPlugin. You should then have access to the mod engine and the game code.
You may be asked to add other DLLs in the folder, to do this just repeat the above method except select them.
Once this is done you will need to make your class impement IPlugin.

## Using IllusionPlugin
As we now use Illusion plugin, you will need to implement it in your class. This will come with description functions and various level update methods for your to use.

## Using the framework
The framework has several classes, you can use these to control you plugin.
Make sure your main class implements the IPlugin interface, as it will not be loaded if it does not. This interface comes with methods for you already made, such as getting information about the plugin and a run function, where you put your code in.
If you want to use other clases, make sure you instanciate them before or instead of calling methods in there

### Events
To use an event, instanciate the ModEngineEvents class, add an if statement in your update void, in the brackets put (What you named the instance).(Event), this will be true if the event has happened

Example: 
ModEngineEvents events = new ModEngineEvents();
if(events.GoldChange()) { //Do stuffs }

### Variables
To use game variables, you can use the ModEngineVariables class, by typing ModEngineVariables.(variable) you can fetch the data from the game code without much knowledge of the game code. Some functions may return custom data types, which you can use as abridge to the class.

Example 1: ModEngineVariables.Gold = 1000; (To set, note you can also use -=, += etc) int gold = ModEngineVariables.Gold; (To get)

Example 2: ModEngineVariables.Substats.boostJuice = 100; (Substats has muliple variables in it)

### Components
Components are the most powerful part, they allows you to get game objects and classes attached to them, or attach your own. the ModEngineComponent class supports getting components attached to objects, and objects themselves, as well as a list of all objects, and all components for the objects, this allows you a high degree of control over the game code.

Example 1: ModEngineComponents.GetComponentFromObject<HealthController>("Player"); returns the health controller class
  
Example 2: ModEngineComponents.GetAllComponents("Player"); returns all components attached to the player

Example 3: ModEngineComponents.GetObjectFromTag("Player"); returns a game object of the player

Example 4: ModEngineComponents.GetAllGameObjects(); returns all the game objects

Example 5: ModEngineComponents.AddComponentToGameObject<MyLogicClass>("Player"); Adds a class to the player object
  
### Chat commands or message listening
To create a command, create a string in your update void and set it to (instanciated name).MessageSent(), (using your instanciated events handler)you can then do an if statement on it to see if it matches a command. You can also just see all chat messages this way

Example:
ModEngineEvents events = new ModEngineEvents();
string command = events.MessageSent();
if (command == "/help") { //dostuff }

### Displaying text
To display feedback to the player, you can either create an overlay over their screen, or using a chat message. To use this you need to create a new instance of ModEngineChatMessage or ModEngineTextOverlay. You dont have to save the instance.

Example: new ModEngineChatMessage("Hello world!", PlayerNetworking.ChatMessageType.BOT);

Example: new ModEngineTextOverlay("Hello world!");

### Functions
You can use ModEngineCommands.(command) to do some functions, such as spawning the time traveler or boss.

Example: ModEngineCommands.SpawnTimeTravller(0f, 0f, true); forces the time traveler to spawn instantly and be good
  
## Using and testing plugins
Simply build the plugin if its yours, or get the dll if its not, and put it in the Plugins folder in your we need to go deeper install folder

There is now a mod manager built in to the installer you can use to get plugins approved by the devs and myself.

If you wish to have your plugin added to the mod manager, open an issue on the reop [here](https://github.com/NateKomodo/WeNeedToModDeeper-Plugins)
