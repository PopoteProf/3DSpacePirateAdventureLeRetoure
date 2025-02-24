using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IRtsRessourceDepos {
    public static List<IRtsRessourceDepos> AllDepos = new List<IRtsRessourceDepos>();
    
    public static IRtsRessourceDepos GetClosetRessourceDepot(Vector3 pos) {
        IRtsRessourceDepos bestDepot = null;
        float bestdistance = Mathf.Infinity; 
        
        foreach (var depo in AllDepos)
        {
            float distance = Vector3.Distance(depo.GetPosition(), pos);
            if (distance < bestdistance) {
                bestDepot = depo;
                bestdistance = distance;
            }
        }
        return bestDepot;
    }

    public Vector3 GetPosition();
    public float GetDropDistance();
    public void TransferRessources(int ressources);
}