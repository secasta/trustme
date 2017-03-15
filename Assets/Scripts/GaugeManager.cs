using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour {

    public Image _redBar1;
    public Image _redBar2;
    public Image _redBar3;
    public Image _greenBar1;
    public Image _greenBar2;
    public Image _greenBar3;

    private int _redCounter = 0;
    private int _greenCounter = 0;

    public bool AddRedBarAndContinue()
    {
        switch (_redCounter)
        {
            case 0:
                _redBar1.enabled = true;
                break;
            case 1:
                _redBar2.enabled = true;
                break;
            case 2:
                _redBar3.enabled = true;
                break;
        }
        _redCounter++;
        if (_redCounter < 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddGreenBarAndContinue()
    {
        switch (_greenCounter)
        {
            case 0:
                _greenBar1.enabled = true;
                break;
            case 1:
                _greenBar2.enabled = true;
                break;
            case 2:
                _greenBar3.enabled = true;
                break;
        }
        _greenCounter++;
        if (_greenCounter < 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
