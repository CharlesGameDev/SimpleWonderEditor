using ImGuiNET;
using System.Reflection;

namespace SimpleWonderEditor
{
    static class ActorsWindow
    {
        class ActorEntry
        {
            public required string Name;
            public string ActorName = "";
            public required List<ActorEntry> Children;
            public bool Show;
        }

        private static bool _initialized;
        private static string _allActorSearch = "";
        private static int _allActorMatches = 0;
        private static List<ActorEntry> actorEntries = [];
        private static int _totalActorEntries;
        private static bool _includeActorNameInSearch = true;

        public static void Initialize()
        {
            if (_initialized) return;

            Print("init actor list");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "actors.txt";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            List<string> result = reader.ReadToEnd().Split("\n").ToList();

            ActorEntry? current = null;

            int i = 0;
            while (i < result.Count)
            {
                string str = result[i].Trim().Replace("\n", "").Replace("\r", "");
                if (string.IsNullOrWhiteSpace(str) && current != null)
                {
                    actorEntries.Add(current);
                    current = null;
                    i++;
                    continue;
                }
                if (!str.Contains('-'))
                {
                    current = new ActorEntry()
                    {
                        Name = str,
                        Children = []
                    };
                } else if (current != null)
                {
                    string[] split = str.Split('-');
                    current.Children.Add(new ActorEntry()
                    {
                        Name = split[1].Trim(),
                        ActorName = split[0].Trim().Replace(" ", ""),
                        Children = []
                    });
                    _totalActorEntries++;
                }

                i++;
            }

            _initialized = true;
        }

        public static void DrawGUI()
        {
            ImGui.Begin("Actors");

            ImGui.BeginTabBar("Selection", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton);

            if (ImGui.BeginTabItem("Property Manager"))
            {
                ImGui.Text("No actor selected.");
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("All Actors"))
            {
                if (!_initialized)
                {
                    ImGui.Text("Initializing actor list...");
                    if (!_initialized) Initialize();
                } else
                {
                    ImGui.AlignTextToFramePadding();
                    ImGui.Text("Search");
                    ImGui.SameLine();
                    ImGui.Checkbox("Include Actor Name", ref _includeActorNameInSearch);
                    ImGui.SameLine();
                    ImGui.InputText($"{_totalActorEntries} actors", ref _allActorSearch, 100);
                    ImGui.Separator();

                    _allActorSearch = _allActorSearch.Trim();

                    foreach (var item in actorEntries)
                        AddActorEntry(item);

                    ImGui.EndTabItem();

                    _allActorMatches = 0;
                }
            }

            ImGui.EndTabBar();

            ImGui.End();
        }

        static void AddActorEntry(ActorEntry item)
        {
            bool show;
            if (item.Children.Count == 0)
            {
                if (item.Show && (item.Name.Contains(_allActorSearch, StringComparison.CurrentCultureIgnoreCase) || (_includeActorNameInSearch && item.ActorName.Contains(_allActorSearch, StringComparison.CurrentCultureIgnoreCase))))
                    ActorButton(item.ActorName, item.Name);
                return;
            } else
            {
                show = ImGui.CollapsingHeader(item.Name, ImGuiTreeNodeFlags.Leaf);
            }

            foreach (var child in item.Children)
            {
                child.Show = show;
                AddActorEntry(child);
            }
        }

        static void ActorButton(string actorName, string name)
        {
            ImGui.Button("+");
            ImGui.SameLine();
            ImGui.Text(name);
            ImGui.SameLine();
            ImGui.TextDisabled(actorName);
            _allActorMatches++;
        }

        static void Print(object msg)
        {
            Console.WriteLine($"[ActorsWindow]: {msg}");
        }
    }
}
