using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FireTrigger : UdonSharpBehaviour
{
    public GameObject Fire;
   
    [UdonSynced]
    private bool isActive;

    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            Fire.SetActive(isActive);
        }
    }

    public override void Interact()
    {
        IsActive = !IsActive;
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        RequestSerialization();
    }

    public override void OnDeserialization()
    {
        Fire.SetActive(isActive);
    }
}
    