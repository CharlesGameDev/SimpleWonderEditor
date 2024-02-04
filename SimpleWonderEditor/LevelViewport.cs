using ImGuiGeneral;
using ImGuiNET;
using System.Numerics;
using static SDL2.SDL;

namespace SimpleWonderEditor
{
    static class LevelViewport
    {
        static readonly uint ACTOR_COLOR = ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1));
        public static ImGuiGLRenderer Renderer;

        public static void Initialize(IntPtr window, IntPtr glContext)
        {
            Print("init OpenGL renderer");
            Renderer = new ImGuiGLRenderer(window, glContext);
        }

        public static void Resize()
        {

        }

        public static void BeginDraw()
        {
            Renderer.ClearColor(0.05f, 0.05f, 0.05f, 1.00f);
            Renderer.NewFrame();
        }

        public static void EndDraw(nint window)
        {
            Renderer.Render();
            SDL_GL_SwapWindow(window);
        }

        public static void Close()
        {

        }

        static void Print(object msg)
        {
            Console.WriteLine($"[LevelViewport]: {msg}");
        }
    }
}
