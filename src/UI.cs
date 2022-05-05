namespace AutoSplitter;

class UI
{
    bool _gameHooked;
    bool _serverConnected;
    readonly string _serverAddress;
    readonly string _resetKey;
    public bool GameHooked
    {
        set
        {
            _gameHooked = value;

            Update();
        }
    }
    public bool ServerConnected
    {
        set
        {
            _serverConnected = value;

            Update();

            if (value)
                return;

            var t = new Thread(() => Clipboard.SetText(_serverAddress));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
    }

    public UI(string serverAddress, string resetKey)
    {
        _serverAddress = serverAddress;
        _resetKey = resetKey;

        ServerConnected = false;
    }
    void Update()
    {
        Console.Clear();

        if (_serverConnected)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LiveSplit One connected.");
            Console.ResetColor();
        }
        else
            Console.WriteLine($"Awaiting LiveSplit One connection to server '{_serverAddress}' (copied to clipboard).");

        if (_gameHooked)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Game detected.");
            Console.ResetColor();
        }
        else
            Console.WriteLine("Waiting for game to start.");

        if (_serverConnected)
            Console.WriteLine($"\nPress {_resetKey} to reset, or ENTER to exit.");
        else
            Console.WriteLine("\nPress ENTER to exit.");
    }
}