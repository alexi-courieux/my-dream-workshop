using System;
using AshLight.BakerySim.ScriptableObjects;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IInteractableAlt, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnHarvesting;
    
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
    private float timeBeforeNextInteraction;

    private void Start()
    {
        Fill();
    }

    private void Update()
    {
        if (state is State.Depleted)
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                Fill();
            }
        }
        
        if (state is State.Full && timeBeforeNextInteraction > 0)
        {
            timeBeforeNextInteraction -= Time.deltaTime;
            if (timeBeforeNextInteraction <= 0)
            {
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
        }
    }

    public void InteractAlt()
    {
        if (state is State.Depleted) return;
        if (timeBeforeNextInteraction > 0) return;
        if(Player.Instance.HandleSystem.HaveAnyItems()) return;
        timeBeforeNextInteraction = resourceNodeSo.timeBetweenInteractions;
        OnHarvesting?.Invoke(this, EventArgs.Empty);
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
    
    // We can move this logic in a separate class defined by each resource node type (Allow for different customisation, like tree leaves color)
    // ResourceNodeRandomizer ? Listening for ResourceNode.OnFill event ?
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
        Item.SpawnItem(resourceNodeSo.product.prefab, Player.Instance.HandleSystem);
        resourceLeft--;
        if (resourceLeft <= 0)
        {
            Deplete();
        }
        interactionsLeft = resourceNodeSo.interactionCountToHarvest;
    }
}
