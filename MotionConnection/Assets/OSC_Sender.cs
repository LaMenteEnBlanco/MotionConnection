using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OSC_Sender : MonoBehaviour
{
    public List<float> dataList;
    // Start is called before the first frame update
    void Start()
    {
        partsArray = new GameObject[] { Hand_left, Hand_right, Elbow_left, Elbow_right, Shoulder_left, Shoulder_right, Hip_left, Hip_right, Knee_left, Knee_right, Foot_left, Foot_right};
        compressednessPartsArray = new GameObject[] { Hand_left, Hand_right, Elbow_left, Elbow_right,  Knee_left, Knee_right, Foot_left, Foot_right};
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 midBetweenShoulders = Vector3.Lerp(Shoulder_left.transform.position, Shoulder_right.transform.position, 0.5f);
        vectorBetweenShoulders = Shoulder_left.transform.position - Shoulder_right.transform.position;
        //Vector3 midBetweenHips = Vector3.Lerp(Hip_left.transform.position, Hip_right.transform.position, 0.5f);
        vectorBetweenHips = Hip_left.transform.position - Hip_right.transform.position;
        vectorForearmLeft = Hand_left.transform.position - Elbow_left.transform.position;
        vectorUpperarmLeft = Elbow_left.transform.position - Shoulder_left.transform.position;
        vectorForearmRight = Hand_right.transform.position - Elbow_right.transform.position;
        vectorUpperarmRight = Elbow_right.transform.position - Shoulder_right.transform.position;

        vectorLowerlegLeft = Foot_left.transform.position - Knee_left.transform.position;
        vectorThighLeft = Knee_left.transform.position - Hip_left.transform.position;
        vectorLowerlegRight = Foot_right.transform.position - Knee_right.transform.position;
        vectorThighRight = Knee_right.transform.position - Hip_right.transform.position; 
        vectorLatusLeft = Hip_left.transform.position - Shoulder_left.transform.position;
        vectorLatusRight = Hip_right.transform.position - Shoulder_right.transform.position;

        distanceHands = 0;
        distanceElbows = 0;
        averageHeight = 0;
        angleShouldersHips = Vector3.Angle(vectorBetweenHips, vectorBetweenShoulders);
        compressedness = 0;
        angleElbowLeft = Vector3.Angle(vectorForearmLeft, vectorUpperarmLeft);
        angleElbowRight = Vector3.Angle(vectorForearmRight, vectorUpperarmRight);
        angleKneeLeft = Vector3.Angle(vectorLowerlegLeft, vectorThighLeft);
        angleKneeRight = Vector3.Angle(vectorLowerlegRight, vectorThighRight);
        angleShoulderLeft = Vector3.Angle(vectorUpperarmLeft, vectorLatusLeft);
        angleShoulderRight = Vector3.Angle(vectorUpperarmRight, vectorLatusRight);

        int numRecognizedParts = 0;
        foreach (GameObject go in partsArray)
        {
            if (go.transform.position != null)
            {
                if (go.transform.position.y > 0)
                {
                    numRecognizedParts = numRecognizedParts + 1;
                }
            }
        }

        distanceHands = Vector3.Distance(Hand_left.transform.position, Hand_right.transform.position) / distanceHandsDivisor;
        distanceElbows= Vector3.Distance(Elbow_left.transform.position, Elbow_right.transform.position) / distanceElbowsDivisor;
        averageHeight = (Hand_left.transform.position.y + Hand_right.transform.position.y + Elbow_left.transform.position.y + Elbow_right.transform.position.y + Shoulder_left.transform.position.y + Shoulder_right.transform.position.y + Hip_left.transform.position.y +
            Hip_right.transform.position.y + Knee_left.transform.position.y + Knee_right.transform.position.y +
            Foot_left.transform.position.y + Foot_right.transform.position.y) / (numRecognizedParts * averageHeightDivisor);
        
        //compressedness 
        int partCounter = 0;
        foreach(GameObject part in compressednessPartsArray)
        {
            float partialCompressedness = 0;
            int counterPartCounter = 0;
            foreach(GameObject counterPart in compressednessPartsArray)
            {
                if( (counterPart != part) && (counterPart.transform.position.y > 0))
                {
                    float distance = Vector3.Distance(part.transform.position, counterPart.transform.position);
                    partialCompressedness = partialCompressedness + distance;
                    counterPartCounter +=1;
                }
            }
            partialCompressedness = partialCompressedness / counterPartCounter;
            compressedness = compressedness + partialCompressedness;
            partCounter+=1;
        }   
        compressedness =  compressedness / partCounter;
        //scaling compressedness from 0 -1
        compressedness = map(compressedness, 0.3f, 1.2f, 0.0f, 1.0f);
        
        // if(dbgIntervallCounter >= 20)
        // {
        //     Debug.Log("average height: " + averageHeight + "distanceHands: " + distanceHands + "distanceElbows: " + distanceElbows + " anleShouldersHips: " + angleShouldersHips + " compressedness: " + compressedness);
        //     dbgIntervallCounter = 0;
        // }else{dbgIntervallCounter+=1;}
        
        dataList.Clear();
        OscMessage message = new OscMessage();
        message.address = "/wek/outputs";
        //message.address = "/wek/inputs";
        message.values.Add(distanceHands);
        dataList.Add(distanceHands);
        message.values.Add(distanceElbows);
        dataList.Add(distanceElbows);
        message.values.Add(averageHeight);
        dataList.Add(averageHeight);
        message.values.Add(angleShouldersHips);
        dataList.Add(angleShouldersHips);
        message.values.Add(compressedness);
        dataList.Add(compressedness);
        message.values.Add(angleElbowLeft);
        dataList.Add(angleElbowLeft);
        message.values.Add(angleElbowRight);
        dataList.Add(angleElbowRight);
        message.values.Add(angleKneeLeft);
        dataList.Add(angleKneeLeft);
        message.values.Add(angleKneeRight);
        dataList.Add(angleKneeRight);
        message.values.Add(angleShoulderLeft);
        dataList.Add(angleShoulderLeft);
        message.values.Add(angleShoulderRight);
        dataList.Add(angleShoulderRight);
        osc.Send(message);





        
    }

    float map(float input, float iMin, float iMax, float oMin, float oMax)
    {
        return oMin + (input - iMin) * (oMax - oMin)/(iMax- iMin); 
    }

    public OSC osc;

    public GameObject Hand_left;

    public GameObject Hand_right;

    public GameObject Elbow_left;

    public GameObject Elbow_right;

    public GameObject Shoulder_left;

    public GameObject Shoulder_right;

    public GameObject Hip_left;

    public GameObject Hip_right;

    public GameObject Knee_left;

    public GameObject Knee_right;

    public GameObject Foot_left;

    public GameObject Foot_right;


    //Calculation / Calibration Parameters
    float averageHeightDivisor = 1.25f;
    float distanceHandsDivisor = 1.32f;
    float distanceElbowsDivisor = 0.95f;
    int dbgIntervallCounter = 0;

    float distanceHands = 0;
    float distanceElbows = 0;
    float averageHeight = 0;
    float angleShouldersHips = 0;
    float compressedness = 0;

    float angleElbowLeft = 0;
    Vector3 vectorForearmLeft;
    Vector3 vectorUpperarmLeft;

    float angleElbowRight = 0;    
    Vector3 vectorForearmRight;    
    Vector3 vectorUpperarmRight;

    float angleKneeLeft = 0;
    Vector3 vectorLowerlegLeft;
    Vector3 vectorThighLeft;

    float angleKneeRight = 0;    
    Vector3 vectorLowerlegRight;
    Vector3 vectorThighRight;

    float angleShoulderLeft = 0;
    Vector3 vectorLatusLeft;

    float angleShoulderRight = 0;    
    Vector3 vectorLatusRight;


    GameObject[] partsArray;
    GameObject[] compressednessPartsArray;

    Vector3 vectorBetweenShoulders;
        //Vector3 midBetweenHips = Vector3.Lerp(Hip_left.transform.position, Hip_right.transform.position, 0.5f);
    Vector3 vectorBetweenHips;
    
}


