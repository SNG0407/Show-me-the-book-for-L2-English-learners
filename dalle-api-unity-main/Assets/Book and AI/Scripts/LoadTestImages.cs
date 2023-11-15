using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using REST_API_HANDLER;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

public class LoadTestImages : MonoBehaviour
{
    public RawImage[] rawImages; // Reference to your RawImage component

    // Start is called before the first frame update
    void Start()
    {
        //rawImages = new RawImage[20];
        // Initialize rawImages array with references to RawImage components
        //rawImages = GetComponentsInChildren<RawImage>(); //Is it because of this? No..
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadImages()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, "Uploaded_AI_Files", description + ".jpg");
        //if (File.Exists(filePath))
        //{
        //    string fileContents = File.ReadAllText(filePath);
        //    // Now use the fileContents as needed, e.g., display it in a UI Text element.
        //    Debug.Log("File found: " + fileContents);

        //}
        //else
        //{
        //    Debug.LogError("File not found: " + filePath);
        //    // Handle the situation where the file doesn't exist.
        //}

        string[] filePaths = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Uploaded_AI_Files"), "*.jpg");
        //rawImages = new RawImage[filePaths.Length]; // Initialize the rawImages array

        int count = 0;
        foreach (string filePath in filePaths)
        {
            Debug.Log(count+". File found: " + filePath);

            Texture2D texture = new Texture2D(256, 256);

            byte[] bytes = File.ReadAllBytes(filePath);
            texture.LoadImage(bytes);

            // Use the loaded texture here
            // Create a new sprite from the loaded texture
            //Sprite imageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // Assign the sprite to the RawImage component
            if(texture != null)
            {
                Debug.Log("----"+ texture.width);
                //rawImages[count] = GetComponent<RawImage>();
                rawImages[count].texture = texture;
            }
            else
            {
                Debug.Log("Null");

            }
            count++;
        }
    }
}
