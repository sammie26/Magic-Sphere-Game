using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldLogic : MonoBehaviour
{
    private List<GameObject> activeShieldComponents = new List<GameObject>();
    private List<GameObject> deactivatedShieldComponents = new List<GameObject>();

    [Header("Colors")]
    [SerializeField] public Color defaultColor = new Color(17, 0, 204);
    [SerializeField] public Color damageTakenColor = Color.red;


    [SerializeField] private float immunityDuration = 5.0f;
    private float timeSinceLastHit = float.MaxValue; // so no need to wait for first hit.
    private bool damageChangedColor; //color was changed due to damage.
    public bool isAlive => activeShieldComponents.Count > 0;
    public bool IsFullShields => deactivatedShieldComponents.Count == 0;
    private bool isImmune => timeSinceLastHit < immunityDuration;

    void Start()
    {
        InitializeShieldComponentLists();
        // https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
        // ColorUtility.TryParseHtmlString("#1100CC", out defaultColor);
        SetActiveShieldColor(defaultColor);
        damageChangedColor = false;
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        // Change Color Back to Default
        if (!isImmune && damageChangedColor)
        {
            damageChangedColor = false;
            SetActiveShieldColor(defaultColor);
        }
    }


    private void InitializeShieldComponentLists()
    {
        foreach (Transform child in transform)
        {
            //Make sure to only add shield parts.
            if (child.CompareTag("ShieldComponent"))
            {
                if (child.gameObject.activeSelf)
                {
                    activeShieldComponents.Add(child.gameObject);
                }
                else deactivatedShieldComponents.Add(child.gameObject);
            }
        }
    }
    private GameObject MoveRandomShieldPart(List<GameObject> fromList, List<GameObject> toList)
    {
        int randomSelection = UnityEngine.Random.Range(0, fromList.Count);
        GameObject shieldComponent = fromList[randomSelection];
        fromList.Remove(shieldComponent);
        toList.Add(shieldComponent);
        return shieldComponent;
    }

    private void RemoveRandomShieldComponent()
    {
        GameObject shieldComponent = MoveRandomShieldPart(activeShieldComponents, deactivatedShieldComponents);
        shieldComponent.SetActive(false);
        Debug.Log($"Removed shield part: {shieldComponent.name}");
    }
    private void AddRandomShieldComponent()
    {
        GameObject shieldComponent = MoveRandomShieldPart(deactivatedShieldComponents, activeShieldComponents);
        shieldComponent.SetActive(true);
        Debug.Log($"Added shield part: {shieldComponent.name}");
    }

    public void SetActiveShieldColor(Color toColor)
    {
        List<GameObject> allShieldComponents = new List<GameObject>(activeShieldComponents);
        allShieldComponents.AddRange(deactivatedShieldComponents);

        foreach (GameObject child in allShieldComponents)
        {
            Renderer renderer = child.gameObject.GetComponent<Renderer>();
            renderer.material.color = toColor;
        }

    }

    public void TakeDamage(int numberOfDamageToInflict = 1)
    {
        if (isImmune) return; // Immune State

        SetActiveShieldColor(damageTakenColor);
        damageChangedColor = true;
        numberOfDamageToInflict = Math.Min(numberOfDamageToInflict, activeShieldComponents.Count);
        for (int i = 0; i < numberOfDamageToInflict; i++) RemoveRandomShieldComponent();
        timeSinceLastHit = 0f;
    }

    public void RegenerateShield()
    {
        if (IsFullShields)
        {
            Debug.Log($"Can't Add Shield: Shields at Full Capacity");
            return;
        }
        AddRandomShieldComponent();
    }

    public float ShieldPercentage
    {
        get
        {
            // avoid integer division.
            float totalPieces = activeShieldComponents.Count + deactivatedShieldComponents.Count;
            return activeShieldComponents.Count / totalPieces * 100f;
        }
    }
}
