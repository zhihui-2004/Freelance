using UnityEngine;

public class VRUI : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float uiDistance = 2f;
    [SerializeField] private Vector3 uiOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    
    [SerializeField] private bool isCurrentlyOpen = false;
    [SerializeField] private bool isLookingAtPlayer;
    [SerializeField] private bool updatePositionOnEnable = true;

    private void OnEnable()
    {
        isCurrentlyOpen = true;
        
        if (updatePositionOnEnable)
        {
            if (isLookingAtPlayer)
            {
                LookAtPlayer();
            }
            else
            {
                UpdatePosition();
            }
        }
        else
        {
            if (isLookingAtPlayer)
            {
                LookAtPlayer();
            }
        }
    }

    private void OnDisable()
    {
        isCurrentlyOpen = false;
    }

    private void LateUpdate()
    {
        if (isCurrentlyOpen)
        {
            if (isCurrentlyOpen && isLookingAtPlayer)
            {
                LookAtPlayer();
            }
        }
    }

    public void OpenUI()
    {
        if (!isCurrentlyOpen)
        {
            gameObject.SetActive(true);
            isCurrentlyOpen = true;
            UpdatePosition();
        }
    }

    public void OpenPartUI()
    {
        if (!isCurrentlyOpen)
        {
            gameObject.SetActive(true);
            isCurrentlyOpen = true;
        }
    }

    public void CloseUI()
    {
        if (isCurrentlyOpen)
        {
            isCurrentlyOpen = false;
            gameObject.SetActive(false);
        }
    }

    private void UpdatePosition()
    {
        Vector3 uiPosition = playerTransform.position + playerTransform.forward * uiDistance + uiOffset;
        transform.position = uiPosition;

        Vector3 lookPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(lookPosition, Vector3.up);

        transform.Rotate(0, 180, 0);
        transform.Rotate(rotationOffset);
    }

    private void LookAtPlayer()
    {
        Vector3 lookPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(lookPosition, Vector3.up);
        transform.Rotate(0, 180, 0);
        transform.Rotate(rotationOffset);
    }
}