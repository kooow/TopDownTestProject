using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Transform firingPoint;

    [SerializeField]
    private Rigidbody bullet;

    [SerializeField]
    private float shootSpeed;

    [SerializeField]
    private float weaponLoadingSpeed;

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private float MovementConstant;

    private Rigidbody playerRigidBody;
    private float currentWeaponLoadingValue;

    public static float MAX_WEAPON_LOADING_VALUE = 100.0f;

    [SerializeField]
    private Image WeaponLoadingBar;


    [SerializeField]
    private bool canMove;

    [SerializeField]
    private bool canShoot;

    void Start()
    {
        this.playerRigidBody = this.GetComponent<Rigidbody>();
        this.currentWeaponLoadingValue = MAX_WEAPON_LOADING_VALUE;
        this.canShoot = true;

        Debug.ClearDeveloperConsole();
    }

    void Update()
    {
        if (canMove) this.MoveAndRotate();

        this.SetupWeaponLoading();

        if (canShoot) this.Fire();
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        float speed = MovementConstant * Time.deltaTime;

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;

        Vector3 movementVelocity = (targetPosition - this.transform.position).normalized;

        movementVelocity *= MovementSpeed;

        this.playerRigidBody.velocity = movementVelocity;
        return targetVector;
    }
    private void MoveAndRotate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var inputVector = new Vector2(horizontal, vertical);

        var targetVector = new Vector3(inputVector.x, 0, inputVector.y);

        var movementVector = MoveTowardTarget(targetVector);

        RotateTowardMovementVector(movementVector);

    }

    private void SetupWeaponLoading()
    {

        if (this.currentWeaponLoadingValue < MAX_WEAPON_LOADING_VALUE)
        {
            currentWeaponLoadingValue += this.weaponLoadingSpeed * Time.deltaTime;
        }
        else
        {
            this.canShoot = true;
            currentWeaponLoadingValue = MAX_WEAPON_LOADING_VALUE;
        }

        this.WeaponLoadingBar.fillAmount = currentWeaponLoadingValue / MAX_WEAPON_LOADING_VALUE;
    }

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO: collection with maxBulletNumber
            Vector3 bulletPos = new Vector3(firingPoint.transform.position.x, firingPoint.transform.position.y, firingPoint.transform.position.z);

            Rigidbody bulletObject = Instantiate(bullet, bulletPos, transform.rotation) as Rigidbody;
            bulletObject.mass = 0.1f;


            Vector3 bulletDirection = (this.firingPoint.transform.position - this.transform.position).normalized;
            bulletObject.velocity = bulletDirection * shootSpeed;

            bulletObject.gameObject.AddComponent<BulletDestroyer>();
            bulletObject.gameObject.SetActive(true);

            this.canShoot = false;
            this.currentWeaponLoadingValue = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.wall.ToString()))
        {
            //Debug.Log("player col" + collision.gameObject.name);
            this.playerRigidBody.velocity = Vector3.zero;
        }
    }

    public void StopAndDisableMoveAndShoot()
    {
        this.playerRigidBody.velocity = Vector3.zero;
        this.canMove = false;
        this.canShoot = false;
        currentWeaponLoadingValue = MAX_WEAPON_LOADING_VALUE;
    }

}
