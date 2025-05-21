using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FindCamera : UdonSharpBehaviour
{
    [SerializeField]
    private GameObject cameraReference;

    void Start()
    {
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        if (localPlayer != null)
        {
            // 플레이어의 Head 위치 가져오기
            var headPosition = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            var headRotation = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            
            // Head 오브젝트 찾기
            var cameraParent = GameObject.Find("PlayerCamera");
            if (cameraParent != null)
            {
                Transform headTransform = cameraParent.transform.Find("Head");
                if (headTransform != null)
                {
                    cameraReference = headTransform.gameObject;
                    Debug.Log($"Found VR Camera at: {cameraReference.transform.position}");
                }
                else
                {
                    Debug.LogWarning("Head object not found under Camera");
                }
            }
            else
            {
                Debug.LogWarning("Camera object not found in scene");
            }
        }
    }
}
