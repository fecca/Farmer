using ARPG.Moving;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputListener : MonoBehaviour
{
    private Raycaster _raycaster;
    private IInteractable interactable;

    [Inject]
    private void Construct(Raycaster raycaster)
    {
        _raycaster = raycaster;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (_raycaster.RaycastAllLayers(Input.mousePosition, out var hit))
        {
            interactable?.Hover(false);

            interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                interactable.Interact();
            }
            else
            {
                interactable.Hover(true);
            }
        }
        else if (interactable != null)
        {
            interactable.Hover(false);
            interactable = null;
        }
    }
}