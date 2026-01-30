using UnityEngine;

public class FloatingWeapon : MonoBehaviour
{
    public float bobbingUpDown = 0.2f;
    public float bobFrequency = 2f;

    private Vector3 initialLocalPos;

    private void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    private void Update()
    {
        float newY = initialLocalPos.y + Mathf.Sin(Time.time * bobFrequency) * bobbingUpDown;
        transform.localPosition = new Vector3(initialLocalPos.x, newY, initialLocalPos.z);
    }
}
