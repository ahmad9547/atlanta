using UnityEditor;

public class BuildScript
{
    public static void BuildWebGL()
    {
        string buildPath = "Build";
        BuildPipeline.BuildPlayer(
            new BuildPlayerOptions
            {
                scenes = new[] { "Assets/Scenes/StartSceneEmpty.unity", "Assets/Scenes/TestRoom_Zuraiz.unity", "Assets/Scenes/Tunnel_Zuraiz.unity" },
                locationPathName = buildPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            }
        );
        Debug.Log($"Build complete. Files saved to {buildPath}");
    }
}
