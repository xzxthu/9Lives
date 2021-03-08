using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Cinemachine;
using UnityEngine.SceneManagement;
using System.Windows;

public class LevelManager : MonoBehaviour
{
    public bool isDrawMode = true;
    
    public float oneWayRestoreTime;

    public bool isInMenu = true;
    public bool isStarting = false;

    private GameObject[] SGs;

    //private string dataPath = Application.persistentDataPath;
    private string dataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

    [HideInInspector]public static LevelManager instance;
    //[HideInInspector] public ArrayList settings = new ArrayList();
    [HideInInspector] public int settingCount = 0;
    public SaveData activeSave;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            DrawEnding();
            Save();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            Load();
            ReDraw();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isInMenu)
            {
                isInMenu = true;
                isStarting = false;

                SceneManager.LoadScene("Start");
                return;
            }
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

    }

    



    public void DrawEnding() //保存所有地块的位置
    {
        GameObject[] SGs;
        SGs = GameObject.FindGameObjectsWithTag("SettingGround");
        activeSave.saveSettings = new Vector3[SGs.Length];
        for(int i = 0; i<SGs.Length;i++)
        {
            activeSave.saveSettings[i] = SGs[i].transform.position;
        }
    }

    public void ReDraw() //重新绘制地块
    {
        for(int i = 0; i < activeSave.saveSettings.Length-1; i++)//除去原来的
        {
            Instantiate(GameObject.FindWithTag("SettingGround"));
        }
        
        SGs = GameObject.FindGameObjectsWithTag("SettingGround");
        for (int i = 0; i < SGs.Length; i++)
        {
            SGs[i].transform.position = activeSave.saveSettings[i];
            //SGs[i].GetComponent<GroundController>().SetTheGround();
        }

    }

    public void Save()
    {

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(dataPath + "/MySave.save", FileMode.Create );
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Save!");
    }

    public void Load()
    {

        if(System.IO.File.Exists(dataPath + "/MySave.save"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(dataPath + "/MySave.save", FileMode.Open);

            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();
        }

        Debug.Log("Load!  " + activeSave.saveSettings.Length);
    }

    public void DeleteSaveData()
    {

        if (System.IO.File.Exists(dataPath + "/MySave.save"))
        {
            File.Delete(dataPath + "/MySave.save");
        }
    }

    private void EnterLevel()
    {
        SceneManager.LoadScene("DrawYourLevel");
        LevelManager.instance.isInMenu = false;
    }

    public void EnterDraw()
    {
        LevelManager.instance.isDrawMode = true;
        EnterLevel();
        Debug.Log("button!");
    }

    public void EnterLoad()
    {
        LevelManager.instance.isDrawMode = false;
        EnterLevel();
    }

}

[System.Serializable]
public class SaveData
{
    public Vector3[] saveSettings;
}
