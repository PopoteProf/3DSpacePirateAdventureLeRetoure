using UnityEngine;

[CreateAssetMenu(fileName = "SO_RSTBuilding", menuName = "SO_RTS/RTSBuilding")]
public class SORTSBuilding : ScriptableObject {
    public RTSBuildingGhost PrefabsGhost;
    public Vector3 Size;
    public RTSBuilding PrefabBuild;
    public int Price;
}