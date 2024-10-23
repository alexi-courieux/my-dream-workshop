using UnityEngine;
public class ActivateOnStart : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate;
    
    private void Awake()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }
}