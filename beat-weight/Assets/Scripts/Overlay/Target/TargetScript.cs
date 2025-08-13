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
        temp = GameObject.FindGameObjectWithTag("Hit").GetComponent<Temp>();
        //beatBlock = GameObject.FindGameObjectWithTag("BeatBlock").GetComponent<BeatBlockMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.Space) == true)
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
