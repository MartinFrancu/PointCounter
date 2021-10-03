using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UiMode
{
    Menu,
    Counting,
}

public class UIManager : MonoBehaviour
{
    public short leftPoints = 0;
    public GameObject leftPointsField;

    public short rightPoints = 0;
    public GameObject rightPointsField;

    
    private UiMode state = UiMode.Menu;

    // Start is called before the first frame update
    void Start()
    {
        updateState();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        } else {
            rightPoints += howMany;
        }

        updateState();
    }

    public void RestartCount() {
        leftPoints = 0;
        rightPoints = 0;
        updateState();
    }

    private void updateState() {
        leftPointsField.GetComponent<UnityEngine.UI.Text>().text = leftPoints.ToString();
        rightPointsField.GetComponent<UnityEngine.UI.Text>().text = rightPoints.ToString();
        Debug.Log("state: left: " + leftPoints + ", right: " + rightPoints +". State: " + state);
    }
}
