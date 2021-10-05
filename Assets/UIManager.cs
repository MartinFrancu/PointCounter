using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public short leftPoints = 0;
    public List<GameObject> leftPointsFields;
    public List<GameObject> leftPointsHistoryFields;
    private List<short> leftPointHistory = new List<short>(); 

    public short rightPoints = 0;
    public List<GameObject> rightPointsFields;
    public List<GameObject> rightPointsHistoryFields;
    private List<short> rightPointHistory = new List<short>(); 

    public GameObject threeButtonsUI;
    public GameObject twoButtonsUI;
    public GameObject settingsUI;
    public GameObject controlUI;

    // Start is called before the first frame update
    void Start()
    {
        updateState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  SwitchTo3Buttons() {
        twoButtonsUI.SetActive(false);
        threeButtonsUI.SetActive(true);
        settingsUI.SetActive(false);
        controlUI.SetActive(true);
    }

    public void  SwitchTo2Buttons() {
        twoButtonsUI.SetActive(true);
        threeButtonsUI.SetActive(false);
        settingsUI.SetActive(false);
        controlUI.SetActive(true);        
    }

    public void  ToSettings() {
        twoButtonsUI.SetActive(false);
        threeButtonsUI.SetActive(false);
        settingsUI.SetActive(true);
        controlUI.SetActive(false);        
    }
    
    public void Add1PointLeft() {
        addPoints(true, 1);
    }    
    
    public void Add2PointLeft() {
        addPoints(true, 2);
    }    
    
    public void Add3PointLeft() {
        addPoints(true, 3);
    }

    public void Add1PointRight() {
        addPoints(false, 1);
    }    
    
    public void Add2PointRight() {
        addPoints(false, 2);
    }    
    
    public void Add3PointRight() {
        addPoints(false, 3);
    }

    private void addPoints( bool left, short howMany) {
        if(left) {
            leftPoints += howMany;
            leftPointHistory.Add(howMany);
        } else {
            rightPoints += howMany;
            rightPointHistory.Add(howMany);
        }

        updateState();
    }

    public void RestartCount() {
        leftPoints = 0;
        rightPoints = 0;
        leftPointHistory.Clear();
        rightPointHistory.Clear();
        updateState();
    }

    private string GetHistory(List<short> list) {
        string result = "";
        foreach(short value in list) {
            result += value + " ";
        }
        return result;
    }

    private void updateState() {
        foreach (GameObject field in leftPointsFields) {
            field.GetComponent<UnityEngine.UI.Text>().text = leftPoints.ToString();
        }
        foreach (GameObject field in leftPointsHistoryFields) {
            field.GetComponent<UnityEngine.UI.Text>().text = GetHistory(leftPointHistory);
        }
        foreach (GameObject field in rightPointsFields) {
            field.GetComponent<UnityEngine.UI.Text>().text = rightPoints.ToString();
        }
        foreach (GameObject field in rightPointsHistoryFields) {
            field.GetComponent<UnityEngine.UI.Text>().text = GetHistory(rightPointHistory);
        }
        Debug.Log("state: left: " + leftPoints + ", right: " + rightPoints +". history: " + leftPointHistory.Count + ", " + rightPointHistory.Count);
    }
}
