using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInventory
{
    // Start is called before the first frame update
    public Light _flashlight;
    public Light _unlight;
    public Camera _cam;

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;
    public float maxFallingSpeed = -10.0f;

    public float interactDistance = 20.0f;

    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;

    CharacterController controller;

    public float mouseSensitivity = 10;
    public Vector2 pitchMinmax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;

    bool jumping;
    bool disabled;
    float lastGroundTime;

    private ICollectable toCollect = null;
    private IInteractable toInteract = null;
    
    public List<IItemStack> Inventory { get; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _flashlight.enabled = true;
        _unlight.enabled = false;
        _unlight.color = new Color(-1.0f, -1.0f, -1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        if(toCollect != null)
            Debug.Log("Press Enter To Collect");
        if(toInteract != null)
            Debug.Log("Press Enter To Interact");
        
        if (toCollect != null && (Input.GetButtonDown("Select")))
        {
            toCollect.Collect(this);
        }
        else if(toInteract != null && (Input.GetButtonDown("Select")))
        {
            toInteract.Interact(this);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white);
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            CheckInteraction(hit);
        }
    }

    void CheckInteraction(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Collectable"))
        {
            toCollect = hit.collider.GetComponent<ICollectable>();
            toCollect.Highlight();
        }
        else
        {
            toCollect?.Unhighlight();
            toCollect = null;
        }

        if (hit.collider.CompareTag("Interactable"))
        {
            toInteract = hit.collider.GetComponent<IInteractable>();
            toInteract.Highlight();
        }
        else
        {
            toInteract?.Unhighlight();
            toInteract = null;
        }
    }

    void MoveCharacter()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);
        
        float currentSpeed = (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < maxFallingSpeed) verticalVelocity = maxFallingSpeed;

        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below)
        {
            jumping = false;
            lastGroundTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            float timeSinceLastTouchedGround = Time.time - lastGroundTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f))
            {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        float mX = Input.GetAxisRaw("Mouse X");
        float mY = Input.GetAxisRaw("Mouse Y");

        float mMag = Mathf.Sqrt(mX * mX + mY * mY);
        if (mMag > 5)
        {
            mX = 0;
            mY = 0;
        }

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, pitchMinmax.x, pitchMinmax.y);

        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * smoothYaw;
        _cam.transform.localEulerAngles = Vector3.right * smoothPitch;
    }
    public void AddItem(IItemStack item)
    {
        IItemStack inventoryObj = Inventory.Find(x => x.ItemID == item.ItemID);
        if (inventoryObj != null)
            inventoryObj.Amount += item.ItemID;
        else
            Inventory.Add(item);
    }

    public int ItemsCount()
    {
        return Inventory.Count;
    }

    public IItemStack GetItem(int index)
    {
        return Inventory[index];
    }

    public void RemoveItem(int index)
    {
        Inventory.RemoveAt(index);
    }

    public void DecreaseAmount(int index, uint amount)
    {
        Inventory[index].Amount -= amount;
        if (Inventory[index].Amount <= 0)
        {
            Inventory.RemoveAt(index);
        }
    }
}
