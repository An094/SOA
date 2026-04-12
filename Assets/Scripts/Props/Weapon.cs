using UnityEngine;
using Utilities;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponType _weaponType;

    private bool _bIsInteracting = false;
    private float _distanceToCamera;


    public WeaponType GetWeaponType() => _weaponType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IWeapon otherWeapon))
        {
            if(_weaponType.IsWinner(otherWeapon.GetWeaponType()))
            {
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        _bIsInteracting = true;
        _distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        _bIsInteracting = false;
    }

    private void Update()
    {
        if(_bIsInteracting)
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = _distanceToCamera;
            var mousePosition = Camera.main.ScreenToWorldPoint(mousePoint);
            //Debug.Log(string.Format("Input: {0} mousePostion: {1}", Input.mousePosition, mousePosition));
            transform.position = mousePosition.With(y: 2);
        }
    }
}
