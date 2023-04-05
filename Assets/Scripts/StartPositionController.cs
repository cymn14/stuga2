using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPositionController : MonoBehaviour
{
    void Start()
    {
        // Deaktiviere das Spielobjekt beim Start des Spiels
        gameObject.SetActive(false);
    }
}
