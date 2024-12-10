using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 좌우 이동 속도
    public GameObject bowlingBallPrefab; // 볼링공 프리팹
    public Transform ballSpawnPoint; // 볼링공 생성 위치
    public float ballLaunchForce = 10f; // 볼링공 던지는 힘
    public float minZ = -1f; // 상대 최소 Z축 제한
    public float maxZ = 1f;  // 상대 최대 Z축 제한
    public float rotationSpeed = 45f; // 좌우 각도 조절 속도
    public float minAngle = -45f; // 좌우 각도 제한 (왼쪽)
    public float maxAngle = 45f; // 좌우 각도 제한 (오른쪽)

    private Vector3 startPosition; // 처음 위치 저장
    private bool hasBall = true;   // 볼링공을 던질 수 있는지 확인
    private float currentAngle = 0f; // 현재 각도

    void Start()
    {
        // 처음 위치 저장
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // 상대 Z축 이동 처리
        float horizontal = Input.GetAxis("Horizontal"); // A/D 또는 좌/우 화살표 키
        Vector3 newLocalPosition = transform.localPosition + Vector3.forward * horizontal * moveSpeed * Time.deltaTime;

        // 상대 Z축 이동 제한 적용
        newLocalPosition.z = Mathf.Clamp(newLocalPosition.z, startPosition.z + minZ, startPosition.z + maxZ);
        transform.localPosition = newLocalPosition;

        // 좌우 각도 조절
        if (Input.GetKey(KeyCode.Q))
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            currentAngle += rotationSpeed * Time.deltaTime;
        }

        // 각도 제한 적용
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // 새로운 각도 적용
        transform.localRotation = Quaternion.Euler(0, currentAngle, 0);

        // 스페이스바로 볼링공 던지기
        if (Input.GetKeyDown(KeyCode.Space) && hasBall)
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        if (bowlingBallPrefab == null || ballSpawnPoint == null)
        {
            Debug.LogWarning("볼링공 프리팹 또는 생성 위치가 설정되지 않았습니다.");
            return;
        }

        // 볼링공 생성
        GameObject bowlingBall = Instantiate(bowlingBallPrefab, ballSpawnPoint.position, transform.rotation);

        // Rigidbody를 통해 힘을 가해 볼링공 던지기
        Rigidbody ballRigidbody = bowlingBall.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = -transform.right * ballLaunchForce; // 현재 각도에 따라 볼링공 던짐
        }

        hasBall = false; // 다시 던질 수 없게 설정
        StartCoroutine(ResetBallAfterDelay(4f)); // 4초 후 다시 던질 수 있게 설정
    }

    IEnumerator ResetBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasBall = true; // 4초 후 다시 던질 수 있게 설정
        Debug.Log("You can throw the ball again.");
    }
}