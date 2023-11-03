using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public string jsonString = "{\"TestObject\":{\"SomeText\":\"Blah\",\"SomeObject\":{\"SomeNumber\":42,\"SomeBool\":true,\"SomeNull\":null},\"SomeEmptyObject\":{},\"SomeEmptyArray\":[],\"EmbeddedObject\":\"{\\\"field\\\":\\\"Valuewith\\\\\\\"escapedquotes\\\\\\\"\\\"}\"}}";
    // Start is called before the first frame update
    void Start()
    {
        Defective.JSON.JSONObject jsonObject = new Defective.JSON.JSONObject(jsonString);
        Defective.JSON.JSONObject testObject = jsonObject.GetField("TestObject");
        Debug.Log("testObject: " + testObject);
        Debug.Log("testObject: " + testObject.Print());
        Debug.Log("testObject: " + testObject.Print().GetType());
        Debug.Log("testObject: " + testObject.stringValue);

        if (testObject.GetField("TestObject").stringValue != null)
        {
            string someObject = testObject.GetField("TestObject").stringValue;
            Debug.Log("someObject: " + someObject);
        }
        else
        {
            Debug.Log("someObject: null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
