using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FencedArea : MonoBehaviour
{
    public bool full
    {
        get
        {
            return currentCount >= requiredCount;
        }
    }

    [SerializeField] private int requiredCount;
    [SerializeField] private int currentCount;

    [SerializeField] TextMeshPro requiredText, currentText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCount();
        requiredText.text = "";
        for (int i = 0; i < requiredCount; i++)
        {
            requiredText.text += ".";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Boid>())
        {
            currentCount++;
            UpdateCount();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Boid>())
        {
            currentCount--;
            UpdateCount();
        }
    }

    private void UpdateCount()
    {
        currentText.text = "";
        for (int i = 0; i < Mathf.Min(currentCount, requiredCount); i++)
        {
            currentText.text += ".";
        }
    }
}