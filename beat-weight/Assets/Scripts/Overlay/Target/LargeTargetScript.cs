using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LargeTargetScript : MonoBehaviour
{

    public TargetScript mainTarget;
    public bool isInsideOuter = false;
    public Temp temp;
    public BeatBlockMove beatBlock;
    public HitTextScript hitTextScript;
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

        if (isInsideOuter && !mainTarget.isInside && Input.GetMouseButtonDown(0) == true)
        {
            temp.addScore(1);
            hitTextScript.ShowText("Early");
            if (beatBlock != null)
            {
                beatBlock.KillBlock();
                beatBlock = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInsideOuter = true;
        beatBlock = collision.GetComponent<BeatBlockMove>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInsideOuter = false;
    }



}
