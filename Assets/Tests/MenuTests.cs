using System.Linq;
using NUnit.Framework;
using UnityEditor;

public class MenuTests
{
    [Test]
    public void PauseMenu_StartsClosed()
    {
        Assert.IsFalse(new PauseMenuState().IsOpen);
    }

    [Test]
    public void PauseMenu_ToggleOpensThenCloses()
    {
        var state = new PauseMenuState();
        state.Toggle();
        Assert.IsTrue(state.IsOpen);
        state.Toggle();
        Assert.IsFalse(state.IsOpen);
    }

    [Test]
    public void PauseMenu_CloseAlwaysCloses()
    {
        var state = new PauseMenuState();
        state.Toggle();
        state.Close();
        Assert.IsFalse(state.IsOpen);
        state.Close();
        Assert.IsFalse(state.IsOpen);
    }

    [Test]
    public void BuildSettings_MainMenuIsFirstScene()
    {
        var first = EditorBuildSettings.scenes.First(s => s.enabled);
        StringAssert.EndsWith("/" + SceneNames.MainMenu + ".unity", first.path);
    }

    [Test]
    public void BuildSettings_IncludesCafe()
    {
        Assert.IsTrue(EditorBuildSettings.scenes.Any(
            s => s.enabled && s.path.EndsWith("/" + SceneNames.Cafe + ".unity")));
    }
}