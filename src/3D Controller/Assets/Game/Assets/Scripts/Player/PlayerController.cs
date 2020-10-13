using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera fov;
	public Rigidbody rb;

	[Header("Ground Check"), Space(5f)]
	public Transform GroundCheck;
	public float GroundCheckRadius;
	public LayerMask GroundLayer;

	public float Speed;

	public float RotationDamping;

	private bool CanJump { get; set; }

	public float Sensitivity;

	public float maxYAngle = 80f;

	private Vector2 currentRotation;

	private void Awake()
	{
		TryGetComponent(out rb);
		fov = GetComponentInChildren<Camera>();
	}

	private void OnEnable()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");

		currentRotation.x -= Input.GetAxisRaw("Mouse Y") * Sensitivity;
		currentRotation.y += Input.GetAxisRaw("Mouse X") * Sensitivity;

		var n = fov.transform.rotation.normalized;
		rb.MovePosition(rb.position + (n * new Vector3(x, 0, z) * Speed * Time.deltaTime));

		if (CanJump && Input.GetButtonUp("Jump"))
			rb.velocity = new Vector3(0, 20, 0);
	}
	private void FixedUpdate()
	{
		currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);

		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, currentRotation.y, 0), RotationDamping * Time.deltaTime);
		fov.transform.rotation = Quaternion.Lerp(fov.transform.rotation, Quaternion.Euler(currentRotation.x, transform.rotation.eulerAngles.y, 0), RotationDamping * Time.deltaTime);

		CanJump = Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayer);
	}

#if UNITY_EDITOR

	private void OnDrawGizmos()
	{
		if (GroundCheck == null) return;

		Gizmos.color = CanJump ? Color.white : Color.red;

		Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
	}

#endif
}