using UnityEngine;
using System.Collections;

public class FireTraps : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay = 0.5f;
    [SerializeField] private float activeTime = 2f;

    [Header("Damage Settings")]
    [SerializeField] private float damageCooldown = 1f;

    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;
    private bool playerInside;

    private float damageTimer;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (active && playerInside)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0)
            {
                playerHealth.TakeDamage(damage);
                damageTimer = damageCooldown;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            playerHealth = collision.GetComponent<Health>();

            if (!triggered)
                StartCoroutine(ActivateFiretrap());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;

        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);

        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
