using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering;

public class EntityScript : MonoBehaviour
{

    public int MaxHP;
    public int ATK;
    public int MSPD;
    public int ATKSPD;
    public int Cost;

    public string targetTag;

    private Transform currentTarget;
    private GameObject targetEntity;

    void Start()
    {
        FindClosestTarget();
    }

    void Update()
    {
        MoveTowardTarget();
        if (targetEntity != null)
        {
            FindClosestTarget();
        }
    }

    void FindClosestTarget()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag(targetTag);

        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Vector2 myPosition = transform.position;
        GameObject target = null;

        foreach (GameObject entity in entities)
        {
            if(entity == gameObject) continue;

            float distance = Vector2.Distance(myPosition, entity.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closest = entity.transform;
                target = entity;
            }
        }
        currentTarget = closest;
        targetEntity = target;
    }
    
    void MoveTowardTarget()
    {
        Vector2 direction = ((Vector2)currentTarget.position - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * MSPD * Time.deltaTime);
    }

}
