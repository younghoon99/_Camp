
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Easteregg : UdonSharpBehaviour
{
    public Transform spawnPoint;
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer)
        {
            player.TeleportTo(spawnPoint.position, spawnPoint.rotation);
        }
    }
}
