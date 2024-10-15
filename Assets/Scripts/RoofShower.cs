using UnityEngine;

public class RoofShower : MonoBehaviour
{
    [SerializeField] private GameObject roof; // Assign the roof GameObject in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        roof.gameObject.SetActive(true);
    }
}