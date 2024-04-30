using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResourceCountView : MonoBehaviour
{
    [SerializeField] private ResourceStorage _storage;

    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _storage.ResourcesAmountChanged += OnResourcesAmountChanged;
    }

    private void OnDisable()
    {
        _storage.ResourcesAmountChanged -= OnResourcesAmountChanged;
    }

    private void OnResourcesAmountChanged(int amount)
    {
        _label.text = $"Resources: {amount}";
    }
}