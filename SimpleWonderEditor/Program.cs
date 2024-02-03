using ImGuiNET;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace SimpleWonderEditor
{
    class Program
    {
        private static Sdl2Window _window;
        private static GraphicsDevice _gd;
        private static CommandList _cl;
        private static ImGuiController _controller;

        private static bool _showActorWindow = true;

        private static Assembly assembly = Assembly.GetExecutingAssembly();

        private static Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);

        static void Main(string[] args)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            Print("init window");
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(0, 0, 1280, 720, WindowState.Maximized, $"Simple Wonder Editor v{version}"),
                new GraphicsDeviceOptions(true, null, false, ResourceBindingModel.Improved, true, true),
                out _window,
                out _gd);
            _window.Resized += () =>
            {
                _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
                _controller.WindowResized(_window.Width, _window.Height);
            };
            _cl = _gd.ResourceFactory.CreateCommandList();
            Print("init imGui");
            _controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);

            Print("main loop");
            var stopwatch = Stopwatch.StartNew();
            float deltaTime = 0f;
            // Main application loop
            while (_window.Exists)
            {
                deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
                stopwatch.Restart();
                InputSnapshot snapshot = _window.PumpEvents();
                if (!_window.Exists) { break; }
                _controller.Update(deltaTime, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.

                SubmitUI();

                _cl.Begin();
                _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));

                _controller.Render(_gd, _cl);
                _cl.End();
                _gd.SubmitCommands(_cl);
                _gd.SwapBuffers(_gd.MainSwapchain);
            }

            Print("exiting");
            // Clean up Veldrid resources
            Exit();
        }

        static void Print(object msg)
        {
            Console.WriteLine($"[SWE]: {msg}");
        }

        static void Exit()
        {
            _gd.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _gd.Dispose();
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
                ImGui.Checkbox("Actors", ref _showActorWindow);

                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();

            if (_showActorWindow)
                ActorsWindow.DrawGUI();
        }

        private static void OpenFile(string path)
        {
            Print(path);
        }
    }
}
