using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public bool isInside = false;
    public Temp temp;
    public BeatBlockMove beatBlock;
    public HitTextScript hitTextScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (isInside && Input.GetMouseButtonDown(0))
        {
            temp.addScore(2);
            hitTextScript.ShowText("Perfect");
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
