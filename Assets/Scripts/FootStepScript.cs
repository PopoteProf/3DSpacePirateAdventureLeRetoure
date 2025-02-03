
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class FootStepScript : MonoBehaviour
{
    [SerializeField] private AudioElement _footStep;
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] private Transform _leftFooTransform;
    [SerializeField] private Transform _rightFootTransform;
    [SerializeField] private GameObject _prefabsFootSplash;
    
    public void FootStep() {
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = _footStep.GetSound();
        audioSource.pitch = _footStep.GetPitch();
        audioSource.volume = _footStep.Volume;
        audioSource.outputAudioMixerGroup = _mixerGroup;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length+1);
    }
    
    public void FootStepLeft() {
       FootStep();
       Instantiate(_prefabsFootSplash, _leftFooTransform.position, quaternion.identity);
    }
    public void FootStepRight() {
        FootStep();
        Instantiate(_rightFootTransform, _leftFooTransform.position, quaternion.identity);
    }
    
    
}
