using UnityEngine;

public class LoadingIcon : MonoBehaviour
{
    private const float speed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * speed);
    }
}
