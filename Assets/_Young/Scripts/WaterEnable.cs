using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WaterEnable : UdonSharpBehaviour
{
    public AudioSource water;

    // 사운드 재생 범위 설정
    [Header("사운드 재생 범위 설정")]
    [Range(1f, 50f)]
    public float minDistance = 5f;
    [Range(5f, 100f)]
    public float maxDistance = 20f;

    // 디버그 시각화 설정
    [Header("디버그 시각화 설정")]
    public bool showDebugSphere = true;
    public Color debugSphereColorMin = Color.green;
    public Color debugSphereColorMax = Color.red;

    private bool isPlaying = false;

    void Start()
    {
        // 시작 시 오디오 소스 설정 적용
        if (water != null)
        {
            water.spatialBlend = 1f; // 3D 사운드로 설정
            water.minDistance = minDistance;
            water.maxDistance = maxDistance;
            water.loop = true; // 루프 재생 설정
        }
    }

    public void Update()
    {
        // 디버그 시각화
        if (showDebugSphere && water != null)
        {
            // 최소 거리 시각화 (녹색)
            Debug.DrawRay(water.transform.position, Vector3.forward * minDistance, debugSphereColorMin);
            Debug.DrawRay(water.transform.position, Vector3.back * minDistance, debugSphereColorMin);
            Debug.DrawRay(water.transform.position, Vector3.left * minDistance, debugSphereColorMin);
            Debug.DrawRay(water.transform.position, Vector3.right * minDistance, debugSphereColorMin);

            // 최대 거리 시각화 (빨간색)
            Debug.DrawRay(water.transform.position, Vector3.forward * maxDistance, debugSphereColorMax);
            Debug.DrawRay(water.transform.position, Vector3.back * maxDistance, debugSphereColorMax);
            Debug.DrawRay(water.transform.position, Vector3.left * maxDistance, debugSphereColorMax);
            Debug.DrawRay(water.transform.position, Vector3.right * maxDistance, debugSphereColorMax);
        }

        // 로컬 플레이어와 오디오 소스 간의 거리 계산
        if (Networking.LocalPlayer != null && water != null)
        {
            float distance = Vector3.Distance(Networking.LocalPlayer.GetPosition(), water.transform.position);

            // 거리에 따른 사운드 재생 제어
            if (distance <= maxDistance && !isPlaying)
            {
                PlayWaterSound();
            }
            else if (distance > maxDistance && isPlaying)
            {
                StopWaterSound();
            }
        }
    }

    public void PlayWaterSound()
    {
        if (water != null && !water.isPlaying)
        {
            water.Play();
            isPlaying = true;
            Debug.Log("물 소리 재생 시작: 플레이어가 재생 범위 내에 들어왔습니다.");
        }
    }

    public void StopWaterSound()
    {
        if (water != null && water.isPlaying)
        {
            water.Stop();
            isPlaying = false;
            Debug.Log("물 소리 재생 중지: 플레이어가 재생 범위를 벗어났습니다.");
        }
    }

    // 에디터에서 시각적으로 범위 표시 (Scene 뷰에서만 보임)
    void OnDrawGizmos()
    {
        if (water != null && showDebugSphere)
        {
            // 최소 거리 기즈모 (녹색)
            Gizmos.color = debugSphereColorMin;
            Gizmos.DrawWireSphere(water.transform.position, minDistance);

            // 최대 거리 기즈모 (빨간색)
            Gizmos.color = debugSphereColorMax;
            Gizmos.DrawWireSphere(water.transform.position, maxDistance);
        }
    }
}
