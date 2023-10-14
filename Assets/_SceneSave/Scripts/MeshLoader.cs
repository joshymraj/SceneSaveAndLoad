using System.IO;
using UnityEngine;
using UnityExtension;

[RequireComponent(typeof(MeshFilter))]
public class MeshLoader : MonoBehaviour {
    private const string BUILDING_MODEL_PATH_FORMAT = @"Assets/_SceneSave/Models/building_0{0}.obj";

    public BuildingType BuildingType {
        get;
        private set;
    }

    public Vector3 Position {
        get {
            return transform.position;
        }
    }

    public Vector3 Rotation {
        get {
            return transform.rotation.eulerAngles;
        }
    }

    public void Init(BuildingType buildingType) {
        using (var lStream = new FileStream(string.Format(BUILDING_MODEL_PATH_FORMAT, ((int)buildingType).ToString()), FileMode.Open)) {
            var lOBJData = OBJLoader.LoadOBJ(lStream);
            var lMeshFilter = GetComponent<MeshFilter>();
            lMeshFilter.mesh.LoadOBJ(lOBJData);
        }

        BuildingType = buildingType;
        gameObject.AddComponent(typeof(BoxCollider));
    }
}