namespace Nuke.App;

public interface IWindow : IDisposable
{
    string Title { get; set; }

    Size Size { get; set; }

    bool IsOpen { get; set; }
}