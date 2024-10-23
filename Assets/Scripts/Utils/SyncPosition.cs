using UnityEngine;

public class SyncPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    private void Update()
    {
        transform.position = target.position;
    }
}