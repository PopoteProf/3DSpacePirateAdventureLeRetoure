using UnityEngine;

public class CaisseMoment : MonoBehaviour {
    [SerializeField] private Animator _animator;
    public void TriggerAnimator() {
        if (_animator != null) _animator.SetBool("IsFall", true);
    }
}