using UnityEngine;

public class PinController : MonoBehaviour
{
    public GameObject[] pins; // 핀 오브젝트 배열 (인스펙터에서 직접 추가)
    private Vector3[] initialPositions; // 초기 위치 저장
    private Quaternion[] initialRotations; // 초기 회전 저장

    void Start()
    {
        SavePinPositions(); // 핀의 초기 위치와 회전 저장
    }

    public void SavePinPositions()
    {
        initialPositions = new Vector3[pins.Length];
        initialRotations = new Quaternion[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i] != null)
            {
                initialPositions[i] = pins[i].transform.position;
                initialRotations[i] = pins[i].transform.rotation;
            }
        }
    }

    public void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            if (pins[i] != null)
            {
                pins[i].transform.position = initialPositions[i];
                pins[i].transform.rotation = initialRotations[i];

                Rigidbody rb = pins[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero; // 속도 초기화
                    rb.angularVelocity = Vector3.zero; // 회전 속도 초기화
                }
            }
        }
    }

    public int CountStandingPins()
    {
        int standingPins = 0;

        foreach (GameObject pin in pins)
        {
            if (pin != null && pin.transform.up.y > 0.9f) // 핀이 서 있는지 확인
            {
                standingPins++;
            }
        }

        return standingPins;
    }
}