using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Ball.OnLightningDisable += OffLightning;
        Ball.OnLightningEnable += OnLightning;
    }

    private void OnDestroy()
    {
        Ball.OnLightningDisable -= OffLightning;
        Ball.OnLightningEnable -= OnLightning;
    }

    void OnLightning()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OffLightning()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
