using UnityEngine;
public class ActivateOnStart : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate;
    
    private void Start()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }
}