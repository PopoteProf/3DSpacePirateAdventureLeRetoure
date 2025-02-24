using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class RTSRessource : MonoBehaviour
{
    public static List<RTSRessource> AllRessources = new List<RTSRessource>();

    [SerializeField] private int _maxRessources = 100;
    [SerializeField] private int _currentRessouces;
    private void Start() {
        AllRessources.Add(this);
        if (_currentRessouces == 0) _currentRessouces = _maxRessources;
    }
    
    public static RTSRessource GetClosetRessourceDepot(Vector3 pos) {
        RTSRessource bestDepot = null;
        float bestdistance = Mathf.Infinity; 
        
        foreach (var rtsRessource in AllRessources)
        {
            float distance = Vector3.Distance(rtsRessource.transform.position, pos);
            if (distance < bestdistance) {
                bestDepot = rtsRessource;
                bestdistance = distance;
            }
        }
        return bestDepot;
    }

    public int MineRessource(int miningAmount) {
        if (_currentRessouces - miningAmount <= 0) {
           
            _currentRessouces = 0;
            Destroy(gameObject);
            return _currentRessouces;
        }

        _currentRessouces -= miningAmount;
        return miningAmount;
    }

    private void OnDestroy() {
        AllRessources.Remove(this);
    }
}