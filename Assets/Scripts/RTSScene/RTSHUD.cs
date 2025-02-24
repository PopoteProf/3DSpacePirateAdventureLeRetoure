using TMPro;
using UnityEngine;

public class RTSHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtRessource;

    public void Start()
    {
        if (RTSManager.Instance != null) {
            RTSManager.Instance.OnRessurceChange+= InstanceOnOnRessurceChange;
        }
    }

    private void InstanceOnOnRessurceChange(object sender, int e)
    {
        _txtRessource.text = "Ressource" + e + "$";
    }
}