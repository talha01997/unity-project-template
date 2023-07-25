//Shady
using UnityEngine;

public class Character : MonoBehaviour, IPoolable
{
    //===================================================
    // FIELDS
    //===================================================
    [SerializeField] int _id = 0;

    //===================================================
    // PROPERTIES
    //===================================================
    public int ID => _id;

    //===================================================
    // METHODS
    //===================================================
    void IPoolable.Init()
    {
        Debug.Log("Init");
    }

    void IPoolable.Reset()
    {
        Debug.Log("Reset");
    }

    void IPoolable.Spawn()
    {
        Debug.Log("Spawn");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}//class end