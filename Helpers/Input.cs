using Microsoft.Xna.Framework.Input;

namespace mizjam1.Helpers
{
    public class Input
    {
        static KeyboardState LastState;
        static KeyboardState CurrentState;

        public static void Update(KeyboardState nextState)
        {
            LastState = CurrentState;
            CurrentState = nextState;
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentState.IsKeyDown(key);
        }
        public static bool IsKeyJustPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key) && (LastState != null && !LastState.IsKeyDown(key));
        }
    }
}
