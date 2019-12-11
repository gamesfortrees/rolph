using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBed : MonoBehaviour
{
    private GameObject seedling;

    // Start is called before the first frame update
    void Start()
    {
        seedling = gameObject.transform.GetChild(0).gameObject;
        seedling.SetActive(false);
    }

    public void SproutSeedling()
    {
        seedling.SetActive(true);
      }

    public void CutDown()
    {
        seedling.SetActive(false);
    }

    public bool HasSeedling()
    {
        return seedling.activeSelf;
    }
}
