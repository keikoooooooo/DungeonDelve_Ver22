using System;
using TMPro;
using UnityEngine;

public class DMGPopUp : MonoBehaviour, IPooled<DMGPopUp>
{
   public TextMeshProUGUI textPopUp;
   public Camera mainCamera;
   
   [Space]
   public AnimationCurve opacityCurve;
   public AnimationCurve heightCurve;
   
   [Space]
   public AnimationCurve DMGScaleCurve;
   public AnimationCurve CRDScaleCurve;

   [Space]
   [Tooltip("Màu text khi nhận sát thương thường")]
   public Color DMGColor;
   [Tooltip("Màu text khi nhận sát thương bạo kích")]
   public Color CRDColor;

   
   private float _time;
   private bool _isCrit;
   private Color _currentColor;
   private Vector3 _originPosition;
   private Vector3 _originScale;
   
   private void Update()
   {
      transform.forward = mainCamera.transform.forward;
      
      _currentColor.a = opacityCurve.Evaluate(_time);
      textPopUp.color = _currentColor;

      transform.localScale = _isCrit ? _originScale * CRDScaleCurve.Evaluate(_time) : _originScale * DMGScaleCurve.Evaluate(_time);
      transform.position = _originPosition + new Vector3(0, 1 + heightCurve.Evaluate(_time), 0);
      
      _time += Time.deltaTime;
   }
   
   public void ShowDMGPopUp(int _damage, bool isApplyCRIT)
   {
      _isCrit = isApplyCRIT;
      _currentColor = isApplyCRIT ? CRDColor : DMGColor;
      
      _originScale = new Vector3(.01f, .01f, .01f);
      _originPosition = transform.position;
      
      textPopUp.color = _currentColor;
      textPopUp.text = $"{_damage}";
   }


   
   public void Release() => ReleaseCallback?.Invoke(this);
   public Action<DMGPopUp> ReleaseCallback { get; set; }
}
