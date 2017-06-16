using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

    public float _speed = -0.2f;
    [Range(-2000, -1250)]
    public int _leftTrigger = -1600;
    [Range(1250, 2000)]
    public int _rightTrigger = 1600;

    void Update ()
    {
        Vector3 aux = transform.localPosition;
        if (transform.localPosition.x < _leftTrigger)
        {
            aux.x = _rightTrigger;
            transform.localPosition = aux;
        }
        else
        {
            aux = transform.localPosition;
            aux.x += _speed * Time.deltaTime;
            transform.localPosition = aux;
        }
    }
}
