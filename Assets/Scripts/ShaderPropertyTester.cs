using Unity.Multiplayer.Center.Common;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderPropertyTester : MonoBehaviour {
    [SerializeField] private string _propertyname;
    [SerializeField] private float _effectTime;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0,0,1,1);

    private Renderer render;
    private bool _doEffect;
    private float _timer;
    
    void Update() {
        if (_doEffect) {
            _timer += Time.deltaTime;
            render.material.SetFloat(_propertyname, _animationCurve.Evaluate(_timer/_effectTime));
            if (_timer >= _effectTime) {
                render.material.SetFloat(_propertyname, _animationCurve.Evaluate(1));
                _doEffect = false;
                
            }
        }
    }

    [ContextMenu("Start Effect")]
    private void StartEffect() {
        render = GetComponent<Renderer>();

        if (render == null) {
            Debug.LogWarning("Pas de renderer sur l'objet !!");
            return;
        }
        _timer = 0;
        render.material.SetFloat(_propertyname, _animationCurve.Evaluate(0));
        _doEffect = true;
    }
}
