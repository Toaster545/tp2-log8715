using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

public class Circle : MonoBehaviour
{
    [FormerlySerializedAs("I")] [HideInInspector]
    public int i;

    [FormerlySerializedAs("J")] [HideInInspector]
    public int j;

    public float Health { get; private set; }

    private const float BaseHealth = 1000;
    private const float HealingPerSecond = 1;
    private const float HealingRange = 3;

    private SpriteRenderer spriteRenderer;
    private GridShape grid;
    private HashSet<Circle> nearbyCircles = new HashSet<Circle>();

    // Start is called before the first frame update
    private void Start()
    {
        Health = BaseHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = GameObject.FindFirstObjectByType<GridShape>();

        // Cache nearby circles at the start
        CacheNearbyCircles();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColor();
        HealNearbyShapes();
    }

    private void UpdateColor()
    {
        spriteRenderer.color = grid.Colors[i, j] * Health / BaseHealth;
    }

    private void CacheNearbyCircles()
    {
        var nearbyColliders = Physics2D.OverlapCircleAll(transform.position, HealingRange);
        foreach (var nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider != null)
            {
                var circle = nearbyCollider.GetComponent<Circle>();
                if (circle != null)
                {
                    nearbyCircles.Add(circle);
                }
            }
        }
    }

    private void HealNearbyShapes()
    {
        foreach (var circle in nearbyCircles)
        {
            circle.ReceiveHp(HealingPerSecond * Time.deltaTime);
        }
    }

    public void ReceiveHp(float hpReceived)
    {
        Health += hpReceived;
        Health = Mathf.Clamp(Health, 0, BaseHealth);
    }
}
