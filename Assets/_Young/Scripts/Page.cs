
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Page : UdonSharpBehaviour
{
    public PPTScript ppt;

    public override void Interact()
    {
        ppt.NextPage();
    }
}
