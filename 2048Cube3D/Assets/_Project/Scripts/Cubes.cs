using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cubes : MonoBehaviour  // add the particle
{

    /**     ## Unique cube neye çarparsa çarpsın destroy ##     */


    public bool IsMainCube; // level fail için gerekli
    public int CubesUniqueID;                                       // for different objects
    public GMScript.CubeProperties cubePro;
    public List<TextMeshPro> textList = new List<TextMeshPro>();
    // control the collision        ** DONE
    // control the textmeshpro text ** DONE 

    // SET THE NUMBER
    public void setText(string textNumber)
    {
        for (int i = 0; i < textList.Count; i++)
        {
            textList[i].text = textNumber;
        }
    }

    public void Update()
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0) && cubePro.cubeType == GMScript.CubeType.Unique && IsMainCube==false)
        {
            StartCoroutine(GMScript.Instance.CreateParticle(this.gameObject.transform.position));
            Debug.Log("unique destroy:"+this.IsMainCube);
            Destroy(this.gameObject);
        }
    }

    // onCollider ** FIX IT+
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == GMScript.Instance.UniqueCubeStopTag && cubePro.cubeType == GMScript.CubeType.Unique)
        {
            // create the particle
            StartCoroutine(GMScript.Instance.CreateParticle(collision.transform.position));
            // Goodbye uniq
            Destroy(this.gameObject);
        }
       
        // otherObject cubes
        if (collision.gameObject.GetComponent<Cubes>())
        {
            if (cubePro.cubeType == GMScript.CubeType.Unique)
            {
                CalculateTheScore(collision.gameObject.GetComponent<Cubes>().cubePro.cubeNumber);
                // create the particle
                StartCoroutine(GMScript.Instance.CreateParticle(collision.transform.position));

                Destroy(collision.gameObject);
            }



            // Cubetype==Cubetype
            if (collision.gameObject.GetComponent<Cubes>().cubePro.cubeType == cubePro.cubeType)
            {
                // must know which objects script
                if (collision.gameObject.GetComponent<Cubes>().CubesUniqueID<CubesUniqueID)
                {
                    // calculate the score
                    CalculateTheScore(collision.gameObject.GetComponent<Cubes>().cubePro.cubeNumber);

                    int indexType = (int)cubePro.cubeType;
                    GMScript.Instance.CollisionCubeGenerator(collision.gameObject.transform.position + new Vector3(0,2f,0), indexType);
                    Destroy(collision.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void CalculateTheScore(int cubeNumber)
    {
        if (cubeNumber!=1)
        {
            GMScript.Instance.score += cubeNumber * 2;
            GMScript.Instance.highScoreController();
        }
        else
        {
            GMScript.Instance.score += cubePro.cubeNumber;
            GMScript.Instance.highScoreController();
        }
    }
}
