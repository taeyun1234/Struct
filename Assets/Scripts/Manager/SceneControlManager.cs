using UnityEngine.SceneManagement;

public class SceneControlManager
{
    public BaseScene CurrentScene { get; private set; }

    public void LoadScene(Define.Scene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    private string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene = null;
    }
}