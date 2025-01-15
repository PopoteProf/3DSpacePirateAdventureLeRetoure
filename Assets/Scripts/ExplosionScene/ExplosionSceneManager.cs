using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExplosionSceneManager : MonoBehaviour
{

    

     [SerializeField] private Toggle _toggleSmallExplotion;
     [SerializeField] private Toggle _toggleBigExplosion;
     [SerializeField] private Button _bpError;
     [SerializeField] private GameObject _prefabBigExplosion;
     [SerializeField] private GameObject _prefabSmallExplosion;
     [SerializeField] private AudioElement _aeSmallExplosion;
     [SerializeField] private AudioElement _aeBigExplosion;
     [SerializeField] private AudioElement _aeError;
     [SerializeField] private AudioElement _aeSelection;

     [Space(10), Header("Impulse Sources")] 
     [SerializeField] private CinemachineImpulseSource _isBigExplostion;
     [SerializeField] private CinemachineImpulseSource _isSmallExplostion;
     [SerializeField] private CinemachineImpulseSource _isError;
     [SerializeField] private CinemachineImpulseSource _isSelect;
     
     private Camera _camera;
     private bool _isSmallExplosion;
    void Start() {
        _camera = Camera.main;    
        
        _toggleBigExplosion.onValueChanged.AddListener(ChangeBigExplosion);
        _toggleSmallExplotion.onValueChanged.AddListener(ChangesmallExplosion);
        _bpError.onClick.AddListener(UIOnErrorClick);
    }

    private void ChangeBigExplosion(bool value)
    {
        if (value) SetSmallExplosion(false);
        else SetSmallExplosion(true);
    }
    private void ChangesmallExplosion(bool value)
    {
        if (value) SetSmallExplosion(true);
        else SetSmallExplosion(false);
    }

    private void SetSmallExplosion(bool value) {
        if (value != _isSmallExplosion) {
            if(_isSelect)_isSelect.GenerateImpulse();
            AudioManager.Instance.PlaySFX(_aeSelection);
        }
        _isSmallExplosion = value;
    }

    private void UIOnErrorClick() {
        if(_isError)_isError.GenerateImpulse();
        AudioManager.Instance.PlaySFX(_aeError);
    }




    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            RaycastHit hit;
            if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit)) {
                if (_isSmallExplosion) {
                    Instantiate(_prefabSmallExplosion, hit.point, Quaternion.identity);
                    AudioManager.Instance.PlaySFX(_aeSmallExplosion);
                    if(_isSmallExplostion)_isSmallExplostion.GenerateImpulse();
                }
                else {
                    Instantiate(_prefabBigExplosion, hit.point, Quaternion.identity);
                   // AudioManager.Instance.PlaySFX(_aeBigExplosion);
                    if(_isBigExplostion)_isBigExplostion.GenerateImpulse();
                }
            }
        }
    }
}
