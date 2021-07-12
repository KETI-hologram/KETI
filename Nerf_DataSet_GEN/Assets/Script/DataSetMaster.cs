using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataSetMaster : MonoBehaviour
{
    public GameObject sampleCamera;
    public Transform cameraParent;
    GameObject[] cameras;

    int width = 4032, height = 3024;
    float fov=60;
    bool isLF = true;

    public InputField inResW, inResH, inInter, inFOV,inZ;
    public Toggle tgisLF;

    //¹Ý±¸
    public float r = 3;
    public int noc = 25;

    //LF
    public int NoCW=5,NoCH=5;
    public float interval=1;

    // Start is called before the first frame update
    void Start()
    {
        cameras = new GameObject[NoCW * NoCH];
        Relocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Relocation();
        }
        //if (Input.GetKey(KeyCode.G))
        //{
        //    StartCoroutine(Generate());
        //}
    }
    
    void Relocation()
    {
        if (isLF)
        {
            cameraParent.position = -Vector3.forward * float.Parse(inZ.text);
            for (int y = 0; y < NoCH; y++)
            {
                for (int x = 0; x < NoCW; x++)
                {
                    Destroy(cameras[NoCW * y + x]);
                    cameras[NoCW * y + x] = Instantiate(sampleCamera, new Vector3((x - (NoCW - 1) / 2) * interval, ((float)(NoCH - 1)/2 - y) * interval, cameraParent.position.z), Quaternion.identity, cameraParent);
                    cameras[NoCW * y + x].GetComponent<Camera>().fieldOfView = fov;
                }
            }
        }
        else
        {
            cameraParent.position = Vector3.zero;
            r= float.Parse(inZ.text);
            for (int i = 0; i < noc; i++)
            {
                Destroy(cameras[i]);
                float x = Random.Range(-r, r);
                float z = Random.Range(-Mathf.Pow(r, 2) + Mathf.Pow(x, 2), Mathf.Pow(r, 2) - Mathf.Pow(x, 2));
                float y = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(x, 2) - Mathf.Abs(z));
                cameras[i] = Instantiate(sampleCamera, new Vector3(x, y, Mathf.Sqrt(Mathf.Abs(z)) * z / Mathf.Abs(z)), Quaternion.identity, cameraParent);
                cameras[i].transform.LookAt(Vector3.zero);
                cameras[i].GetComponent<Camera>().fieldOfView = fov;
            }
        }
    }
    public void GenerateButton()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        width = int.Parse(inResW.text);
        height = int.Parse(inResH.text);
        interval = float.Parse(inInter.text);
        fov = float.Parse(inFOV.text);
        isLF = tgisLF.isOn;
        Relocation();
        string path = string.Format(@"{0}\..\Sample\{1}", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
        Directory.CreateDirectory(path);
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture rTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, 0);
            for(int i=0; i<cameras.Length;i++)
            {
                cameras[i].GetComponent<Camera>().targetTexture=rTex;
                RenderTexture.active = rTex;
                cameras[i].GetComponent<Camera>().Render();
                //yield return new WaitForEndOfFrame();
                tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                tex.Apply();
                File.WriteAllBytes(string.Format(path + @"\{0}.png", i), tex.EncodeToPNG());
            }
        yield return null;
    }
    
    //public void SetButton()
    //{
    //    width = int.Parse(inResW.text);
    //    height = int.Parse(inResH.text);
    //    interval = float.Parse(inInter.text);
    //    fov = float.Parse(inFOV.text);
    //    isLF = tgisLF.enabled;
    //}
}
