
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [Header("Keys")]
    [SerializeField] private int totalKeys = 6;
    [SerializeField] private TMPro.TMP_Text keyCounterText;

    private int collectedKeys = 0;

    public int CollectedKeys => collectedKeys;
    [SerializeField] private CollectibleKey[] keys;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void CollectKey()
    {
        if (collectedKeys >= totalKeys)
            return;

        collectedKeys++;
        UpdateUI();

        Debug.Log("Keys collected: " + collectedKeys + "/" + totalKeys);
    }

    public void ResetKeys()
    {
        collectedKeys = 0;
        
        foreach(CollectibleKey key in keys){
            if(key != null){
                key.gameObject.SetActive(true);
                key.ResetKey();
            }
        }
        UpdateUI();

        Debug.Log("Keys reset!");
    }
    void UpdateUI()
    {
        keyCounterText.text = "Keys: " + collectedKeys + " / " + totalKeys;
    }
}
