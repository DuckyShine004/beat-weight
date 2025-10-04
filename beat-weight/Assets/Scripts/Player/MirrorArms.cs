using UnityEngine;

public class MirrorCalibration : MonoBehaviour
{
    public HandManager handManager;

    public Transform leftHand;
    public Transform rightHand;

    public Transform head;

    public KeyCode mirrorKey = KeyCode.M;

    private Transform _activeHand;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activeHand = handManager.activeHand == HandManager.Hand.Left ? leftHand : rightHand;
    }

    void Update()
    {
        if (Input.GetKeyDown(mirrorKey))
        {
            _activeHand = handManager.activeHand == HandManager.Hand.Left ? rightHand : leftHand;
            DelayedStart();
        }

    }

    // Update is called once per frame
    void MirrorHands()
    {
        if (!head || !_activeHand) return;

        Vector3 headPos = head.position;
        Vector3 handPos = _activeHand.position;

        // Mirror position
        Vector3 offset = handPos - headPos;
        offset.x = -offset.x; // mirror on X axis
        transform.position = headPos + offset;
        transform.rotation = _activeHand.rotation;
        

        // Mirror rotation
        Vector3 handEuler = _activeHand.eulerAngles;
        transform.eulerAngles = new Vector3(handEuler.x, -handEuler.y, -handEuler.z);

        if (handManager.activeHand == HandManager.Hand.Right)
        {
            rightHand.position = transform.position;
            rightHand.rotation = transform.rotation;
        }
        else
        {
            leftHand.position = transform.position;
            leftHand.rotation = transform.rotation;
        }
    }

    void DelayedStart()
    {
        Invoke("MirrorHands", 2f);
    }
}
