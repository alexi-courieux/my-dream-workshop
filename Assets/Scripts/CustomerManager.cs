using System;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }
    private const int CustomerLimit = 3;

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform dispawnPoint;
    [SerializeField] private float spawnIntervalMin = 5f;
    [SerializeField] private float spawnIntervalMax = 10f;
    [SerializeField] private CheckoutStation checkoutStation;

    private Customer[] _customers;
    private float _spawnTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _customers = new Customer[CustomerLimit];
        for (int i = 0; i < CustomerLimit; i++)
        {
            GameObject customerObject = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            _customers[i] = customerObject.GetComponent<Customer>();
            customerObject.SetActive(false);
        }
        _spawnTimer = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
            SpawnCustomer();
        }

        // Test purpose
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
    {
        for (int i = 0; i < CustomerLimit; i++)
        {
            if (_customers[i].gameObject.activeInHierarchy) continue;

            _customers[i].transform.position = spawnPoint.position;
            _customers[i].gameObject.SetActive(true);
            _customers[i].Initialize(checkoutStation);
            break;
        }
    }

    public Transform GetDispawnPoint()
    {
        return dispawnPoint;
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public static void Dispawn(Customer customer)
    {
        customer.gameObject.SetActive(false);
    }
}