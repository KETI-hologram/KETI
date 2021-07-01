using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class create_object : MonoBehaviour
{
    public int count;
    List<string> files;
    static string root = "Assets/Scenes/resources";
    private static List<string> GF()
    {
        List<string> files = new List<string>();
        if (System.IO.Directory.Exists(root))
        {
            string[] Allobj = System.IO.Directory.GetFiles(root, "*.obj", SearchOption.AllDirectories);
            
            foreach (var file in Allobj)
            {
                string[] tmp = file.Split('/');
                string name = string.Join("/", tmp, 3, tmp.Length-3).Split('.')[0];
                Debug.Log(name);
                files.Add(name);
            }
        }    
        return files;
    }

    private void Awake()
    {
        count = -1;
        files = GF();
    }


    void Start()
    {
        Create_new_object();
        // GameObject sp = Resources.Load("ob") as GameObject;
        // GameObject instance = Instantiate(sp, new Vector3 (0,0,0), Quaternion.identity);
        // instance.AddComponent<Rotate>();
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Create_new_object()
    {
        count += 1;
        Debug.Log(count);
        if (count >= files.Count)
        {
            count = 0;
            Application.Quit();
        }
        else
        {
            GameObject sp = Resources.Load(files[count]) as GameObject;
            GameObject instance = Instantiate(sp, new Vector3 (0,0,0), Quaternion.identity);
            instance.AddComponent<Rotate>();

            GameObject.Find("camera60").GetComponent<sixty>().StartRecord(files[count]);
            GameObject.Find("camera45").GetComponent<fourtyfive>().StartRecord(files[count]);
            GameObject.Find("camera30").GetComponent<thirty>().StartRecord(files[count]);
            

        }
        
    }
}