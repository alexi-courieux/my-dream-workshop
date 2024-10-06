using System;
using AshLight.BakerySim.ScriptableObjects;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IInteractableAlt, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    [SerializeField] private GameObject fullPrefab;
    [SerializeField] private GameObject depletedPrefab;
    [SerializeField] private ResourceNodeSo resourceNodeSo;
    
    private enum State
    {
        Full,
        Depleted
    }
    private State state;
    private int resourceLeft;
    private int interactionsLeft;
    
    private float regenTimer;

    private void Start()
    {
        Fill();
    }

    private void Update()
    {
        if (state is not State.Depleted) return;
        regenTimer -= Time.deltaTime;
        if (regenTimer <= 0)
        {
            Fill();
        }
    }

    public void InteractAlt()
    {
        if (state is State.Depleted) return;
        if(Player.Instance.HandleSystem.HaveAnyItems()) return;
        
        interactionsLeft--;
        if (interactionsLeft <= 0)
        {
            Harvest();
        }
        
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 1 - (float)interactionsLeft / resourceNodeSo.interactionCountToHarvest
        });
    }
    
    private void Fill()
    {
        state = State.Full;
        fullPrefab.SetActive(true);
        depletedPrefab.SetActive(false);
        resourceLeft = resourceNodeSo.maxResource;
        interactionsLeft = resourceNodeSo.interactionCountToHarvest;
        RandomiseVisual();
    }
    
    private void Deplete()
    {
        state = State.Depleted;
        fullPrefab.SetActive(false);
        depletedPrefab.SetActive(true);
        regenTimer = resourceNodeSo.timeToRegen;
    }
    
    private void RandomiseVisual()
    {
        float randomScale = UnityEngine.Random.Range(0.6f, 1.1f);
        fullPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        depletedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        
        float randomRotation = UnityEngine.Random.Range(0, 360);
        fullPrefab.transform.rotation = Quaternion.Euler(0, randomRotation, 0);
        depletedPrefab.transform.rotation = Quaternion.Euler(0, randomRotation, 0);
    }

    private void Harvest()
    {
        Item.SpawnItem<Product>(resourceNodeSo.product.prefab, Player.Instance.HandleSystem);
        resourceLeft--;
        if (resourceLeft <= 0)
        {
            Deplete();
        }
        interactionsLeft = resourceNodeSo.interactionCountToHarvest;
    }
}
