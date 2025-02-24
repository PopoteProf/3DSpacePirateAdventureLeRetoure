using UnityEngine;

public interface IRtsSelectable {
    public void SetSelected(bool isSelected);
    
    public bool CanBeSelected();
}