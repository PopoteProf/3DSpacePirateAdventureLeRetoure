using System;
using UnityEngine;

public class RTSManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _prefabsArrow;
    [SerializeField] private GameObject _prefabsAttack;
    [SerializeField] private RTSUnite _selectedUnit;
    void Update() {

        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (hit.collider.GetComponent<RTSUnite>()) {
                    SelectNewUnit(hit.collider.GetComponent<RTSUnite>());
                }
                else SelectNewUnit( null);
            }
        }
        
        if (Input.GetButtonDown("Fire2")) {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {


                if (_selectedUnit) {
                    if (IsAttackableTarget(hit.collider)) {
                        _selectedUnit.SetAttackTarget(hit.collider.GetComponent<IDamagable>());
                        Instantiate(_prefabsAttack, hit.collider.transform.position, Quaternion.identity);
                    }
                    else {
                        _selectedUnit.SetDestination(hit.point);
                        _selectedUnit.SetAttackTarget( null);
                        Instantiate(_prefabsArrow, hit.point, Quaternion.identity);
                    }
                }
                
            }
        }
    }

    private void SelectNewUnit(RTSUnite newUnite) {
        if (_selectedUnit == newUnite) return;

        if (_selectedUnit) _selectedUnit.SetSelected(false);
        _selectedUnit = newUnite;
        if (_selectedUnit) _selectedUnit.SetSelected(true);
    }

    private bool IsAttackableTarget(Collider col) {
      if(col.GetComponent<IDamagable>() == null)return false;
      if(col.GetComponent<IDamagable>().GetAlignment() != IDamagable.Alignment.Ennemy)return false;
      if(col.GetComponent<IDamagable>().IsDead())return false;
      return true;
    }
}