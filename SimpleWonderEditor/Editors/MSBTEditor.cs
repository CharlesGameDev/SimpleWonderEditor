using ImGuiNET;
using System.Numerics;

namespace SimpleWonderEditor.Editors
{
    static class MSBTEditor
    {
        static bool _initialized;
        static bool _isOpen;
        static string _currentInput = "";
        static bool _msbtLoaded;
        static int _currentNameListSelected;

        public static void Initialize()
        {
            if (_initialized) return;

            _initialized = true;
        }

        public static void Open()
        {
            if (_isOpen)
            {
                Close();
                return;
            }

            Initialize();
            _isOpen = true;
        }

        public static void Draw()
        {
            if (!_isOpen) return;

            ImGui.Begin("MSBT Editor");

            if (ImGui.Button("Open MSBT..."))
            {

            }

            ImGui.Separator();
            if (_msbtLoaded)
            {
                ImGui.Text("Items");
                ImGui.ListBox("", ref _currentNameListSelected, [], 0);
                ImGui.Separator();
            }

            Vector2 inputSize = new Vector2(ImGui.GetWindowWidth() - 100, 60);
            ImGui.Text("Text Input");
            ImGui.InputTextMultiline("", ref _currentInput, 32767, inputSize);

            ImGui.Separator();
            ImGui.Text("Preview");

            ImGui.BeginListBox("", inputSize);
            ImGui.Text(_currentInput);
            ImGui.EndListBox();

            ImGui.End();
        }

        public static void Close()
        {
            if (!_isOpen || !_initialized) return;
            _isOpen = false;


        }
    }
}
