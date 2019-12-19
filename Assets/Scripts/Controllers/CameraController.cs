using UnityEngine;

/// <summary>
/// Camera controller for camera movement and look around.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Normal speed of camera movement.
    /// </summary>
    public float movementSpeed = 10f;

    /// <summary>
    /// Speed of camera movement when shift is held down,
    /// </summary>
    public float fastMovementSpeed = 100f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    /// </summary>
    public float fastZoomSensitivity = 50f;

    /// <summary>
    /// Set to true when free looking (on right mouse button).
    /// </summary>
    private bool looking = false;

    public float XLimitPositive = 10f;
    public float XLimitNegative = -10f;
    public float YLimitPositive = 50f;
    public float YLimitNegative = 1f;
    public float ZLimitPositive = 10f;
    public float ZLimitNegative = -10f;

    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }
    
    void Update()
    {
        var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition = newPosition + (-transform.right * movementSpeed * Time.deltaTime);
            SetPositionWhenEligable(newPosition);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition = newPosition + (transform.right * movementSpeed * Time.deltaTime);
            SetPositionWhenEligable(newPosition);
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPosition = newPosition + (transform.up * movementSpeed * Time.deltaTime);
            newPosition = newPosition + (transform.forward * movementSpeed * Time.deltaTime);
            SetPositionWhenEligable(newPosition);
        }

        if (Input.GetKey(KeyCode.S))
        {
            newPosition = newPosition + (-transform.up * movementSpeed * Time.deltaTime);
            newPosition = newPosition + (-transform.forward * movementSpeed * Time.deltaTime);
            SetPositionWhenEligable(newPosition);
        }
        
        
        if (looking)
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? this.fastZoomSensitivity : this.zoomSensitivity;
            newPosition = newPosition + transform.forward * axis * zoomSensitivity;
            SetPositionWhenEligable(newPosition);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
    }

    private void SetPositionWhenEligable(Vector3 newPosition)
    {
        if (CheckEligablePositon(newPosition))
        {
            transform.position = newPosition;
        }
    }

    private bool CheckEligablePositon(Vector3 newPosition)
    {
        if (newPosition.x < XLimitNegative || newPosition.x > XLimitPositive)
        {
            return false;
        }
        
        if (newPosition.y < YLimitNegative || newPosition.y > YLimitPositive)
        {
            return false;
        }
        
        if (newPosition.z < ZLimitNegative || newPosition.z > ZLimitPositive)
        {
            return false;
        }

        return true;

    }

    public void ResetToInitialPosition()
    {
        transform.position = initialPosition;
    }

    void OnDisable()
    {
        StopLooking();
    }

    /// <summary>
    /// Enable free looking.
    /// </summary>
    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}