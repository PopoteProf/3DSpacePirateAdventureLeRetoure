using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TopDownPuzzleFootStep : MonoBehaviour
{
    [SerializeField] private AudioElement _aEFootStep;
    
    [SerializeField] private Transform _leftFooTransform;
    [SerializeField] private Transform _rightFootTransform;
    [SerializeField] private GameObject _prefabsFootSplash;
    public void OnFootStep()
    {
        AudioManager.Instance.PlaySFX(_aEFootStep);
    }
    
    public void FootStepLeft() {
        OnFootStep();
        if (_leftFooTransform == null || _prefabsFootSplash == null) return;
        Instantiate(_prefabsFootSplash, _leftFooTransform.position, Quaternion.identity);
    }
    public void FootStepRight() {
        OnFootStep();
        if (_rightFootTransform == null || _prefabsFootSplash == null) return;
        Instantiate(_prefabsFootSplash, _rightFootTransform.position, Quaternion.identity);
    }
}
