using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileHandler 
{
    private string dataPirPath = "";

    private string dataFileName = "";

    private bool useEncryption = false;
    private string encryptKeyword = "patates"; 
 

    public FileHandler(string dataFname , string dataPPath , bool isEncryptionOn)
    {
        Debug.Log("DATAFNAME : " + dataFname + "\nDATAPPath : " + dataPPath);
        this.dataFileName = dataFname;
        this.dataPirPath = dataPPath;
        this.useEncryption = isEncryptionOn;
    }

    public GameData Load(string profileID) 
    {
        string fullPath = Path.Combine(dataFileName ,profileID, dataPirPath);
        Debug.Log(profileID +"  ///////  "+ fullPath);
        GameData LoadedData = null;

        if (File.Exists(fullPath))
        {
            string data2load = "";
            try
            {

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader rdr = new StreamReader(stream)) {
                            data2load = rdr.ReadToEnd();
                        } 
                    }

                    if (useEncryption)
                    {
                        data2load = EncryptDecrypt(data2load);
                    }

                    // deserialize data from string to c# script 
                    LoadedData = JsonUtility.FromJson<GameData>(data2load);

            }
            catch (Exception e)
            {
                    Debug.LogError("Error occured when trying to load the file : " + fullPath + "\n" + e.Message);
            }


        }

        return LoadedData;


    }

    public void Save(string profileID , GameData data)
    {
        string fullPath = Path.Combine(dataFileName , profileID , dataPirPath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string data2Store = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                data2Store = EncryptDecrypt(data2Store);
            }



            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter wr = new StreamWriter(stream, System.Text.Encoding.UTF8))
                {
                    wr.Write(data2Store);
                }
            }

            

        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save the file : " + fullPath + "\n" + e.Message );
        }
    }


    public Dictionary<string , GameData> LoadAllProfile()
    {
        Dictionary<string, GameData> profileDirectory = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfo = new DirectoryInfo(Path.Combine(dataPirPath, dataFileName)).EnumerateDirectories();
        foreach(DirectoryInfo info in dirInfo)
        {
            string profileID = info.Name;
            string fullPath = Path.Combine(dataPirPath, profileID, dataFileName);

            if (!File.Exists(fullPath)) Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data");

            GameData profileData = Load(profileID);
            if (profileData != null)
            {
                profileDirectory.Add(profileID, profileData);
            }
            else Debug.LogError("Tried to load profile but something went wrong. ProfileID : " + profileID);
        }

        return profileDirectory;
    }

    public string EncryptDecrypt(string data) // XOR Encryption
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i += 1)
            modifiedData += (char)(data[i] ^ encryptKeyword[i % encryptKeyword.Length]);

        return modifiedData;
    }



}
