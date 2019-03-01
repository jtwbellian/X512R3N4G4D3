using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class themeButton : MonoBehaviour
{
    //Colors 
    private static Color C_WHITE = new Color(1f, 1f, 1f, 1f);
    private static Color C_WOOD = new Color(0.7294f, 0.455f, 0.2275f, 1);

    // SAND_DOLLAR PALETTE

    private static Color C_SWIMMING = new Color(0.862745f, 0.921569f, 0.929412f, 1f);
    private static Color C_STRING = new Color(0.745098f, 0.721569f, 0.694118f, 1f);
    private static Color C_DRIFTWOOD = new Color(0.494118f, 0.462745f, 0.439216f, 1f);
    private static Color C_PARCHMENT = new Color(0.941176f, 0.933333f, 0.901961f, 1f);


    // TUSCAN_SUNSET_PALETTE

    private static Color C_SUMMERWHITE = new Color(0.921569f, 0.8f, 0.615686f, 1f);
    private static Color C_PAVILLION = new Color(0.831373f, 0.772549f, 0.709804f, 1f);
    private static Color C_WHEATPENNY = new Color(0.639216f, 0.470588f, 0.380392f, 1f);
    private static Color C_PALM = new Color(0.447059f, 0.419608f, 0.309804f, 1f);

    // incognito
    private static Color C_ZINC = new Color(0.796078f, 0.788235f, 0.772549f, 1f);
    private static Color C_NAVY = new Color(0.45098f, 0.54902f, 0.647059f, 1f);
    private static Color C_WOOL = new Color(0.427451f, 0.439216f, 0.454902f, 1f);
    private static Color C_GRAPHITE = new Color(0.309804f, 0.32549f, 0.317647f, 1f);

    // winery

    private static Color C_BLANC = new Color(0.956863f, 0.94902f, 0.94902f, 1f);
    private static Color C_WINE = new Color(0.592157f, 0.133333f, 0.137255f, 1f);
    private static Color C_ONYX = new Color(0.33333f, 0.301961f, 0.301961f, 1f);
    private static Color C_TOFFY = new Color(0.533333f, 0.427451f, 0.321569f, 1f);

    // Prestons theme 1
    private static Color C_TUSCAN_TAN = new Color(0.956863f, 0.94902f, 0.94902f, 1f);
    private static Color C_INDEPENDENCE = new Color(0.290196f, 0.270588f, 0.388235f, 1f);
    private static Color C_SLATE_GREY = new Color(0.431372f, 0.060310f, 0.580392f, 1f);
    private static Color C_CAMBRIDGE_BLUE = new Color(0.639215f, 0.768627f, 0.737254f, 1f);
    private static Color C_DARK_SEA_GREEN = new Color(.498039f, 0.760784f, 0.607843f, 1f);

    // Prestons theme 2
    private static Color C_DEEP_TUSCAN_RED = new Color(0.388235f, 0.278431f, 0.301960f, 1f);
    private static Color C_TURKISH_ROSE = new Color(0.666666f, 0.052328f, 0.486274f, 1f);
    private static Color C_TUMBLEWEED = new Color(0.839215f, 0.631372f, 0.517647f, 1f);
    private static Color C_VIVID_TANGERINE = new Color(1f, 0.650980f, 0.525490f, 1f);
    private static Color C_PEACH_ORANGE = new Color(0.996078f, 0.756862f, 0.588235f, 1f);


    // Material slots

    private const int WALL = 7;
    private const int ACCENTWALL = 5;
    private const int CEILING = 8;
    private const int TRIM1 = 10;
    private const int TRIM2 = 0;
    private const int FLOORB = 3;
    private const int FLOORA = 2;

    private const int CABINETFINISH = 4;

    private Image buttonImage;

    // For themes...
    // 0 =  trim color 
    // 1 = 
    // 3 =
    // 3 = 

    // Default White
    private Color[] theme0 = { C_WHITE, C_WHITE, C_WHITE, C_WHITE, C_WOOD };

    // Tuskan Sunset
    private Color[] theme1 = { C_SUMMERWHITE, C_WHEATPENNY, C_PAVILLION, C_PALM, C_WOOD };

    // Sand Dollar
    private Color[] theme2 = { C_PARCHMENT, C_SWIMMING, C_STRING, C_DRIFTWOOD, C_WOOD };

    // winery
    private Color[] theme3 = { C_BLANC, C_WINE, C_TOFFY, C_ONYX, C_WOOD };

    // Incognito
    private Color[] theme4 = { C_ZINC, C_NAVY, C_WOOL, C_GRAPHITE, C_WOOD };

    // Prestons Theme1
    private Color[] theme6 = { C_TUSCAN_TAN, C_INDEPENDENCE, C_SLATE_GREY, C_CAMBRIDGE_BLUE, C_DARK_SEA_GREEN };

    // Prestons Theme2
    private Color[] theme5 = { C_DEEP_TUSCAN_RED, C_TURKISH_ROSE, C_TUMBLEWEED, C_VIVID_TANGERINE, C_PEACH_ORANGE };

    GameObject floorplan;

    GameObject[] doors;
    GameObject[] blinds;
    GameObject[] accentObjects1;
    GameObject[] accentObjects2;


    MeshRenderer houseWalls; // the mesh renderer for the floorplan 
    Material[] mats; // materials slots on floorplan

    // Use this for initialization
    void OnEnable()
    {
        floorplan = GameObject.FindWithTag("Floorplan");
        houseWalls = floorplan.GetComponent<MeshRenderer>();

        buttonImage = transform.GetComponent<Image>();
        doors = GameObject.FindGameObjectsWithTag("door");
        blinds = GameObject.FindGameObjectsWithTag("blinds");
        accentObjects1 = GameObject.FindGameObjectsWithTag("Accent1");
        accentObjects2 = GameObject.FindGameObjectsWithTag("Accent2");

    }

    // Update is called once per frame
    void Update()
    {

        if (buttonImage != null)
        {
            if (buttonImage.color.r != 1f && buttonImage.color.r != 1f && buttonImage.color.r != 1f)
            {
                buttonImage.color = new Color(buttonImage.color.r + 0.1f, buttonImage.color.g + 0.1f, buttonImage.color.b + 0.1f, 1f);
            }
        }
    }

    void OnClick()
    {

        Color wallColor, ceilingColor, accentColor, trimColor, doorColor;
        Material floorMat1, floorMat2, finishMat, blindsMat;

        wallColor = ceilingColor = accentColor = trimColor = doorColor = new Color(1f, 1f, 1f, 1f);

        switch (this.gameObject.name)
        {

            // THEME BUTTONS

            // Basic White
            case "Theme0":
                {
                    wallColor = theme0[1];
                    accentColor = theme0[2];
                    ceilingColor = theme0[0];
                    trimColor = theme0[0];
                    doorColor = theme0[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }

            // Tuskan Sunset
            case "Theme1":
                {
                    wallColor = theme1[0];
                    accentColor = theme1[1];
                    ceilingColor = theme1[2];
                    trimColor = theme1[1];
                    doorColor = theme1[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }

            // Sand Dollar
            case "Theme2":
                {
                    wallColor = theme2[1];
                    accentColor = theme2[2];
                    ceilingColor = theme2[0];
                    trimColor = theme2[2];
                    doorColor = theme2[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }

            // winery
            case "Theme3":
                {
                    wallColor = theme3[0];
                    accentColor = theme3[1];
                    ceilingColor = theme3[0];
                    trimColor = theme3[2];
                    doorColor = theme3[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }
            // Incognito
            case "Theme4":
                {
                    wallColor = theme4[1];
                    accentColor = theme4[2];
                    ceilingColor = theme4[0];
                    trimColor = theme4[2];
                    doorColor = theme4[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }

            // Incognito
            case "Theme5":
                {
                    wallColor = theme5[3];
                    accentColor = theme5[1];
                    ceilingColor = theme5[0];
                    trimColor = theme5[2];
                    doorColor = theme5[0];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }
            // Incognito
            case "Theme6":
                {
                    wallColor = theme6[1];
                    accentColor = theme6[2];
                    ceilingColor = theme6[0];
                    trimColor = theme6[2];
                    doorColor = theme6[3];
                    UpdatePaintColors(houseWalls, wallColor, ceilingColor, accentColor, trimColor, doorColor);
                    break;
                }

            // ------------ END THEME BUTTONS ------------


            // Flooring Buttons 

            case "FloorA1":
                {
                    floorMat1 = (Material)Resources.Load("HomeAssets/HA_Materials/mat_woodFlooring1", typeof(Material));

                    mats = houseWalls.materials;
                    mats[FLOORA] = floorMat1;
                    houseWalls.materials = mats;
                    break;
                }
            case "FloorA2":
                {
                    floorMat1 = (Material)Resources.Load("HomeAssets/HA_Materials/mat_woodFlooring2", typeof(Material));

                    mats = houseWalls.materials;
                    mats[FLOORA] = floorMat1;
                    houseWalls.materials = mats;
                    break;
                }

            case "FloorB1":
                {
                    floorMat2 = (Material)Resources.Load("HomeAssets/HA_Materials/mat_tile1", typeof(Material));

                    mats = houseWalls.materials;
                    mats[FLOORB] = floorMat2;
                    houseWalls.materials = mats;
                    break;
                }
            case "FloorB2":
                {
                    floorMat2 = (Material)Resources.Load("HomeAssets/HA_Materials/mat_tile2", typeof(Material));

                    mats = houseWalls.materials;
                    mats[FLOORB] = floorMat2;
                    houseWalls.materials = mats;
                    break;
                }

            // ------------ END FLOORING BUTTONS ------------


            // Finish Buttons 

            case "finishWood":
                finishMat = (Material)Resources.Load("HomeAssets/HA_Materials/mat_hardwood", typeof(Material));
                blindsMat = (Material)Resources.Load("HomeAssets/HA_Materials/mat_blinds", typeof(Material));

                mats = houseWalls.materials;
                mats[CABINETFINISH] = finishMat;
                houseWalls.materials = mats;

                foreach (GameObject b in blinds)
                {
                    b.GetComponent<MeshRenderer>().material = blindsMat;
                }

                break;
            case "finishWhite":
                finishMat = (Material)Resources.Load("HomeAssets/HA_Materials/mat_white", typeof(Material));
                blindsMat = (Material)Resources.Load("HomeAssets/HA_Materials/mat_blindsWhite", typeof(Material));

                mats = houseWalls.materials;
                mats[CABINETFINISH] = finishMat;
                houseWalls.materials = mats;

                foreach (GameObject b in blinds)
                {
                    b.GetComponent<MeshRenderer>().material = blindsMat;
                }
                break;


            // ------------ END FINISH BUTTONS ------------



            default:
                {
                    break;
                }
        }




        // Update the pallete 
        /* foreach (Image i in palletIcons)
         {
             switch (i.name)
             {
                 case "Walls":    
                     i.color = wallColor;
                     break;
                 case "Trim":
                     i.color = trimColor;
                     break;
                 case "Floor":
                     i.color = floorColor;
                     break;

                 case "Ceiling":
                     i.color = ceilingColor;
                     break;

                 case "Door":
                     i.color = doorColor;
                     break;

                 default:
                     i.color = new Color(1, 1, 1, 1);
                     break;
             }
         }*/

    }


    private void UpdatePaintColors(MeshRenderer renderer, Color wallColor, Color ceilingColor, Color accentColor, Color trimColor, Color doorColor)
    {
        renderer.materials[WALL].SetColor("_Color", wallColor);
        renderer.materials[ACCENTWALL].SetColor("_Color", accentColor);
        renderer.materials[CEILING].SetColor("_Color", ceilingColor);

        renderer.materials[TRIM1].SetColor("_Color", trimColor);
        renderer.materials[TRIM2].SetColor("_Color", trimColor);


        foreach (GameObject d in doors)
        {
            d.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", doorColor);
        }

        foreach (GameObject a1 in accentObjects1)
        {
            a1.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", accentColor);
        }

        foreach (GameObject a2 in accentObjects2)
        {
            a2.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", doorColor);
        }
    }
}
