using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool isTouched = false;

    public Material _defaultMaterial;
    public Material _touchedMaterial;
    // If the agent touches the obstacle, set isTouched to True
    // Return True if it is the first time that it is touched
    public bool touchThis()
    {
        if (!isTouched)
        {
            isTouched = true;
            
            // Change material for visualization
            gameObject.GetComponent<MeshRenderer>().material = _touchedMaterial;
            
            return true;
        }

        return false;
    }
    
    public bool getIsTouched()
    {
        return isTouched;
    }

    public void setIsTouched(bool isTouched)
    {
        if(!isTouched)
            // Change material for visualization
            gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;
        this.isTouched = isTouched;
    }
}
