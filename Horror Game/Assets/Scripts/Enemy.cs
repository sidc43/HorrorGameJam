using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float range;
    public float damage;
    public float speed;
    public Transform orientation;
    public NavMeshAgent agent;
    [SerializeField] private BoxCollider rangeCollider;
    private bool targetting;

    public void Start()
    {
        health = maxHealth;
        agent.speed = speed;
        rangeCollider.size = new Vector3(range, 1, range);
        targetting = false;
    }

    public void Update()
    {
        transform.rotation = orientation.rotation;
        if (targetting)
        {
            agent.SetDestination(FindObjectsByType<PlayerController>(FindObjectsSortMode.None)[0].gameObject.transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            targetting = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
