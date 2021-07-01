using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEditor;

public class sixty : MonoBehaviour
{
    RecorderController TestRecorderController;
    // Start is called before the first frame update
    void Start()
    {
        
        float deg = 60.0F;
        float rad = deg * Mathf.Deg2Rad;
        transform.position = new Vector3(0, 5 * Mathf.Sin(rad), -5 * Mathf.Cos(rad));
        // transform.position = new Vector3(0, 10, -10);

        // Wait a while
    }
    
    public void StartRecord(string name)
    {
        string save_root = "Assets/Scenes/tmpsaveroot";
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        TestRecorderController = new RecorderController(controllerSettings);
        
        var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "My Video Recorder2";
        videoRecorder.Enabled = true;
        videoRecorder.VideoBitRateMode = VideoBitrateMode.High;
        
        videoRecorder.ImageInputSettings = new CameraInputSettings
        {
            Source = ImageSource.TaggedCamera,
            FlipFinalOutput = false, 
            CameraTag = "camera60",
            OutputWidth = 1920,
            OutputHeight = 1080
        };
        
        videoRecorder.AudioInputSettings.PreserveAudio = false;
        videoRecorder.OutputFile = save_root+"/"+name+"_60";
        //videoRecorder.OutputFile; // Change this to change the output file name (no extension)
        
        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.SetRecordModeToFrameInterval(0, 1440); // 2s @ 30 FPS
        controllerSettings.FrameRate = 480;
        
        RecorderOptions.VerboseMode = false;
        TestRecorderController.PrepareRecording();
        TestRecorderController.StartRecording();
    }    

    // Update is called once per frame
    void Update()
    {
        
    }
}
