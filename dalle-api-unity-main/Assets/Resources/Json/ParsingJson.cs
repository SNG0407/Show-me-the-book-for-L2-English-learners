using System; /* for Serializable */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParsingJson : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public string name;
        public int lives;
        public float health;

        public static PlayerInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }

        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }
    [Serializable]
    public class Lotto
    {
        public int id;
        public string date;
        public int[] number;
        public int bonus;

        public void printNumbers()
        {
            string str = "numbers : ";
            for (int i = 0; i < 6; i++) str += number[i] + " ";

            Debug.Log(str);
            Debug.Log("bonus : " + bonus);
        }
    }
    public class Player_Data
    {
        public int kill_Count;
        public int Hit_Count;

        public Player_Data(int killcount, int hitcount)
        {
            kill_Count = killcount;
            Hit_Count = hitcount;
        }

        public void getKillCount()
        {
            Debug.Log("killcount: " + kill_Count);
        }

        public void getHitCount()
        {
            Debug.Log("hitcount: " + Hit_Count);
        }
    }

    public class LottoNumbers
    {
        public Lotto[] winning;
    }
    string ObjectToJson(object obj)
    {
        //ToJson의 2번째 매개변수(prettyprint)는 생성될 Json의 형식을 이쁘게 보이게 해주기 위함
        return JsonUtility.ToJson(obj, true);
    }

    T JsonToObject<T>(string JsonData) where T : class
    {
        return JsonUtility.FromJson<T>(JsonData);
    }
    void Start()
    {
        Player_Data playerData = new Player_Data(10, 1);
        string json_Data = ObjectToJson(playerData); //Json 형식으로
        Debug.Log(json_Data);

        Player_Data playrData_1 = JsonToObject<Player_Data>(json_Data); //Json 형식 받아오는
        playrData_1.getKillCount();
        playrData_1.getHitCount();

        //json파일로 저장
        string path = Path.Combine(Application.dataPath + "/playerData.json");
        File.WriteAllText(path, json_Data);

        //json파일도 물론 받아올 수 있음
        string getJSON = File.ReadAllText(Application.dataPath + "/playerData.json");
        Player_Data playerData_2 = JsonToObject<Player_Data>(getJSON);
        playerData_2.getKillCount();
        playerData_2.getHitCount();

        //
        TextAsset textAsset = Resources.Load<TextAsset>("Json/LottoWinningNumber");
        Debug.Log("textAsset.text: "+ textAsset.ToString());

        string JsonText = "{\"Items\":" + textAsset.ToString() + "}"; //https://hungry2s.tistory.com/214

        LottoNumbers lottoList = JsonUtility.FromJson<LottoNumbers>(JsonText);
        Debug.Log("lottoList: " + lottoList);

        foreach (Lotto lt in lottoList.winning)
        {
            lt.printNumbers();
            Debug.Log("=============");
        }

        string classToJson = JsonUtility.ToJson(lottoList);
        Debug.Log(classToJson);
    }
}