using UdonSharp; // UdonSharp 라이브러리를 사용하여 스크립트를 작성
using UnityEngine; // Unity 엔진의 기본 기능을 사용
using VRC.SDKBase; // VRC SDK의 기본 네트워크 기능 사용
using VRC.Udon; // VRC Udon 관련 기능 사용

// UdonBehaviourSyncMode.Manual: 동기화가 필요할 때만 수동으로 동기화
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PPTScript : UdonSharpBehaviour
{
    // 페이지를 관리하는 GameObject 배열
    public GameObject[] pages;

    // 동기화가 필요한 변수(index)
    // FieldChangeCallback(nameof(Index)): Index 값이 변경될 때 호출되는 콜백 지정
    [UdonSynced(), FieldChangeCallback(nameof(Index))] public int index;

    // Index 프로퍼티: index 값의 get/set 동작 정의
    public int Index
    {
        get => index; // index 값을 반환
        set
        {
            index = value; // index 값 설정
            Debug.Log("Index changed to " + value); // 디버그 로그로 변경된 index 값 출력
            OnIndexChanged(value); // Index 값이 변경되었을 때 호출되는 메서드
        }
    }

    // Index 값이 변경되었을 때 호출되는 메서드
    public void OnIndexChanged(int p)
    {
          Debug.Log("OnIndexChanged called with value: " + p); // Index 값 변경 시 호출되었는지 확인
        // pages 배열의 모든 페이지에 대해 활성화 여부 설정
        for (int i = 0; i < pages.Length; i++)
        {
            if (p == i)
            {
                // 현재 Index와 일치하는 페이지 활성화
                pages[i].SetActive(true);
                 Debug.Log("Activating page " + i); // 페이지 활성화 로그 출력
            }
            else
            {
                // 나머지 페이지 비활성화
                pages[i].SetActive(false);
                 Debug.Log("Deactivating page " + i); // 페이지 비활성화 로그 출력
            }
        }
    }

    // 다음 페이지로 이동
    public void NextPage()
    {
        Debug.Log("NextPage"); // 디버그 로그: 다음 페이지 요청
        if (index + 1 > pages.Length - 1)
        {
            // index 값이 최대 페이지 수를 초과하면 아무 동작도 하지 않음
            return;
        }
        else
        {
            Debug.Log("Can go to Next Page"); // 다음 페이지로 이동 가능
            Networking.SetOwner(Networking.LocalPlayer, gameObject); // 로컬 플레이어를 오브젝트의 소유자로 설정
            Index += 1; // index 값을 1 증가
            RequestSerialization(); // 변경된 index 값을 네트워크에 동기화
        }
    }

    // 이전 페이지로 이동
    public void PrevPage()
    {
        Debug.Log("LastPage"); // 디버그 로그: 이전 페이지 요청
        if (index - 1 < 0)
        {
            // index 값이 0보다 작아지면 아무 동작도 하지 않음
            return;
        }
        else
        {
            Debug.Log("Can go to Last Page"); // 이전 페이지로 이동 가능
            Networking.SetOwner(Networking.LocalPlayer, gameObject); // 로컬 플레이어를 오브젝트의 소유자로 설정
            Index -= 1; // index 값을 1 감소
            RequestSerialization(); // 변경된 index 값을 네트워크에 동기화
        }
    }
}
