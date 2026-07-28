using UnityEngine;
using UnityEngine.EventSystems;

// Click/drag on a circle to pick a ball spin offset, like pool english.
public class BallSpinSelector : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public static BallSpinSelector Instance { get; private set; }

    [Header("References")]
    [SerializeField] private RectTransform circleRect; // defines the clickable circle
    [SerializeField] private RectTransform handle; // marker showing the chosen spot
    [SerializeField] private Camera uiCamera; // leave empty for Screen Space - Overlay

    public Vector2 SpinOffset { get; private set; } // -1..1 per axis, x = right, y = up

    void Awake(){
        Instance = this;
        SetOffset(Vector2.zero);
    }
    public void OnPointerDown(PointerEventData eventData) => UpdateOffset(eventData);
    public void OnDrag(PointerEventData eventData) => UpdateOffset(eventData);

    void UpdateOffset(PointerEventData eventData){
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(circleRect, eventData.position, uiCamera, out Vector2 localPoint))
            return;
        Vector2 normalized = localPoint / (circleRect.rect.width * 0.5f);
        SetOffset(normalized.magnitude > 1f ? normalized.normalized : normalized);
    }
    void SetOffset(Vector2 offset){
        SpinOffset = offset;
        if (handle != null) handle.anchoredPosition = offset * (circleRect.rect.width * 0.5f);
    }
    // Clears the spin choice, ready for the next kick
    public void ResetOffset() => SetOffset(Vector2.zero);
}
