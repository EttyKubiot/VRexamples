using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;


public class MovmentRecognizer : MonoBehaviour
{
    private TouchScreenKeyboard keyboard;

    public XRNode inputSource;
    public InputHelpers.Button inputButton;
    public float inputThreshold = 0.1f;
    public Transform movementSource;
    public Transform movementSource1;

    public float newPositionThesholdDistance = 0.05f;
    public GameObject[] debugCubePrefab;
    public Material debugCubeMatirial;
    public Material debugCubeMatirial1;
    public Material debugCubeMatirial2;
    public int debugCubePrefabint = 0;
    public float forScale ;
    public bool creatoinMode = true;
    public string newGestureName;
    public Slider mySlider;
    public Slider scaleSlider;

    public float recognitoinThreshold = 0.1f;

    public float space;
    
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognize;

    private List<Gesture> traningSet = new List<Gesture>();
    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();
    

    // Start is called before the first frame update
    void Start()
    {
        ShowKeyboard();
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach(var item in gestureFiles)
        {
            traningSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPreesed, inputThreshold);

        if(!isMoving && isPreesed)
        {
            StartMovment();
           
        }
        else if(isMoving && !isPreesed)
        {
            EndMovment();
            
        }
        else if (isMoving && isPreesed)
        {
            UpdateMovment();
        }
    }

    void StartMovment()
    {
        isMoving = true;
        positionList.Clear();
        positionList.Add(movementSource1.position);
        if (debugCubePrefab[debugCubePrefabint])
        {
            /*Destroy(*/Instantiate(debugCubePrefab[debugCubePrefabint], movementSource1.position /*+ new Vector3(0.2f, 0, 0.2f)*/, Quaternion.identity)/*,3)*/;

            
        }
    }
    void EndMovment()
    {
        isMoving = false;

        Point[] pointArray = new Point[positionList.Count];

        for(int i = 0; i< positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }
        Gesture newgesture = new Gesture(pointArray);

        if (creatoinMode)
        {
            newgesture.Name = newGestureName;
            traningSet.Add(newgesture);

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newgesture, traningSet.ToArray());
            Debug.Log(result.GestureClass + result.Score);
            if(result.Score > recognitoinThreshold)
            {
                OnRecognize.Invoke(result.GestureClass);
            }
        }
    }
    void UpdateMovment()
    {
        Vector3 lastPosition = positionList[positionList.Count - 1];
        if (Vector3.Distance(movementSource1.position,lastPosition) > newPositionThesholdDistance)
        {
            positionList.Add(movementSource1.position);
            if (debugCubePrefab[debugCubePrefabint])
            {
                /*Destroy(*/Instantiate(debugCubePrefab[debugCubePrefabint], movementSource1.position /*+ new Vector3(0.2f, 0, 0.2f)*/, Quaternion.identity)/*, 3)*/;

            }
        }
    }

    public void ToglleActiveMode()
    {
        if (creatoinMode)
        {
            creatoinMode = false;
        }
        else
        {
            creatoinMode = true;

        }
        
    }

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        
    }
    public void Red()
    {
        //debugCubeMatirial.color = Color.red;
        debugCubePrefab[0].GetComponent<MeshRenderer>().material = debugCubeMatirial;
        debugCubePrefab[1].GetComponent<MeshRenderer>().material = debugCubeMatirial;
    }
    public void Blue()
    {
        //debugCubeMatirial.color = Color.blue;
        debugCubePrefab[0].GetComponent<MeshRenderer>().material = debugCubeMatirial1;
        debugCubePrefab[1].GetComponent<MeshRenderer>().material = debugCubeMatirial1;
    }
    public void Pink()
    {
        //debugCubeMatirial.color = Color.green;
        debugCubePrefab[0].GetComponent<MeshRenderer>().material = debugCubeMatirial2;
        debugCubePrefab[1].GetComponent<MeshRenderer>().material = debugCubeMatirial2;
    }
    public void Sircal()
    {
        debugCubePrefabint = 1;
    }
    public void cube()
    {
        debugCubePrefabint = 0;
    }
    public void SliderSpace()
    {
        newPositionThesholdDistance = mySlider.value;
    }
    public void SliderScale()
    {
        forScale = scaleSlider.value;

        debugCubePrefab[debugCubePrefabint].gameObject.transform.localScale = new Vector3(forScale, forScale, forScale);
    }
}
