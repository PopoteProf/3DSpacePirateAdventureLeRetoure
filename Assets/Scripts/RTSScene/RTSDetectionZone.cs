using System.Collections.Generic;
using UnityEngine;

public class RTSDetectionZone : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private float Range = 5;
    [SerializeField] private IDamagable.Alignment _tragetAlingnment = IDamagable.Alignment.Ally;   
    
    private List<IDamagable> _targets =new List<IDamagable>() ;

    public IDamagable GetFirstTarget() {
        ClearListFromNull();
        if( _targets.Count>0) return _targets[0];
        return null;
    }
    
    
    private void Start() {
        _sphereCollider.radius = Range;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Unite entre");
        if (other.CompareTag("Damagable")) {
            IDamagable damagable = other.GetComponent<IDamagable>();
            print("unit is damageble");
            if (IsValideTarget(damagable)&&!_targets.Contains(damagable)) {
                print("unit add to list");
                _targets.Add(damagable);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Damagable")) {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable != null && _targets.Contains(damagable)) {
                _targets.Remove(damagable);
            }
        }
    }

    private void ClearListFromNull() {
        for (int i = _targets.Count-1; i >= 0; i--) {
            if (!IsValideTarget(_targets[i]))
            {
                print("unit remove");
                _targets.RemoveAt(i);
            }
        }
    }

    private bool IsValideTarget(IDamagable damagable) {
        if (damagable == null) return false;
        if (damagable.IsDead()) return false;
        if (damagable.GetAlignment() != _tragetAlingnment) return false;
        return true;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , Range);
    }
}