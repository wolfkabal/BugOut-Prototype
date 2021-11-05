using UnityEngine;

/// <summary>
/// Simple example of Grabbing system.
/// </summary>
public class SimpleGrabSystem : MonoBehaviour
{
    // Reference to the character camera.
    [SerializeField] private Rigidbody player;
    //private Camera characterCamera;

    // Reference to the slot for holding picked item.
    [SerializeField] private Transform slot;
    [SerializeField] private float DropForce;


    // Reference to the currently held item.
    private PickableItem pickedItem;
    private bool itemCollected = false;

    /// <summary>
    /// Method called very frame.
    /// </summary>
    private void Update()
    {
        // Execute logic only on button pressed
        if (Input.GetButtonDown("Fire1"))
        {
            // Check if player picked some item already
            if (pickedItem)
            {
                // If yes, drop picked item
                DropItem(pickedItem);
            }
            else
            {
                // If no, try to pick item in front of the player
                // Create ray from center of the screen
                //var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
                Ray ray = new Ray(player.transform.position, player.transform.forward);
                RaycastHit hit;
                // Shot ray to find object to pick
                if (Physics.Raycast(ray, out hit, 1.5f))
                {
                    // Check if object is pickable
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    var collectable = hit.transform.GetComponent<CollectableItem>();
                    
                    // If object has PickableItem class
                    if (pickable)
                    {
                        if(collectable && !itemCollected)
                        {
                            CollectItem(collectable);
                        }

                        //Debug.Log($"Pickable: {pickable.name}");
                        // Pick it
                        PickItem(pickable);
                    }
                }
            }
        }
        // Execute logic only on button pressed
        if (Input.GetButtonDown("Fire2"))
        {
            // Check if player picked some item already
            if (pickedItem) {
                TossItem(pickedItem);
            } else {
                Ray ray = new Ray(player.transform.position, player.transform.forward);
                RaycastHit hit;
                // Shot ray to find object to pick
                if (Physics.Raycast(ray, out hit, 1.5f)) {
                    // Check if object is pickable
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    var collectable = hit.transform.GetComponent<CollectableItem>();
                    
                    // If object has PickableItem class
                    if (pickable) {
                        if(collectable && !itemCollected) {
                            CollectItem(collectable);
                        }
                        //Debug.Log($"Pickable: {pickable.name}");
                        // Pick it
                        TossItem(pickable);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Method for collecting item.
    /// </summary>
    /// <param name="item"></param>
    private void CollectItem(CollectableItem item)
    {
        Debug.Log($"Item Collected!!!!");
        itemCollected = true;
    }

    /// <summary>
    /// Method for picking up item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void PickItem(PickableItem item)
    {
        // Assign reference
        pickedItem = item;

        // Disable rigidbody and reset velocities
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;

        // Set Slot as a parent
        item.transform.SetParent(slot);

        // Reset position and rotation
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;

    }

    /// <summary>
    /// Method for tossing item in the air.
    /// </summary>
    /// <param name="item">Item.</param>
    private void TossItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;
        Vector3 toss = Vector3.up * DropForce;
        item.Rb.AddForce(toss,ForceMode.Impulse);
    }

    /// <summary>
    /// Method for dropping item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void DropItem(PickableItem item)
    {
        // Remove reference
        pickedItem = null;

        // Remove parent
        item.transform.SetParent(null);

        // Enable rigidbody
        item.Rb.isKinematic = false;

        // Add force to throw item a little bit
        item.Rb.AddForce(item.transform.forward * DropForce, ForceMode.VelocityChange);
    }
}