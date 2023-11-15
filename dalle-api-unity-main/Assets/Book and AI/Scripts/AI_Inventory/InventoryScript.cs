using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject ContentParent;
    public GameObject inventorySlotPrefab;

    //public RawImage[] AI_Images; // Reference to your RawImage component
    public string[] fileNames; // Reference to your RawImage component

    public int Current_Item_Num=0;

    private bool isInventoryVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel.SetActive(false);

        string[] filePaths = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Uploaded_AI_Files"), "*.jpg");
        //rawImages = new RawImage[filePaths.Length]; // Initialize the rawImages array

        Current_Item_Num = filePaths.Length;
        //AI_Images = new RawImage[Current_Item_Num];
        fileNames = new string[Current_Item_Num];

        Debug.Log("How many files found: " + Current_Item_Num);

        int count = 0;
        foreach (string filePath in filePaths)
        {
            fileNames[count]= Path.GetFileName(filePath);
            Debug.Log(count + ". File Name: " + fileNames[count]);
            //Debug.Log(count + ". File found: " + filePath);

            Texture2D texture = new Texture2D(256, 256);

            byte[] bytes = File.ReadAllBytes(filePath);
            texture.LoadImage(bytes);

            // Use the loaded texture here
            // Create a new sprite from the loaded texture
            //Sprite imageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // Assign the sprite to the RawImage component
            if (texture != null)
            {
                Debug.Log("----" + texture.width);
                //rawImages[count] = GetComponent<RawImage>();
                //AI_Images[count].texture = texture;

                AddItem(fileNames[count], texture);
            }
            else
            {
                Debug.Log("Null");

            }
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isInventoryVisible = !isInventoryVisible;
            inventoryPanel.SetActive(isInventoryVisible);
        }
    }
    public void CreateFrame(string fileName)
    {
        // Call a function to instantiate the frame prefab using the file name
        // You can implement this part based on your specific requirements
    }

    // Call this function to add an item to the inventory
    public void AddItem(string fileName, Texture2D fileImage)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, ContentParent.transform);
        InventorySlotScript slotScript = slot.GetComponent<InventorySlotScript>();
        slotScript.SetItem(fileName, fileImage, this);
    }
}
