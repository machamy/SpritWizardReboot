using UnityEngine;
using UnityEngine.UI;

public class PopUpUIHandler : MonoBehaviour
{
    [SerializeField] private float notClickedLineDistance = 3f;
    [SerializeField] private float ClickedLineDistance = 10f;

    private Vector2 notClickedEffectDistance => new Vector2(notClickedLineDistance, -notClickedLineDistance);
    private Vector2 clickedEffectDistance => new Vector2(ClickedLineDistance, -ClickedLineDistance);

    private GameObject currentOpenPopUp;
    private Outline currentClickedOutline;

    public void ClosePopUp()
    {
        if (currentOpenPopUp == null) return;

        currentOpenPopUp.SetActive(false);
        currentOpenPopUp = null;

        currentClickedOutline.effectDistance = notClickedEffectDistance;
        currentClickedOutline = null;
    }

    public void OpenPopUp(GameObject popUp, Outline outline)
    {
        ClosePopUp();

        popUp.SetActive(true);
        currentOpenPopUp = popUp;
        
        outline.effectDistance = clickedEffectDistance;
        currentClickedOutline = outline;
    }
}
