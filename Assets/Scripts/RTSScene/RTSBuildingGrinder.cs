using UnityEngine;

public class RTSBuildingGrinder : RTSBuilding, IRtsRessourceDepos
{
    [SerializeField] private float _ressourceDropDistance = 5;
    public Vector3 GetPosition() => transform.position;
    public float GetDropDistance() => _ressourceDropDistance;

    public void TransferRessources(int ressources) {
        if (RTSManager.Instance != null) {
            RTSManager.Instance.ChangeRessource(ressources);
        }
    }

    private void Start()
    {
        IRtsRessourceDepos.AllDepos.Add(this);
    }

    private void OnDestroy()
    {
        IRtsRessourceDepos.AllDepos.Remove(this);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _ressourceDropDistance);
    }
}