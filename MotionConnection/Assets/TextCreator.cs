using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCreator : MonoBehaviour
{
    public GameObject textMeshObjectTemplate;
    public OSC osc;
    public GameObject osc_object_manager;
    private OSC_Sender osc_sender;


    List<GameObject> assignTextRows = new List<GameObject>();
    List<GameObject> skeletonDataTextRows = new List<GameObject>();
    List<float> dList = new List<float>();
    
    // Start is called before the first frame update
    void Start()
    {
        //osc.SetAddressHandler( "/assign" , SetAssignmentText );
        osc.SetAddressHandler("/CubeX", OnReceiveX);
        
        osc_sender = osc_object_manager.GetComponent<OSC_Sender>();

        CreateAssignTexts(5);


        dList = osc_sender.dataList;

        int numSkeletonTexts = dList.Count;
        CreateSkeletonDataTexts(numSkeletonTexts);
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkeletonDataTexts();

    }

    void CreateAssignTexts(int numTexts)
    {
        RectTransform rt = this.GetComponent<RectTransform>();
        Vector3[] wCornders = new Vector3[4];
        rt.GetWorldCorners(wCornders);
        Vector3 bottomLeft = wCornders[0];
        Vector3 topLeft = wCornders[1];
        Vector3 topRight = wCornders[2];
        Vector3 bottomRight = wCornders[3];
        int height = (int) Vector3.Distance(topLeft, bottomLeft);
        int width = (int) Vector3.Distance(topRight, topLeft);
        int lineHeight = height / 30;
        int yOffset = height / 15;
        int xOffset = width / 4;

        foreach (GameObject go in assignTextRows)
        {
            Destroy(go);
        }

        assignTextRows.Clear();



        //go = Instantiate(textMeshObjectTemplate, transform);
        //go.GetComponent<TextMeshProUGUI>().text = "wonderful test text ";
        for(int i = 0; i < numTexts; i++)
        {
            
            //textMeshObjectTemplate.GetComponent<TextMeshPro>().text = "wonderful test text ";
            assignTextRows.Add(Instantiate(textMeshObjectTemplate, transform));
            assignTextRows[i].GetComponent<TextMeshProUGUI>().text = "wonderful world";
            assignTextRows[i].transform.position = bottomLeft + new Vector3(xOffset, yOffset + (lineHeight * i), 0);
            //bottomLeft + new Vector3(xOffset, yOffset + (lineHeight * i), 0), transform.rotation
            //assignTextRows[i].GetComponent<TextMeshPro>().text = "wonderful test text ";
        }

    
            
        
    }

    void CreateSkeletonDataTexts(int numTexts)
    {
        RectTransform rt = this.GetComponent<RectTransform>();
        Vector3[] wCornders = new Vector3[4];
        rt.GetWorldCorners(wCornders);
        Vector3 bottomLeft = wCornders[0];
        Vector3 topLeft = wCornders[1];
        Vector3 topRight = wCornders[2];
        Vector3 bottomRight = wCornders[3];
        int height = (int) Vector3.Distance(topLeft, bottomLeft);
        int width = (int) Vector3.Distance(topRight, topLeft);
        int lineHeight = height / 35;
        int yOffset = height / 2;
        int xOffset = width / 4;

        foreach (GameObject go in skeletonDataTextRows)
        {
            Destroy(go);
        }
        skeletonDataTextRows.Clear();
        for(int i = 0; i < numTexts; i++)
        {
            
            //textMeshObjectTemplate.GetComponent<TextMeshPro>().text = "wonderful test text ";
            skeletonDataTextRows.Add(Instantiate(textMeshObjectTemplate, transform));
            skeletonDataTextRows[i].GetComponent<TextMeshProUGUI>().text = "wonderful world";
            skeletonDataTextRows[i].transform.position = bottomLeft + new Vector3(xOffset, yOffset + (lineHeight * i), 0);
            //bottomLeft + new Vector3(xOffset, yOffset + (lineHeight * i), 0), transform.rotation
            //assignTextRows[i].GetComponent<TextMeshPro>().text = "wonderful test text ";
        }


    
            
        
    }

    void OnReceiveX(OscMessage message) {
        float x = message.GetFloat(0);
        int numArgsInMsg = message.values.Count;
        CreateAssignTexts(numArgsInMsg);
        for (int i = 0; i < numArgsInMsg; i++)
        {
           assignTextRows[i].GetComponent<TextMeshProUGUI>().text = message.values[i].ToString();    
           
        }
        

    }

    void UpdateSkeletonDataTexts() {
        dList = osc_sender.dataList;
        int numTexts = dList.Count;

        for (int i = 0; i < numTexts; i++)
        {
           skeletonDataTextRows[i].GetComponent<TextMeshProUGUI>().text = dList[i].ToString();    
        }
        

    }
}
