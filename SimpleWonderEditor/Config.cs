using Newtonsoft.Json;

namespace SimpleWonderEditor
{
    [Serializable]
    public class Config
    {
        public bool showActorWindow;

        public void SaveToFile()
        {
            string path = Directory.GetCurrentDirectory() + "/config.json";
            Print($"Saving to {path}");

            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config LoadFromFile()
        {
            string path = Directory.GetCurrentDirectory() + "/config.json";
            if (!File.Exists(path))
                return new Config();

            Print($"Loading from {path}");
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }

        static void Print(object msg)
        {
            Console.WriteLine($"[Config]: {msg}");
        }
    }
}
