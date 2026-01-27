using System.IO;
using UnityEngine;

[System.Serializable]
public class RPGConfig
{
    public string characterName = "Aragorn";
    public int startingGold = 100;
    public float moveSpeed = 6.0f;
    public float sprintMultiplier = 1.8f;
    public float mouseSensitivity = 2.0f;

    public static RPGConfig Load()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "rpg_config.json");
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<RPGConfig>(File.ReadAllText(path));
        }

        return new RPGConfig();
    }
}
