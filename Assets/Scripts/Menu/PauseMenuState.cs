//C# so it can be unit tested
public class PauseMenuState
{
    public bool IsOpen { get; private set; }

    public void Toggle() => IsOpen = !IsOpen;
    public void Close()  => IsOpen = false;
}