using WebSocketSharp;
using WebSocketSharp.Server;

namespace AutoSplitter;

class LSO : WebSocketBehavior
{
    public Program Program;
    public UI UI;
    protected override void OnOpen()
    {
        if (Program.Connected)
        {
            Close();

            return;
        }

        Program.LSO = this;
        Program.Connected = UI.ServerConnected = true;

        while (ConnectionState == WebSocketState.Open) ;

        Program.Connected = UI.ServerConnected = false;
    }

    public new void Send(string message)
    {
        base.Send(message);
    }
}