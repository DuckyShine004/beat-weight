using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{

    public bool isInside = false;
    public Temp temp;
    public BeatBlockMove beatBlock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left click detected");
        }

        if (isInside && Input.GetMouseButtonDown(0) == true)
        {
            temp.addScore();
            if (beatBlock != null)
            {
                beatBlock.KillBlock();
                beatBlock = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInside = true;
        beatBlock = collision.GetComponent<BeatBlockMove>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInside = false;
    }



}
