using UnityEngine;

public class SyncPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}