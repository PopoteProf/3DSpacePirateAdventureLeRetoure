using UnityEngine;
using UnityEngine.UI;

public class RTSBuildingFactory : RTSBuilding
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _bpFightingUhit;
    [SerializeField] private Image _imgProgress;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private RTSUnite _prfWorker;
    [SerializeField] private float _workerCreatingTime=10;

    private bool _isCreatingWorker;
    private float _craftingTimer;
    public override void SetSelected(bool isSelected) {
        _canvas.gameObject.SetActive(isSelected);
        InitializeCanvas();
        base.SetSelected(isSelected);
    }
    
    private void Start() {
        _canvas.worldCamera= Camera.main;
        _canvas.gameObject.SetActive(false);
        _bpFightingUhit.onClick.AddListener(UIOnFightUniteClick);
        _imgProgress.fillAmount = 0;
    }

    private void UIOnFightUniteClick()
    {
        _isCreatingWorker = true;
        RTSManager.Instance.ChangeRessource(-70);
        InitializeCanvas();
    }
    

    private void Update()
    {
        if( _isCreatingWorker)ManagerWorkerCreation();
        _canvas.transform.forward = _canvas.transform.position - _canvas.worldCamera.transform.position;
    }
    private void ManagerWorkerCreation() {
        _craftingTimer += Time.deltaTime;
        _imgProgress.fillAmount = _craftingTimer / _workerCreatingTime;
        if (_craftingTimer >= _workerCreatingTime) {
            _imgProgress.fillAmount = 0;
            Instantiate(_prfWorker, _spawnPoint.position, _spawnPoint.rotation);
            _isCreatingWorker = false;
            _craftingTimer = 0;
            InitializeCanvas();
        }
    }
    
    private void InitializeCanvas() {
        _bpFightingUhit.interactable = RTSManager.Instance.CurrentRessources >= 70 && !_isCreatingWorker;
    }
}