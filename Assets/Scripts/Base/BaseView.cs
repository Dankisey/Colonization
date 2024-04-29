using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class BaseView : MonoBehaviour
{
    [SerializeField] private Base _base;

    private TMP_Text _label;

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _base.ResourcesAmountChanged += OnResourcesAmountChanged;
    }

    private void OnResourcesAmountChanged(int amount)
    {
        _label.text = $"Resources: {amount}";
    }

    private void OnDisable()
    {
        _base.ResourcesAmountChanged -= OnResourcesAmountChanged;
    }
}