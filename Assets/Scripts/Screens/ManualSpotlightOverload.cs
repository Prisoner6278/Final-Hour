using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSpotlightOverload : MonoBehaviour
{

    public void ActivatePlayerSpotlight()
    {
        GameObject.FindWithTag("Player").GetComponentInChildren<ActivateSpotlightPlay>().InstantActivateSpotlight();
    }

    public void DeactivatePlayerSpotlight()
    {
        GameObject.FindWithTag("Player").GetComponentInChildren<ActivateSpotlightPlay>().InstantDeActivateSpotlight();
    }
}
