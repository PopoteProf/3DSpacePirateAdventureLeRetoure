using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class RTSManager : MonoBehaviour
{
    public static RTSManager Instance;

    public event EventHandler<int> OnRessurceChange;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _prefabsArrow;
    [SerializeField] private GameObject _prefabsAttack;
    [SerializeField] private IRtsSelectable _selectedUnit;

    [Space(10), Header("ConstructionPart")]
    [SerializeField] private bool _isInContructMod;
    [SerializeField] private RTSBuildingController _prefabBuildingController;
    [SerializeField] private SORTSBuilding _sortsBuilding;
    [SerializeField] private SORTSBuilding _currentsortsBuilding;
    [SerializeField] private int _defaultRessource = 0;

    private int _currenteRessouces = 0;
    
    private RTSBuildingController _buildingController;
    
    public int CurrentRessources { get => _currenteRessouces; }

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        ChangeRessource(_defaultRessource);
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            SetConstructionMode(_sortsBuilding);
            SelectNewSelection(null);
        }

        if (_isInContructMod) {
            ManageBuildingMode();
        }
        else {
            ManageSelection();
        }
    }

    private void ManageSelection() {
        if (Input.GetButtonDown("Fire1")&&!IsPointerOnUI()) {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (hit.collider.GetComponent<IRtsSelectable>()!=null) {
                    SelectNewSelection(hit.collider.GetComponent<IRtsSelectable>());
                }
                else SelectNewSelection( null);
            }
        }
        
        if (Input.GetButtonDown("Fire2")) {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (_selectedUnit!=null && _selectedUnit is RTSUnite) {

                    if (_selectedUnit is RTSUniteWorker) {
                        if (hit.collider.GetComponent<RTSRessource>()) {
                            (_selectedUnit as RTSUniteWorker).SetNewRessourceTarget(hit.collider.GetComponent<RTSRessource>());
                            return;
                        }
                    }
                    
                    RTSUnite unit = _selectedUnit as RTSUnite;
                    if (IsAttackableTarget(hit.collider)) {
                        unit.SetAttackTarget(hit.collider.GetComponent<IDamagable>());
                        Instantiate(_prefabsAttack, hit.collider.transform.position,Quaternion.identity);
                    }
                    else {
                        unit.SetDestination(hit.point);
                        unit.SetAttackTarget( null);
                        Instantiate(_prefabsArrow, hit.point, Quaternion.identity);
                    }
                }
                
            }
        }
    }

    private void ManageBuildingMode() {
        if (_buildingController == null) return;

        if (Input.GetKeyDown(KeyCode.A)) _buildingController.transform.eulerAngles += new Vector3(0, 45, 0);
        if (Input.GetKeyDown(KeyCode.E)) _buildingController.transform.eulerAngles += new Vector3(0, -45, 0);
        
        
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
            _buildingController.SetNewPosition( hit.point);

            if (Input.GetButtonDown("Fire1")) {
                if (_buildingController.IsSpaceFree) {
                    Instantiate(_currentsortsBuilding.PrefabBuild, hit.point,  _buildingController.transform.rotation);
                    ChangeRessource(-_currentsortsBuilding.Price);
                }
                CloseConstructionMode();
                
            }
        }
    }

    public void SetConstructionMode(SORTSBuilding soBuilding) {
        if (_buildingController == null) {
            _buildingController = Instantiate(_prefabBuildingController, Vector3.zero, Quaternion.identity);
        }
        else {
            _buildingController.gameObject.SetActive(true);
        }
        _buildingController.SetNewSOBuilding(soBuilding);
        _currentsortsBuilding = soBuilding;
        _isInContructMod = true;
    }

    private void CloseConstructionMode() {
        _buildingController.gameObject.SetActive(false);
        _isInContructMod = false;
    }

    

    private void SelectNewSelection(IRtsSelectable selectable)
    {
        if (_selectedUnit == selectable) return;

        if (_selectedUnit!=null) _selectedUnit.SetSelected(false);
        _selectedUnit = selectable;
        if (_selectedUnit!=null) _selectedUnit.SetSelected(true);
    }
    
    

    private bool IsAttackableTarget(Collider col) {
      if(col.GetComponent<IDamagable>() == null)return false;
      if(col.GetComponent<IDamagable>().GetAlignment() != IDamagable.Alignment.Ennemy)return false;
      if(col.GetComponent<IDamagable>().IsDead())return false;
      return true;
    }

    public void ChangeRessource(int change) {
        _currenteRessouces = Mathf.Clamp(_currenteRessouces += change, 0, 100000);
        OnRessurceChange?.Invoke(this, _currenteRessouces);
    }

    public bool IsPointerOnUI()
    {
        if (EventSystem.current != null) return EventSystem.current.IsPointerOverGameObject();
        return false;
    }
}