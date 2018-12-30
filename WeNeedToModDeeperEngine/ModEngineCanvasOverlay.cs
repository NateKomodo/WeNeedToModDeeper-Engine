namespace WeNeedToModDeeperEngine //NOTE the types below are a framework that mod makers can choose to include
{
    public class ModEngineCanvasOverlay
    {
        public ModEngineCanvasOverlay(string message, int framesToLast)
        {
            if (!ModEngine.HasChecked) ModEngine.CheckForUpdates();
            TechLogCanvasBehavior.SetDisplayString(message, framesToLast);
        }
    }
}
