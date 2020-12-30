using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInventory
{
    // Start is called before the first frame update
    public Light _flashlight;
    public Camera _cam;

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;
    public float maxFallingSpeed = -10.0f;

    public float interactDistance = 20.0f;
    public float collectDistance = 20.0f;

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
    
    public List<IItemStack> Inventory { get; private set; }
    public GameObject inventoryObject;
    public InventoryController inventoryController;

    private int choosenLight = -1;
    private int previousLight = -1;

    public GameObject armLantern;
    public GameObject armFlashlight;

    SphereSpawnController ssControler;

    float SphereGrowTime = 0.0f;
    bool hidingLantern = false;
    bool shouldShowFlashlight = false;
    bool controlsBlocked = false;

    void Start()
    {
        Inventory = new List<IItemStack>();
        controller = GetComponent<CharacterController>();
        ssControler = GetComponent<SphereSpawnController>();
        inventoryController = inventoryObject.GetComponent<InventoryController>();
        _flashlight.enabled = false;
        armFlashlight.SetActive(false);
        armLantern.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!controlsBlocked)
        {
            MoveCharacter();
            if (toCollect != null && (Input.GetButtonDown("Interact")))
            {
                toCollect.Collect(this);
            }
            else if (toInteract != null && (Input.GetButtonDown("Interact")))
            {
                toInteract.Interact(this);
            }

            if (Input.GetButtonDown("Lantern"))
            {
                choosenLight = 2;
                ChooseLight();
            }
            if (Input.GetButtonDown("Flashlight"))
            {
                choosenLight = 1;
                ChooseLight();
            }
        }
        /*if (toCollect != null)
            Debug.Log("Press E To Collect");
        if (toInteract != null)
            Debug.Log("Press E To Interact");*/

        if (hidingLantern)
        {
            SphereGrowTime += Time.deltaTime;
            if (SphereGrowTime >= ssControler.growTime)
            {
                armLantern.SetActive(false);
                armFlashlight.SetActive(shouldShowFlashlight);
                _flashlight.enabled = shouldShowFlashlight;
                SphereGrowTime = 0.0f;
                hidingLantern = false;
            }
        }
    }

    private void ChooseLight()
    {
        if(!hidingLantern)
        {
            if (choosenLight == 2)
            {
                if (armFlashlight.activeSelf)
                {
                    armFlashlight.SetActive(false);
                    _flashlight.enabled = false;
                }
                if (armLantern.activeSelf)
                {
                    hidingLantern = true;
                    shouldShowFlashlight = false;
                    ssControler.SpawnBigSphere(false);
                }
                else
                {
                    armLantern.SetActive(true);
                    ssControler.SpawnBigSphere(true);
                }
            }
            else if (choosenLight == 1)
            {
                if (armLantern.activeSelf)
                {
                    hidingLantern = true;
                    shouldShowFlashlight = true;
                    ssControler.SpawnBigSphere(false);
                }
                else
                {
                    armFlashlight.SetActive(!armFlashlight.activeSelf);
                    _flashlight.enabled = armFlashlight.activeSelf;
                }
            }
            else
            {
                if (armLantern.activeSelf)
                {
                    hidingLantern = true;
                    shouldShowFlashlight = false;
                    ssControler.SpawnBigSphere(false);
                }
                if (armFlashlight.activeSelf)
                {
                    armFlashlight.SetActive(false);
                    _flashlight.enabled = false;
                }
            }
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
        else
        {
            toInteract = null;
        }

        if (Physics.Raycast(ray, out hit, collectDistance))
        {
            CheckCollactable(hit);
        }
        else
        {
            toCollect = null;
        }
    }

    void CheckCollactable(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Collectable"))
        {
            if (toCollect == null)
            {
                toCollect = hit.collider.GetComponent<ICollectable>();
                toCollect.Highlight();
            }
            else
            {
                ICollectable newCollect = hit.collider.GetComponent<ICollectable>();
                if (newCollect != toCollect)
                {
                    toCollect.Unhighlight();
                    toCollect = newCollect;
                    toCollect.Highlight();
                }
            }
        }
        else
        {
            if (toCollect != null)
            {
                var toFuckingDelete = toCollect;
                toCollect = null;
                toFuckingDelete.Unhighlight();
            }
            toCollect = null;
        }
    }

    void CheckInteraction(RaycastHit hit)
    {
        

        if (hit.collider.CompareTag("Interactable"))
        {
            if (toInteract == null)
            {
                toInteract = hit.collider.GetComponent<IInteractable>();
                toInteract.Highlight();
            }
            else
            {
                IInteractable newInteract = hit.collider.GetComponent<IInteractable>();
                if (newInteract != toInteract)
                {
                    toInteract.Unhighlight();
                    toInteract = newInteract;
                    toInteract.Highlight();
                }
            }
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

    public bool IsDarkDimension()
    {
        return choosenLight == 2;
    }

    public bool GetDimensionChanged()
    {
        if(previousLight != choosenLight)
        {
            choosenLight = previousLight;
            return true;   
        }
        return false;
    }

    public void SetControlsBlocked(bool flag)
    {
        this.controlsBlocked = flag;
    }
}
