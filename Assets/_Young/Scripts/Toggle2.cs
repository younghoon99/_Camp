using UdonSharp; // UdonSharp 라이브러리 사용
using UnityEngine; // Unity 엔진의 기본 기능 사용
using VRC.SDKBase; // VRC SDK의 기본 기능 사용
using VRC.Udon; // VRC Udon 기능 사용

// UdonBehaviourSyncMode를 수동으로 설정하여 동기화 제어
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Toggle2 : UdonSharpBehaviour
{
    // 활성화/비활성화할 거울 오브젝트
    public GameObject mirror;

    // 네트워크에서 동기화할 변수, 거울 활성화 상태를 저장
    [UdonSynced]
    private bool isActive;

    // isActive의 getter와 setter를 정의하여 상태 변경 시 오브젝트 활성화 상태를 업데이트
    public bool IsActive
    {
        get => isActive; // 현재 활성화 상태 반환
        set
        {
            isActive = value; // 상태를 업데이트
            mirror.SetActive(isActive); // mirror 오브젝트 활성화 상태를 변경
        }
    }

    // 상호작용 시 호출되는 메서드
    public override void Interact()
    {
        IsActive = !IsActive; // 현재 상태를 반전시킴
        Networking.SetOwner(Networking.LocalPlayer, gameObject); // 로컬 플레이어가 이 오브젝트의 소유자가 되도록 설정
        RequestSerialization(); // 변경된 상태를 네트워크에 동기화 요청
    }

    // 네트워크에서 데이터가 동기화될 때 호출되는 메서드
    public override void OnDeserialization()
    {
        // 네트워크 상 동기화된 활성화 상태로 거울의 활성화 상태를 업데이트
        mirror.SetActive(isActive);
    }
}


