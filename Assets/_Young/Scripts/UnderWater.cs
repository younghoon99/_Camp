using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UnderWater : UdonSharpBehaviour
{
    public AudioSource audioSource;
    private bool isPlayerInWater = false;

    [Header("포스트 프로세싱 설정")]
    public GameObject normalPostProcessing; // 일반 환경용 포스트 프로세싱 게임오브젝트
    public GameObject underwaterPostProcessing; // 물속 환경용 포스트 프로세싱 게임오브젝트

    void Start()
    {
        // 포스트 프로세싱 초기 설정
        if (normalPostProcessing != null)
        {
            normalPostProcessing.SetActive(true);
        }

        if (underwaterPostProcessing != null)
        {
            underwaterPostProcessing.SetActive(false);
        }
    }

    // 포스트 프로세싱 효과 업데이트 - 즉시 전환
    private void UpdatePostProcessing()
    {
        if (normalPostProcessing != null && underwaterPostProcessing != null)
        {
            // 물속에 있을 때는 물속 포스트 프로세싱을 활성화
            if (isPlayerInWater)
            {
                normalPostProcessing.SetActive(false);
                underwaterPostProcessing.SetActive(true);
            }
            // 물 밖에 있을 때는 일반 포스트 프로세싱을 활성화
            else
            {
                normalPostProcessing.SetActive(true);
                underwaterPostProcessing.SetActive(false);
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer)
        {
            isPlayerInWater = true;
            // 플레이어가 물에 들어갔을 때 소리 재생 시작
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            // 즉시 포스트 프로세싱 업데이트
            UpdatePostProcessing();
            Debug.Log("플레이어가 물에 들어갔습니다.");
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer)
        {
            isPlayerInWater = false;
            // 플레이어가 물에서 나갔을 때 소리 정지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // 즉시 포스트 프로세싱 업데이트
            UpdatePostProcessing();
            Debug.Log("플레이어가 물에서 나갔습니다.");
        }
    }
}