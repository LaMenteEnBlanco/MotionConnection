using UnityEngine;
using System.Collections;
using TMPro;

public class ReceivePosition : MonoBehaviour {
    
   	public OSC osc;
    // public GameObject assign_object_1;
    // public GameObject assign_object_2;
    // public GameObject assign_object_3;
    // public GameObject assign_object_4;
    // public GameObject assign_object_5;
    // private TMP_Text assign_text_1;
    // private TMP_Text assign_text_2;
    // private TMP_Text assign_text_3;
    // private TMP_Text assign_text_4;
    // private TMP_Text assign_text_5;


	// Use this for initialization
	void Start () {
       osc.SetAddressHandler( "/assign" , SetAssignmentText );
       osc.SetAddressHandler("/CubeX", OnReceiveX);


    //    assign_text_1 = assign_object_1.GetComponent<TMP_Text>();
    //    assign_text_2 = assign_object_2.GetComponent<TMP_Text>();
    //    assign_text_3 = assign_object_3.GetComponent<TMP_Text>();
    //    assign_text_4 = assign_object_4.GetComponent<TMP_Text>();
    //    assign_text_5 = assign_object_5.GetComponent<TMP_Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetAssignmentText(OscMessage message){
        // Debug.Log("OSC Message Received -- : ");
		// assign_text_1.text = message.values[0].ToString();
        // Debug.Log("assign_text_1 -- : " + assign_text_1.text );
        // assign_text_2.text = message.values[1].ToString();
        // Debug.Log("assign_text_2 -- : " + assign_text_2.text );
		// assign_text_3.text = message.values[2].ToString();
        // Debug.Log("assign_text_3 -- : " + assign_text_3.text );
        // assign_text_4.text = message.values[3].ToString();
        // Debug.Log("assign_text_4 -- : " + assign_text_4.text );
        // assign_text_5.text = message.values[4].ToString();

	}

    void OnReceiveX(OscMessage message) {
        float x = message.GetFloat(0);
        string str = message.values[1].ToString();

        Vector3 position = transform.position;

        position.x = x;

        transform.position = position;

        Debug.Log("OSC Message Received: 0: " + x + " 1: " + str);
    }

    void OnReceiveY(OscMessage message) {
        float y = message.GetFloat(0);

        Vector3 position = transform.position;

        position.y = y;

        transform.position = position;
    }

    void OnReceiveZ(OscMessage message) {
        float z = message.GetFloat(0);

        Vector3 position = transform.position;

        position.z = z;

        transform.position = position;
    }


}