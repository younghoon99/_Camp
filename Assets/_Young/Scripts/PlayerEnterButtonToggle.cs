using UdonSharp; // UdonSharp 라이브러리 사용
using UnityEngine; // Unity 엔진의 기본 기능 사용
using VRC.SDKBase; // VRC SDK의 기본 기능 사용
using VRC.Udon; // VRC Udon 기능 사용

// 플레이어가 특정 트리거 영역에 들어오거나 나갈 때 버튼의 활성화 상태를 제어하는 클래스
public class PlayerEnterButtonToggle : UdonSharpBehaviour
{
    // 버튼을 포함하는 게임 오브젝트
    public GameObject buttons;
    public GameObject ppt;

    // 플레이어가 트리거 영역에 들어올 때 호출되는 메서드
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        // 로컬 플레이어가 트리거에 들어오면 로컬플레이어(나)에게만 버튼을 활성화
        if(player == Networking.LocalPlayer) // 이 부분이 없다면 모든 플레이어가 버튼을 활성화 됨
        {
            buttons.SetActive(true); // 버튼 활성화
            ppt.SetActive(true);
        }
    }

    // 플레이어가 트리거 영역에서 나갈 때 호출되는 메서드
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        // 로컬 플레이어가 트리거에서 나가면 버튼을 비활성화
        if(player == Networking.LocalPlayer)
        {
            buttons.SetActive(false); // 버튼 비활성화
            ppt.SetActive(false);
        }
    }
}
