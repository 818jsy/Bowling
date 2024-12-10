using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public string targetTag = "Target"; // 충돌 대상 태그 (예: "Pin")

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 지정된 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(gameObject); // 볼링공 삭제
        }
    }
}