using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float maxLife;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;
    [SerializeField] private Color DamageColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = spriteRenderer.color;

        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            print("Mort");
        }
    }

    public float GetLife()
    {
        return life;
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        StartCoroutine(TakeDamageColor());
    }

    public IEnumerator TakeDamageColor()
    {
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = DamageColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = DamageColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originalColor;
    }
}
