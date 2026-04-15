using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private FloatVariable _playerHealthPercent;

    #region Hidden
    //private void OnEnable() => _playerHealthPercent.OnValueChanged += SetPercent;

    //private void OnDisable() => _playerHealthPercent.OnValueChanged -= SetPercent;

    //public void SetPercent(float InPercent)
    //{
    //    if(InPercent < 0 || InPercent > 1f || Mathf.Approximately(_fillImage.fillAmount,InPercent))
    //    {
    //        return;
    //    }

    //    _fillImage.fillAmount = InPercent;
    //}
    #endregion

    private void Update()
    {
        _fillImage.fillAmount = _playerHealthPercent.Value;
    }
}
