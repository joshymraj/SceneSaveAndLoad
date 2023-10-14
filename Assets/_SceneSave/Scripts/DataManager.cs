using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour {
    const string kSceneSaveFileName = @"Assets/Streaming Assets/scene_json.txt";

    string sceneSaveFileName = string.Empty;

    void Start() {
        sceneSaveFileName = kSceneSaveFileName;
    }

    public void SaveScene(SceneData sceneData) {
        string sceneDataJson = JsonConvert.SerializeObject(sceneData);
        File.WriteAllText(sceneSaveFileName, sceneDataJson);
    }

    public void ClearSceneSave() {
        File.Delete(sceneSaveFileName);
    }

    public SceneData LoadScene() {
        if (File.Exists(sceneSaveFileName)) {
            string sceneDataJson = File.ReadAllText(sceneSaveFileName);
            if (!string.IsNullOrEmpty(sceneDataJson)) {
                SceneData sceneData = JsonConvert.DeserializeObject<SceneData>(sceneDataJson);
                return sceneData;
            }
        }

        return null;
    }
}
