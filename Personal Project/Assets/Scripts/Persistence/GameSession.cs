
public static class GameSession
{
    public static bool ContinueRequested { get; private set; }

    public static void RequestContinue()
    {
        ContinueRequested = true;
    }

    public static void RequestNewGame()
    {
        ContinueRequested = false;
    }

    public static void ClearRequest()
    {
        ContinueRequested = false;
    }
}
