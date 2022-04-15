using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GMScript : MonoBehaviour   // add the scoreboard
{
    public float speed = 1000f;

    // UNIQUE CUBE CREATE ** MUST FIX
    public string tempUniqStr;

    // GAME STATUS
    public bool IsGameOver;

    // CALCULATE THE SCORE
    public int score = 0;
    public int highScore = 0;
    public string highScoreKey = "HighScore";

    public string UniqueCubeStopTag;

    public static GMScript Instance;

    public static int uniqueID;

    // cubeproperties Class
    [Serializable]
    public class CubeProperties
    {
        public string cubeName;
        public Color cubeColor;
        public CubeType cubeType;
        public int cubeNumber;
    }

    private void Awake()
    {
        uniqueID = 0;
        Instance = this;
    }

    // particle
    public GameObject particlePrefab;

    // cubetype enum
    public enum CubeType
    {
        Two,
        Four,
        Eight,
        SixTeen,
        ThirtyTwo,
        SixtyFour,
        OneHundredAndTwentyEight,
        TwoHundredAndFiftySix,
        FiveHundredTwelve,
        OneThousandTwentyFour,
        TwoThousandFortyEight,
        FourThousandNinetySix,
        Unique  // unique cube
    }

    // random color List
    public List<CubeProperties> defaultCubes = new List<CubeProperties>();

    //uniqueCubeList
    public List<CubeProperties> uniqueCubes = new List<CubeProperties>();

    // cubes
    public GameObject cubePrefab;
    public GameObject cubeParent;

    public Transform cubeSpawnTransform;

    public GameObject cube;

    // drag cube NECESSARY
    private float limPos = 2.1f;        // MAX MIN VALUE
    public Vector3 preMousePos;
    public Vector3 currentMousePos;
    public Vector3 deltaMousePos;

    private void Start()
    {
        // SAVE THE HIGH SCORE
        if (!PlayerPrefs.HasKey(highScoreKey))
        {
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }
        else
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);
        }

        highScoreController();


        // STEP 1 -> CREATE THE CUBE
        //if (cube == null)
        //{
        //    StartCoroutine(CubeGenerator());
        //}
    }

    private void Update()
    {
        // STEP 2 -> SET THE CUBE POS ON MOUSE POS
        if (Input.GetMouseButtonDown(0) && cube != null)
        {
            currentMousePos = Input.mousePosition;
        }

        // STEP 3 -> SYNCH CUBE MOVEMENT WITH THE MOUSE MOVEMENT
        if (Input.GetMouseButton(0) && cube != null)
        {
            DragDefaultCube();
        }

        // STEP 4 -> THROW THE CUBE
        if (Input.GetMouseButtonUp(0) && cube != null)
        {
            //ThrowTheDefaultCube();
        }
    }

    // DRAG THE CUBE WITH MOUSE
    private void DragDefaultCube()
    {
        // CUBE MOVEMENT

        preMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;
        deltaMousePos = currentMousePos - preMousePos;
        //Bar.transform.position = new Vector3(Mathf.Clamp(Bar.transform.position.x, xMinLimit - 1, xMaxLimit + 1),
        //Bar.transform.position.y, Bar.transform.position.z);
        cube.transform.position += new Vector3(Mathf.Clamp(deltaMousePos.x, -limPos, limPos) * 2, 0, 0) * Time.fixedDeltaTime;

    }

    // THROW THE CUBE
    public IEnumerator ThrowTheDefaultCube()
    {
        cube.GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        LevelFailController.Instance.Timer = 0.2f;
        // FIX IT
        yield return new WaitForSeconds(0.1f);
        cube.GetComponent<Cubes>().IsMainCube = false;
        //StartCoroutine(SetCubeMainFalse(cube));
        cube = null;
        StartCoroutine(CubeGenerator());
    }

    // MAINCUBE CONTROLLER
    //public IEnumerator SetCubeMainFalse(GameObject cube)
    //{
    //    if (!cube)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        cube.GetComponent<Cubes>().IsMainCube = false;
    //    }
    //}

    // CUBE SPAWN TIME
    private IEnumerator CubeGenerator()
    {
        yield return new WaitForSeconds(1.5f);

        DefaultCubeGenerator(cubeSpawnTransform.position);
    }

    // CREATE THE DEFAULT CUBE
    public void DefaultCubeGenerator(Vector3 spawnPos)
    {
        // create cube
        cube = Instantiate(cubePrefab);
        // level fail controller
        cube.GetComponent<Cubes>().IsMainCube = true;
        cube.transform.position = spawnPos;
        // set the color on the cube material ** bir liste olduğu için değer indeksleri eşittir
        SetCube(cube, RandomDefaultCubeGenerator());
    }

    // CREATE A RANDOM DEFAULT CUBE
    private CubeProperties RandomDefaultCubeGenerator()
    {
        System.Random random = new System.Random();
        return defaultCubes[random.Next(3)];
    }

    // COLLISION CUBE CREATOR
    public void CollisionCubeGenerator(Vector3 spawnPos, int typeIndex)
    {
        // particle
        StartCoroutine(CreateParticle(spawnPos));
        // create cube
        GameObject tempCube = Instantiate(cubePrefab);
        tempCube.transform.position = spawnPos;
        // set the color on the cube material ** bir liste olduğu için değer indeksleri eşittir
        SetCube(tempCube, CollisionCheckNewCubeGenerator(typeIndex + 1));   // nextType cube create
    }

    // RETURN THE COLLISION CUBE WHICH ONE
    private CubeProperties CollisionCheckNewCubeGenerator(int indexType)
    {
        for (int i = 0; i < defaultCubes.Count; i++)
        {
            if (defaultCubes[i].cubeType == (CubeType)indexType)
            {
                return defaultCubes[i];
            }
        }
        return defaultCubes[0];
    }

    // SET CUBE PROPERTIES
    public void SetCube(GameObject cube, CubeProperties defaultCube)
    {
        //UNIQID UPDATE
        //public string cubeName;
        //public Color cubeColor;
        //public CubeType cubeType;
        //public string cubeNumber;
        uniqueID++;

        cube.GetComponent<Cubes>().cubePro.cubeName = defaultCube.cubeName;
        // SET THE COLOR ON CUBE MATERIAL COLOR
        cube.GetComponent<Cubes>().cubePro.cubeColor = defaultCube.cubeColor;
        //cube.GetComponent<MeshRenderer>().material.color = defaultCube.cubeColor;
        // NEW MATERIAL CHANGE COLOR
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultCube.cubeColor);
        // SET THE CUBE TYPE
        cube.GetComponent<Cubes>().cubePro.cubeType = defaultCube.cubeType;
        // SET THE NUMBER ON CUBE TEXT
        cube.GetComponent<Cubes>().setText(defaultCube.cubeNumber.ToString());
        cube.GetComponent<Cubes>().cubePro.cubeNumber = defaultCube.cubeNumber;
        // UPDATE THE UNIQID
        cube.GetComponent<Cubes>().CubesUniqueID = uniqueID;

        // SET THE PARENT
        cube.transform.SetParent(cubeParent.transform);


    }

    // COLLISION PARTICLE
    public IEnumerator CreateParticle(Vector3 spawnPos)
    {
        //create particle
        GameObject instantiatedParticle = Instantiate(particlePrefab);

        instantiatedParticle.transform.position = spawnPos;

        yield return null;

    }

    // CREATE GAME OVER ONLEVELFINSHED AND GAMERESTART METHODS btns
    public void gameRestart()
    {
        //tryAgain(); SANTA
        score = 0;
        highScoreController();
        IsGameOver = false;
        foreach (Transform item in cubeParent.transform)
        {
            Destroy(item.gameObject);
        }
        StartCoroutine(CubeGenerator());
    }

    // HIGH SCORE CONTROLLER
    public void highScoreController()
    {
        if (highScore < score)
        {
            highScore = score;
        }
        if (highScore > PlayerPrefs.GetInt(highScoreKey))
        {
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }
        // SET THE SCORE
        UIManager.Instance.scoreText.text = "SCORE: " + score;
        UIManager.Instance.highScoreText.text = "HIGH SCORE: " + highScore;
        UIManager.Instance.finalScoreText.text = "" + score;

    }

    // diactive level fail, active gameplay
    //public void tryAgain()
    //{
    //    UIManager.Instance.UILevelFailed.SetActive(false);
    //    UIManager.Instance.UIGamePlay.SetActive(true);
    //}

    public IEnumerator lostStage()
    {
        highScoreController();

        UIManager.Instance.ChangeUI(UIManager.UIElements.UILevelFail);

        yield return new WaitForSeconds(3f);
        UIManager.Instance.ChangeUI(UIManager.UIElements.UIMainMenu);
        // maybe this solution
        LevelFailController.Instance.Timer = 0f;

    }

    // uniqueCubeCreator **adGoogle
    public void uniqueCubeGenerator()
    {
        // theCube transform to unique cube addOnClick
        setUniqueCube();

    }

    // Set the UniqueCube
    public void setUniqueCube()
    {
        /** Unique Cube List*/
        if (cube != null)
        {
            System.Random random = new System.Random();
            CubeProperties tempUniqProperties = uniqueCubes[random.Next(2)];

            Debug.Log("Set Unique: " + cube.GetComponent<Cubes>().IsMainCube);

            if (tempUniqProperties.cubeName == "FlameCube")
            {
                tempUniqStr = "X";
            }
            if (tempUniqProperties.cubeName == "BlackCube")
            {
                tempUniqStr = "?";
            }

            setMaterialUniqCube(tempUniqProperties);
        }

    }

    public void setMaterialUniqCube(CubeProperties tempUniqProperties)
    {

        cube.GetComponent<Cubes>().cubePro.cubeName = tempUniqProperties.cubeName;
        // SET THE COLOR ON CUBE MATERIAL COLOR
        cube.GetComponent<Cubes>().cubePro.cubeColor = tempUniqProperties.cubeColor;
        cube.GetComponent<MeshRenderer>().material.color = tempUniqProperties.cubeColor;
        // SET THE CUBE TYPE
        cube.GetComponent<Cubes>().cubePro.cubeType = tempUniqProperties.cubeType;
        // SET THE NUMBER ON CUBE TEXT
        cube.GetComponent<Cubes>().setText(tempUniqStr);
        // SET THE CUBE NUMBER COLOR
        for (int i = 0; i < cube.GetComponent<Cubes>().textList.Count; i++)
        {
            cube.GetComponent<Cubes>().textList[i].color = Color.white;
        }
        // SET THE CUBE NUMBER
        cube.GetComponent<Cubes>().cubePro.cubeNumber = tempUniqProperties.cubeNumber;

        // SET THE PARENT
        cube.transform.SetParent(cubeParent.transform);
    }
}
