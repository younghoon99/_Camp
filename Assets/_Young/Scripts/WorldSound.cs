using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldSound : UdonSharpBehaviour
{
    [Tooltip("소리를 재생할 오브젝트")]
    public GameObject Sound;

    [Header("소리 범위 설정")]
    [Tooltip("소리가 최대 볼륨으로 들리는 거리")]
    public float minDistance = 1f;

    [Tooltip("소리가 들리는 최대 거리")]
    public float maxDistance = 10f;

    [Tooltip("소리 볼륨")]
    [Range(0f, 1f)]
    public float volume = 1f;

    // 디버그 시각화 설정
    [Header("디버그 시각화 설정")]
    public bool showDebugSphere = true;
    public Color debugSphereColorMin = Color.green;
    public Color debugSphereColorMax = Color.red;

    private AudioSource audioSource;

    void Start()
    {
        // 유효한 값으로 제한
        EnsureValidValues();

        // AudioSource 컴포넌트 가져오기
        if (Sound != null)
        {
            audioSource = Sound.GetComponent<AudioSource>();

            // 소리 설정 적용
            ApplySoundSettings();
        }
    }

    // 업데이트에서 실시간 디버그 가시화
    public void Update()
    {
        if (showDebugSphere && audioSource != null)
        {
            // 최소 거리 시각화 (녹색)
            Debug.DrawRay(audioSource.transform.position, Vector3.forward * minDistance, debugSphereColorMin);
            Debug.DrawRay(audioSource.transform.position, Vector3.back * minDistance, debugSphereColorMin);
            Debug.DrawRay(audioSource.transform.position, Vector3.left * minDistance, debugSphereColorMin);
            Debug.DrawRay(audioSource.transform.position, Vector3.right * minDistance, debugSphereColorMin);

            // 최대 거리 시각화 (빨간색)
            Debug.DrawRay(audioSource.transform.position, Vector3.forward * maxDistance, debugSphereColorMax);
            Debug.DrawRay(audioSource.transform.position, Vector3.back * maxDistance, debugSphereColorMax);
            Debug.DrawRay(audioSource.transform.position, Vector3.left * maxDistance, debugSphereColorMax);
            Debug.DrawRay(audioSource.transform.position, Vector3.right * maxDistance, debugSphereColorMax);
        }
    }

    // 값이 유효한지 확인하고 필요시 조정
    private void EnsureValidValues()
    {
        if (minDistance < 0.1f)
        {
            minDistance = 0.1f;
        }

        if (maxDistance < minDistance)
        {
            maxDistance = minDistance + 0.1f;
        }

        volume = Mathf.Clamp01(volume);
    }

    // 소리 설정 적용하기
    public void ApplySoundSettings()
    {
        if (audioSource != null)
        {
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.volume = volume;
        }
    }

    // 상호작용 시 소리 재생/정지
    public override void Interact()
    {
        if (audioSource == null && Sound != null)
        {
            audioSource = Sound.GetComponent<AudioSource>();
            ApplySoundSettings();
        }

        if (audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                // 소리가 재생 중이면 정지
                audioSource.Stop();
            }
            else
            {
                // 소리가 정지 상태면 재생
                audioSource.Play();
            }
        }
    }

    // 에디터에서 시각적으로 범위 표시 (Scene 뷰에서만 보임)
    void OnDrawGizmos()
    {
        if (Sound != null && showDebugSphere)
        {
            // AudioSource가 없는 경우 가져오기
            if (audioSource == null)
            {
                audioSource = Sound.GetComponent<AudioSource>();
            }

            if (audioSource != null)
            {
                // 최소 거리 기즈모 (녹색)
                Gizmos.color = debugSphereColorMin;
                Gizmos.DrawWireSphere(audioSource.transform.position, minDistance);

                // 최대 거리 기즈모 (빨간색)
                Gizmos.color = debugSphereColorMax;
                Gizmos.DrawWireSphere(audioSource.transform.position, maxDistance);
            }
        }
    }
}
