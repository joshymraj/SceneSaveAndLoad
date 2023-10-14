using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    [SerializeField]
    private int NumberOfBuildings = 3;

    [SerializeField]
    private GameObject buildingMesh;

    [SerializeField]
    private Dropdown buildingPicker;

    [SerializeField]
    private MessageBlinker messageBlinker;

    public DataManager dataManager;

    public BuildingType SelectedBuildingType {
        get;
        private set;
    }

    private List<MeshLoader> _sceneBuildingMeshes;

    private SceneData _sceneData;

    void Start() {
        PopulateBuildingOptions();

        SelectedBuildingType = BuildingType.CorporateHeadOffice;
    }

    // Update is called once per frame
    void Update() {

    }

    private void PopulateBuildingOptions() {
        List<Dropdown.OptionData> buildingOptions = new List<Dropdown.OptionData>();

        for (int i = 1; i <= NumberOfBuildings; i++) {
            buildingOptions.Add(new Dropdown.OptionData(((BuildingType)i).ToString()));// string.Format("Building {0}", i.ToString())));
        }

        buildingPicker.AddOptions(buildingOptions);
    }

    public void Load() {
        _sceneData = dataManager.LoadScene();
        if (_sceneData != null) {
            foreach (BuildingData buildingData in _sceneData.Buildings) {
                AddBuilding(buildingData.BuildingType, buildingData.Position, buildingData.Rotation);
            }
            messageBlinker.ShowMessage("Loaded last saved scene.");
        }
    }

    public void Clear() {
        if (_sceneBuildingMeshes == null) {
            return;
        }
        // dataManager.ClearSceneSave(); // Uncomment if we need to clear save also.
        foreach (MeshLoader buildingMesh in _sceneBuildingMeshes) {
            Destroy(buildingMesh.gameObject);
        }

        _sceneBuildingMeshes.Clear();

        messageBlinker.ShowMessage(" Cleared scene.");
    }

    public void Save() {
        if (_sceneBuildingMeshes != null) {
            if (_sceneData == null) {
                _sceneData = new SceneData();
            }
            _sceneData.Buildings.Clear();
            foreach (MeshLoader buildingMesh in _sceneBuildingMeshes) {
                _sceneData.Buildings.Add(new BuildingData {
                    BuildingType = buildingMesh.BuildingType,
                    Position = new SimpleVector3(buildingMesh.Position.x, buildingMesh.Position.y, buildingMesh.Position.z),
                    Rotation = new SimpleVector3(buildingMesh.Rotation.x, buildingMesh.Rotation.y, buildingMesh.Rotation.z)
                });
            }

            dataManager.SaveScene(_sceneData);

            messageBlinker.ShowMessage("Scene saved.");
        }
    }

    public void AddRandomBuilding() {
        SelectedBuildingType = (BuildingType)Random.Range(1, NumberOfBuildings + 1);
        AddBuilding(SelectedBuildingType);
    }

    public void BuildingPicker_SelectedIndexChanged() {
        SelectedBuildingType = (BuildingType)(buildingPicker.value + 1);
    }

    ///Spawns specific building in specific position in scene with specfied rotation
    public void AddBuilding(BuildingType buildingType, SimpleVector3 position, SimpleVector3 rotation) {
        var model = Instantiate(buildingMesh, new Vector3(position.x, position.y, position.z), Quaternion.Euler(new Vector3(rotation.x, rotation.y, rotation.z)));
        var modelObj = model.GetComponent<MeshLoader>();
        modelObj.Init(buildingType);
        if (_sceneBuildingMeshes == null) {
            _sceneBuildingMeshes = new List<MeshLoader>();
        }
        _sceneBuildingMeshes.Add(modelObj);
    }

    ///Spawns specific building at center of the scene with default transform
    public void AddBuilding(BuildingType buildingType) {
        var model = Instantiate(buildingMesh);
        var modelObj = model.GetComponent<MeshLoader>();
        modelObj.Init(buildingType);
        if (_sceneBuildingMeshes == null) {
            _sceneBuildingMeshes = new List<MeshLoader>();
        }
        _sceneBuildingMeshes.Add(modelObj);
    }

    public void AddBuilding() {
        AddBuilding(SelectedBuildingType);
    }
}

public class SceneData {
    public List<BuildingData> Buildings { get; set; }

    public SceneData() {
        Buildings = new List<BuildingData>();
    }
}

public class BuildingData {
    [JsonProperty("BuildingType")]
    public BuildingType BuildingType {
        get;
        set;
    }

    [JsonProperty("Position")]
    public SimpleVector3 Position {
        get;
        set;
    }

    [JsonProperty("Rotation")]
    public SimpleVector3 Rotation {
        get;
        set;
    }
}

//Json serialization friendly Vector3
public struct SimpleVector3 {
    public SimpleVector3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public float x;
    public float y;
    public float z;
}

public enum BuildingType {
    CorporateHeadOffice = 1,
    GovernmentComplex,
    TwinTower
}
