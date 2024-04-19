using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider: MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    private GameObject _ParentObject;
    public GameObject ParentObject {get { return _ParentObject; } set { _ParentObject = value; } }

    private Transform cam;

    private void Start()
    {
        transform.SetParent(UImanager.Instance.Wroldcanvas.transform);
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if(cam != null)
        {
            transform.position = _ParentObject.transform.position + Vector3.up*2.5f;
            transform.LookAt(transform.position);
        }
    }
    public void SetTransform(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetValue(float current , float max)
    {
        float value = (float)(current / max);
        _slider.value = value;
    }
}
