using System;
using Unity.VisualScripting;
using UnityEngine;

public class RoofHider : MonoBehaviour
{
    [SerializeField] private GameObject roof; // Assign the roof GameObject in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        roof.gameObject.SetActive(false);
    }
}