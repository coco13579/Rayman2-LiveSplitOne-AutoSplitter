using WebSocketSharp.Server;

namespace AutoSplitter;

class Program
{
    public bool Connected = false;
    public LSO LSO;
    const string _address = "ws://localhost";
    const ConsoleKey _resetKey = ConsoleKey.R;
    readonly Game _game;
    readonly WebSocketServer _server = new(_address);

    static void Main()
    {
        Console.Title = "Rayman 2 LiveSplit One AutoSplitter";
        Console.SetWindowSize(85, 6);
        Console.SetBufferSize(85, 6);

        new Program();
    }

    Program()
    {
        var ui = new UI(_address, _resetKey.ToString());

        _game = new(ui);

        _server.Start();
        _server.AddWebSocketService("/", (LSO lso) => {
            lso.Program = this;
            lso.UI = ui;
        });

        Loop();
    }

    void Loop()
    {
        const ConsoleKey exitKey = ConsoleKey.Enter;
        ConsoleKey inputKey = default;

        do
        {
            _game.Check();

            if (Connected && _game.Hooked)
            {
                CheckGameEvents();
            }

            if (!Console.KeyAvailable)
                continue;

            inputKey = Console.ReadKey(true).Key;

            if (Connected && inputKey == _resetKey)
            {
                LSO.Send("reset");
                _game.ResetBooleans();
            }
        } while (inputKey != exitKey);

        _server.Stop();
    }

    public void CheckGameEvents()
    {
        _game.FirstLevelMovement(LSO);
        _game.LoadingScreen(LSO);
        _game.LevelChanged(LSO);
        _game.FinalBossDead(LSO);
        _game.NewGameStarted(LSO);
    }
}