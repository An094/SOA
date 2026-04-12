using UnityEngine;

public class MiniGameCamera : MonoBehaviour
{
    private IWeapon _cachedWeapon;
    //private void Update()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if(Physics.Raycast(ray, out RaycastHit hit))
    //        {
    //            if(hit.collider.TryGetComponent(out _cachedWeapon) && _cachedWeapon.CanInteract())
    //            {
    //                Debug.Log("Start Interact");
    //                _cachedWeapon.StartInteract();
    //            }
    //        }
    //    }else if(_cachedWeapon != null && Input.GetMouseButtonUp(0))
    //    {
    //        _cachedWeapon.StopInteract();
    //        _cachedWeapon = null;
    //    }
    //}
}
