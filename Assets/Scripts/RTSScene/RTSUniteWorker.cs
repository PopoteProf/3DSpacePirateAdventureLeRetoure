using System;
using UnityEngine;

public class RTSUniteWorker : RTSUnite
{
    [Space(10), Header("WorkerParametters")]
    [SerializeField] private int _miningPower =1;
    [SerializeField] private float _miningSpeed = 0.5f;
    [SerializeField] private float _miningRange = 1.5f;
    [SerializeField] private int _maxRessourceStock = 10;
    [SerializeField] private int _currentRessourceStock;


    private float _timerMining;
    private bool _isAutoMining;
    private RTSRessource _currentRessource;
    private IRtsRessourceDepos _currentDepos;
    private bool _isStockFull {
        get => _maxRessourceStock <= _currentRessourceStock;
    }
    
    public void SetNewRessourceTarget(RTSRessource ressource) {
        _currentRessource = ressource;
        _navMeshAgent.SetDestination(_currentRessource.transform.position);
        _rtsUnitStat = RTSUnitStat.GoToRessource;
        _isAutoMining = true;
    }

    protected override void Update()
    {
        if( _isAutoMining&&!_isDead)ManageAutoMining();
        base.Update();
    }

    private void ManageAutoMining() {
        switch (_rtsUnitStat) {
            case RTSUnitStat.GoToRessource: ManageGotoRessource(); break;
            case RTSUnitStat.GoToDepot:ManageGoToDepot(); break;
            case RTSUnitStat.Mining:ManageMining(); break;
            default: throw new ArgumentOutOfRangeException();
        }
        
    }

    private void ManageGotoRessource() {
        if (IsInMiningRange())
        {
            _navMeshAgent.SetDestination(transform.position);
            StartMining();
        }
    }

    private void ManageMining() {
        if( _isStockFull) StartGoToDepot();
        if(_currentRessource==null) StartGoToRessource();

        _timerMining += Time.deltaTime;
        if (_timerMining >= _miningSpeed) {
            _currentRessourceStock+= _currentRessource.MineRessource(_miningPower);
            _timerMining = 0;
        }

    }

    private void ManageGoToDepot() {
        if(_currentRessource==null)StartGoToDepot();
        if (IsInDepotRange()) {
            _currentDepos.TransferRessources(_currentRessourceStock);
            _currentRessourceStock = 0;
            StartGoToRessource();
        }
    }

    private void StartGoToRessource()
    {
        RTSRessource ressource;
        if( _currentRessource==null)  ressource = RTSRessource.GetClosetRessourceDepot(transform.position);
        else ressource = _currentRessource;
        if (ressource == null) {
            _rtsUnitStat = RTSUnitStat.Idles;
            _isAutoMining = false;
            return;
        }
        _currentRessource = ressource;
        _navMeshAgent.SetDestination(_currentRessource.transform.position);
        _rtsUnitStat = RTSUnitStat.GoToRessource;
    }

    private void StartMining() {
        _timerMining = 0;
        _rtsUnitStat = RTSUnitStat.Mining;
    }

    private void StartGoToDepot() {
        _currentDepos = IRtsRessourceDepos.GetClosetRessourceDepot(transform.position);
        if (_currentDepos == null) {
            _rtsUnitStat = RTSUnitStat.Idles;
            _isAutoMining = false;
            return;
        }

        _navMeshAgent.SetDestination(_currentDepos.GetPosition());
        _rtsUnitStat = RTSUnitStat.GoToDepot;
    }
    
    private bool IsInMiningRange() {
        if (_currentRessource == null) return false;
        return Vector3.Distance(_currentRessource.transform.position, transform.position) <= _miningRange;
    }

    private bool IsInDepotRange()
    {
        if (_currentDepos == null) return false;
        return Vector3.Distance(_currentDepos.GetPosition(), transform.position) <= _currentDepos.GetDropDistance();
    }

    public override void SetDestination(Vector3 value) {
        _isAutoMining = false;
        base.SetDestination(value);
    }

    public override void SetAttackTarget(IDamagable target) {
        _isAutoMining = false;
        base.SetAttackTarget(target);
    }
}