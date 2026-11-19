
using UnityEngine;
using TMPro;

public class NumberLock : MonoBehaviour
{
    [Header("Lock Settings")]
    [SerializeField] private string correctCode = "427";
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TMP_Text codeDisplay;
    [SerializeField] private GameObject lockedDoor;

    private string enteredCode = "";
    private bool isOpen = false;

    void Start()
    {
        lockPanel.SetActive(false);
        codeDisplay.text = "";
    }

    public void OpenLock()
    {
        if (isOpen) return;

        lockPanel.SetActive(true);
        enteredCode = "";
        codeDisplay.text = "";
    }

    public void AddNumber(string number)
    {
        if (isOpen || enteredCode.Length >= 3) return;

        enteredCode += number;
        codeDisplay.text = enteredCode;
    }

    public void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            codeDisplay.text = "UNLOCKED!";
            isOpen = true;

            if (lockedDoor != null)
                lockedDoor.SetActive(false);

            Invoke(nameof(CloseLock), 1.5f);
        }
        else
        {
            codeDisplay.text = "INCORRECT";
            enteredCode = "";
        }
    }

    public void ClearCode()
    {
        enteredCode = "";
        codeDisplay.text = "";
    }

    public void CloseLock()
    {
        lockPanel.SetActive(false);
    }
}
