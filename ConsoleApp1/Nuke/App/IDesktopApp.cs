namespace Nuke.App;

public interface IDesktopApp : IDisposable
{
    public struct UserTerminationRequestArgs
    {
        public bool CanTerminate { get; set; }
    }

    public delegate void OnUserTerminationRequestHandler(IDesktopApp desktopApp, ref UserTerminationRequestArgs e);

    public event OnUserTerminationRequestHandler OnUserTerminationRequest;

    void Main();
}