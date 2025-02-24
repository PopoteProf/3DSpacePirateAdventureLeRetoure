using System;
using UnityEngine;
using UnityEngine.UI;

public class RTSBuildingCommendCenter : RTSBuilding , IRtsRessourceDepos
{
    
    
    [SerializeField] private float _ressourceDropDistance = 5;

    [SerializeField] private SORTSBuilding _soGinder;
    [SerializeField] private SORTSBuilding _soTurret;
    [SerializeField] private SORTSBuilding _soFactory;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private RTSUnite _prfWorker;
    [SerializeField] private float _workerCreatingTime=10;
    
    [Space(10), Header("UIParte")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _bpGrinder;
    [SerializeField] private Button _bpTurret;
    [SerializeField] private Button _bpFactory;
    [SerializeField] private Button _bpWorker;
    [SerializeField] private Image _imgProgressBar;


    private bool _isCreatingWorker;
    private float _craftingTimer;
    public Vector3 GetPosition() => transform.position;
    public float GetDropDistance()=>_ressourceDropDistance;
    public void TransferRessources(int ressources) {
        if (RTSManager.Instance != null) {
            RTSManager.Instance.ChangeRessource(ressources);
        }
        InitializeCanvas();
    }

    private void UIButtonWorker() {
        _isCreatingWorker = true;
        RTSManager.Instance.ChangeRessource(-30);
        InitializeCanvas();
    }

    private void UIButtonFactory() {
        RTSManager.Instance.SetConstructionMode(_soFactory);
    }

    private void UIButtonTurret() {
        RTSManager.Instance.SetConstructionMode(_soTurret);
    }

    private void UIButtonGrinder() {
        RTSManager.Instance.SetConstructionMode(_soGinder);
    }

    private void Start() {
        IRtsRessourceDepos.AllDepos.Add(this);
        _canvas.worldCamera= Camera.main;
        _canvas.gameObject.SetActive(false);
        _bpGrinder.onClick.AddListener(UIButtonGrinder);
        _bpTurret.onClick.AddListener(UIButtonTurret);
        _bpFactory.onClick.AddListener(UIButtonFactory);
        _bpWorker.onClick.AddListener(UIButtonWorker);
        _imgProgressBar.fillAmount = 0;
    }

    private void Update()
    {
        if( _isCreatingWorker)ManagerWorkerCreation();
        _canvas.transform.forward = _canvas.transform.position - _canvas.worldCamera.transform.position;
    }

    private void ManagerWorkerCreation() {
        _craftingTimer += Time.deltaTime;
        _imgProgressBar.fillAmount = _craftingTimer / _workerCreatingTime;
        if (_craftingTimer >= _workerCreatingTime) {
            _imgProgressBar.fillAmount = 0;
            Instantiate(_prfWorker, _spawnPoint.position, _spawnPoint.rotation);
            _isCreatingWorker = false;
            _craftingTimer = 0;
            InitializeCanvas();
        }
    }

    private void OnDestroy() {
        IRtsRessourceDepos.AllDepos.Remove(this);
        
    }
    
    public override void SetSelected(bool isSelected) {
        _canvas.gameObject.SetActive(isSelected);
        InitializeCanvas();
        base.SetSelected(isSelected);
    }

    private void InitializeCanvas() {
        _bpGrinder.interactable = RTSManager.Instance.CurrentRessources >= 50;
        _bpTurret.interactable = RTSManager.Instance.CurrentRessources >= 50;
        _bpFactory.interactable = RTSManager.Instance.CurrentRessources >= 100;
        _bpWorker.interactable = RTSManager.Instance.CurrentRessources >= 30 && !_isCreatingWorker;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _ressourceDropDistance);
    }
}