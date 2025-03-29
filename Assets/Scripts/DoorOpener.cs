using UnityEngine;

public class DoorOpener : MonoBehaviour
{
       [SerializeField] private Rigidbody _leftDoorRigidbody;
    [SerializeField] private Rigidbody _rightDoorRigidbody;
    [SerializeField] private float _openDistance = 2f; 
    [SerializeField] private float _moveSpeed = 3f;     
    [SerializeField] private bool _isOpen = false;

    private Vector3 _leftDoorStartPos;
    private Vector3 _rightDoorStartPos;

    private void Start()
    {
        _leftDoorStartPos = _leftDoorRigidbody.position;
        _rightDoorStartPos = _rightDoorRigidbody.position;
    }

    private void FixedUpdate()
    {
        if (_isOpen)
        {
            MoveDoorsToOpen();
        }
        else
        {
            MoveDoorsToClose();
        }
    }
    
    private void MoveDoorsToOpen()
    {
        Vector3 leftTargetPos = _leftDoorStartPos - new Vector3(_openDistance, 0, 0);
        Vector3 rightTargetPos = _rightDoorStartPos + new Vector3(_openDistance, 0, 0);

        
        _leftDoorRigidbody.MovePosition(Vector3.MoveTowards(_leftDoorRigidbody.position, leftTargetPos, _moveSpeed * Time.fixedDeltaTime));
        _rightDoorRigidbody.MovePosition(Vector3.MoveTowards(_rightDoorRigidbody.position, rightTargetPos, _moveSpeed * Time.fixedDeltaTime));
    }
    
    private void MoveDoorsToClose()
    {
        _leftDoorRigidbody.MovePosition(Vector3.MoveTowards(_leftDoorRigidbody.position, _leftDoorStartPos, _moveSpeed * Time.fixedDeltaTime));
        _rightDoorRigidbody.MovePosition(Vector3.MoveTowards(_rightDoorRigidbody.position, _rightDoorStartPos, _moveSpeed * Time.fixedDeltaTime));
    }
    
    public void ToggleDoors()
    {
        _isOpen = !_isOpen;
    }
}
