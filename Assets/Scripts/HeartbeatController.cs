using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class HeartbeatController : MonoBehaviour
{
    [Header("Game Objects")]
    public UILineRenderer UILineRenderer;

    public GameObject positionObj;

    public Text heartRateText;

    public Color lineColour;

    [Header("Attributes")]

    [Range(0,10)]
    public float lineWidth;

    public int resolution;

    public int heartRate;

    public float flatLineLength;
    private List<Vector2> points;

    int maxPoints;

    int curPosition;

    bool beatFlag;
    int iterations;

    float graphX = 0;

    int zeroCounter;

    float currentFlatLength;

    float curX;

    // Rect
    RectTransform lineRendererRect;

    float width;
    float height;

    float padding = 1;

    void Start()
    {
        UILineRenderer.color = lineColour;
        UILineRenderer.LineThickness = lineWidth;
        points = new List<Vector2>();
        curPosition = 0;
        iterations = 0;
        graphX = 0;
        zeroCounter = 0;
        currentFlatLength = 0.0f;
        flatLineLength = 100f / (float)heartRate;

        lineRendererRect = UILineRenderer.gameObject.GetComponent<RectTransform>();
        width = lineRendererRect.rect.width;
        height = lineRendererRect.rect.height;

        maxPoints = (int)width * resolution;

        for(int i = 0 ; i < maxPoints ; i++ ){
            points.Add(new Vector2(i/3f,0));
        }

    }

    // Update is called once per frame
    void Update()
    {
        flatLineLength = 400f / (float)heartRate;
        if(!beatFlag)
            StartCoroutine("Beat");
    }

    IEnumerator Beat() {
        beatFlag = true;
        heartRateText.text = heartRate.ToString();

        if(curPosition >= maxPoints) {
            curPosition = 0;
            curX = 0;
        }
                

        curX += (width / maxPoints);
       
        if(graphX >= 4.18f) {
            graphX= 0;
            Debug.Log(iterations);
            iterations = 0;
        }
            

        if(zeroCounter == 4){
            points[curPosition] = new Vector3(curX - width/2f + padding,Random.Range(0f,.3f));
            currentFlatLength += 1/3f;
                
        }else{
            float y = (1f+0.3f*Mathf.Sin(graphX*0.2f) ) * ((Mathf.Sin(graphX)+Mathf.Sin(2f*graphX))) ;
            if(Mathf.Abs(y) < 0.1f)
                zeroCounter++;
            points[curPosition] = new Vector3(curX - width/2f + padding, y*(heartRate/5f));
        }
        
        RectTransform blipTransform = positionObj.GetComponent<RectTransform>();
        blipTransform.localPosition = new Vector2(points[curPosition].x,points[curPosition].y);                
        curPosition++;

        if(currentFlatLength >= flatLineLength) {
                currentFlatLength = 0;
                zeroCounter = 0;
                graphX = 0;
                iterations = 0;
        } else {
            graphX = iterations/3f;
            iterations++;
        }

        UILineRenderer.Points = points.ToArray();

        yield return new WaitForSeconds(1f/(float)maxPoints);
        beatFlag = false;
    }
}
