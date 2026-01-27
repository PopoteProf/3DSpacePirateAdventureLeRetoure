
using UnityEngine;

public class TopDownPuzzleFootStep : MonoBehaviour
{
    [SerializeField] private AudioElement _aEFootStep;
    
    [SerializeField] private Transform _leftFooTransform;
    [SerializeField] private Transform _rightFootTransform;
    [SerializeField] private GameObject _prefabsFootSplash;
    public void OnFootStepAudio() {
        if(AudioManager.Instance!=null) AudioManager.Instance.PlaySFX(_aEFootStep);
    }
    
    public void FootStepLeft()
    {
        if (_leftFooTransform == null) return;
        OnFootStepAudio();
        if (_leftFooTransform == null || _prefabsFootSplash == null) return;
        Instantiate(_prefabsFootSplash, _leftFooTransform.position, Quaternion.identity);
    }
    public void FootStepRight() {
        if (_rightFootTransform == null) return;
        OnFootStepAudio();
        if (_rightFootTransform == null || _prefabsFootSplash == null) return;
        Instantiate(_prefabsFootSplash, _rightFootTransform.position, Quaternion.identity);
    }
}
