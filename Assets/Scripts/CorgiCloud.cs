using UnityEngine;

public class CorgiCloud : MonoBehaviour
{
    public float[] stageYPositions; // set 4 Y values in Inspector e.g. 0, 5, 10, 15
    private int currentStage = 0;
    public float riseSpeed = 3f;
    private bool rising = false;
    private float targetY;

    void Update()
    {
        if (rising)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                riseSpeed * Time.deltaTime
            );
            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
                rising = false;
        }
    }

    public void RiseToNextStage()
    {
        currentStage++;
        if (currentStage < stageYPositions.Length)
        {
            targetY = stageYPositions[currentStage];
            rising = true;
        }
    }
}