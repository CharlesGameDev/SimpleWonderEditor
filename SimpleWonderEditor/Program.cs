using ImGuiNET;
using System.Diagnostics;
using System.Reflection;
using static SDL2.SDL;
using static SDL2.SDL.SDL_EventType;
using ImGuiGeneral;
using SimpleWonderEditor.Editors;

namespace SimpleWonderEditor
{
    class Program
    {
        private static Config config;
        static IntPtr _window;
        static IntPtr _glContext;

        static void Main(string[] args)
        {
            config = Config.LoadFromFile();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Print("init SDL2 and OpenGL");
            (_window, _glContext) = ImGuiGL.CreateWindowAndGLContext($"Simple Wonder Editor v{version}", 1280, 720, maximized: true);

            LevelViewport.Initialize(_window, _glContext);

            MSBTEditor.Open();

            Print("main loop");
            bool quit = false;
            while (!quit)
            {
                while (SDL_PollEvent(out SDL_Event e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_QUIT:
                            quit = true;
                            break;
                    }
                    LevelViewport.Renderer.ProcessEvent(e);
                }

                LevelViewport.BeginDraw();
                SubmitUI();
                LevelViewport.EndDraw(_window);
            }

            Print("exiting");
            LevelViewport.Close();
            // Clean up Veldrid resources
            Exit();
        }

        static void Print(object msg)
        {
            Console.WriteLine($"[SWE]: {msg}");
        }

        static void Exit()
        {
            SDL_GL_DeleteContext(_glContext);
            SDL_DestroyWindow(_window);
            SDL_Quit();
            config.SaveToFile();
            Environment.Exit(0);
        }

        private static unsafe void SubmitUI()
        {
            ImGui.BeginMainMenuBar();

            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("New...", "Ctrl+N"))
                {

                }
                if (ImGui.MenuItem("Open...", "Ctrl+O"))
                {

                }

                ImGui.Separator();

                if (ImGui.MenuItem("Exit"))
                {
                    Exit();
                    return;
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                ImGui.MenuItem("Preferences", "Ctrl+P");

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Window"))
            {
                ImGui.Checkbox("Actors", ref config.showActorWindow);

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Tools"))
            {
                if (ImGui.MenuItem("MSBT Editor"))
                    MSBTEditor.Open();
                ImGui.MenuItem("Area Param Editor", false);
                ImGui.MenuItem("Music Editor", false);
                ImGui.MenuItem("World Map Editor", false);
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();

            if (config.showActorWindow)
                ActorsWindow.DrawGUI();
            MSBTEditor.Draw();
        }

        private static void OpenFile(string path)
        {
            Print(path);
        }
    }
}
