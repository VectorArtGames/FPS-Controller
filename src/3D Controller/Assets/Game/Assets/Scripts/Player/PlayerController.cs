using UnityEngine;

[RequireComponent(typeof(PlayerShoot))]
public class PlayerController : MonoBehaviour
{
	public Camera fov;
	public Rigidbody rb;
	private PlayerShoot playerShoot;

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
		TryGetComponent(out playerShoot);
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

	private async void Update()
	{
		currentRotation.x -= Input.GetAxisRaw("Mouse Y") * Sensitivity;
		currentRotation.y += Input.GetAxisRaw("Mouse X") * Sensitivity;


		if (CanJump && Input.GetButtonUp("Jump"))
			rb.velocity = new Vector3(0, 20, 0);

		if (Input.GetButtonUp("Fire1"))
			playerShoot.Shoot(2.0f);

	}
	private void FixedUpdate()
	{
		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");
		var n = fov.transform.rotation.normalized;
		var dir = n * new Vector3(x, 0, z) * Speed;
		
		var p = dir;
		p.y = rb.velocity.y;

		rb.velocity = p;

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