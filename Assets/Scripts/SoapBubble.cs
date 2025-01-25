using UnityEngine;

public class SoapBubble : MonoBehaviour
{
    [SerializeField] private Transform m_targetPosition;
    private float moveSpeed = 2f;

    protected void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition.position, moveSpeed * Time.deltaTime);

        if (transform.position == m_targetPosition.position)
        {
            Destroy(gameObject);
        }
    }
}


