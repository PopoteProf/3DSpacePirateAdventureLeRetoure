using UnityEngine;

public class RTSBuildingGhost : MonoBehaviour {
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private Color _colorValidate = new Color(0, 1, 0, 0.2f);
    [SerializeField] private Color _colorError = new Color(1, 0, 0, 0.2f);

    public void ChangeValidateState(bool isValidate)
    {
        foreach (var renderer in _renderers)
        {
            if (renderer == null) continue;
            if (isValidate) renderer.material.color = _colorValidate;
            else renderer.material.color = _colorError;

        }
    }
}