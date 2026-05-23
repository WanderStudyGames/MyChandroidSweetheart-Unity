using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class LevelBuildingTools : EditorWindow
{
    //misc
    private static bool singleOpen = true;
    private GUIStyle styleM;
    private string msg = "<size=18><color=navy>Welcome to Level Building Tools!</color></size>";  //initial msg
  //  private Color msgCol = Color.black;
    private Vector2 scrollPos;
    private bool expandScale = false;
    private bool expandRotate = false;
    private bool expandSpread = false;
    private bool expandProject = false;
    private bool expandStack = false;
    private bool expandPlace = false;
    string previous = "0000";
    int repId = 0;
    private string helpText = "<color=grey>Tip: Use Ctrl + Z to UNDO actions</color>";
    private static Texture[] uiTex; //container for UI images
    private static EditorWindow toolWindowRef;

    //randomRot
    private bool r_doXRot = false;
    private bool r_doYRot = true;
    private bool r_doZRot = false;
    private float r_rotAngle = 45f;
    private int r_rotateCount = 1;
    private bool r_rotateLocal = false;
    private bool r_additiveRotation = false;
    private Directions r_rotDir = Directions.Both;

    //sCale
    private float c_minScale = 0.5f;
    private float c_maxScale = 1.5f;
    private static AnimationCurve c_scaleDistribution = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    private bool c_keepOriginalScale = true;
    private bool c_scaleX = true;
    private bool c_scaleY = true;
    private bool c_scaleZ = true;

    //randomSpread
    private bool s_xSpread = true;
    private bool s_ySpread = false;
    private bool s_zSpread = true;
    private bool s_avgSel = true;
    private bool s_additiveSpread = false;
    private float s_spreadDistMin = 0.25f;
    private float s_spreadDistMax = 1.5f;
    private Directions s_spreadDir = Directions.Both;

    //Project
    private ProjectionType p_projType = ProjectionType.Direction;
    private Collider p_surfaceColl;
    private Sides p_projectDirE = Sides.Ynegative;
    private Sides p_alignAxis = Sides.Ypositive;
    private bool p_rotateToSurf = true;
    private bool p_parentToHitSurf = false;
    private float p_projectOffset = 0f;
    private bool p_useMeshBottom = false;
    private string p_useMeshBottomTooltip = "Whether to use the prefab's visual bottom for projection instead of its pivot";

    //sTack
    private Sides t_stackType;
    private float t_stackOffset = 0f;

    //pLace
    private List<Object> l_objectsToPlace = new List<Object>();
    private Transform l_placeParent;
    private bool l_rotateToSurf = true;
    private Sides l_alignAxis = Sides.Ypositive;
    private float l_placeOffset = 0f;

    [MenuItem("Tools/Level Building Tools/Open Main Window &g")]
    public static void Init()
    {
        singleOpen = true;

        uiTex = Resources.LoadAll("PropTransformImages/", typeof(Texture2D)).Cast<Texture2D>().ToArray();

        EditorWindow w = GetWindow<LevelBuildingTools>();
        toolWindowRef = w;

        w.titleContent = new GUIContent("Level Toolbox");
        if (singleOpen)
        {
            w.minSize = new Vector2(444f, 210f);
            w.maxSize = new Vector2(1000f, 210);
        }
        else
        {
            w.minSize = new Vector2(458, 300f);
            w.maxSize = new Vector3(1000f, 1000f);
        }
        //  Debug.Log(w.position.width);
        //  Debug.Log(w.position.height);

        c_scaleDistribution = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        AnimationUtility.SetKeyRightTangentMode(c_scaleDistribution, 0, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyLeftTangentMode(c_scaleDistribution, 1, AnimationUtility.TangentMode.Linear);
    }

    private void Awake()
    {
        c_scaleDistribution = GetCurve(1);
    }

    private void OnGUI()
    {
        //styles for proper alignment and looks
        styleM = new GUIStyle();
        styleM.richText = true;
        styleM.alignment = TextAnchor.MiddleCenter;
        styleM.wordWrap = true;
        styleM.fontStyle = FontStyle.Bold;

        //set-up
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUIUtility.wideMode = false;
        EditorGUIUtility.labelWidth = 110f;
        GUI.skin.textArea.alignment = TextAnchor.MiddleCenter;

        //status
        //   EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label(msg, styleM);
        // EditorGUILayout.EndHorizontal();

        if (uiTex == null)
            uiTex = uiTex = Resources.LoadAll("PropTransformImages/", typeof(Texture2D)).Cast<Texture2D>().ToArray();

        //---------------Main buttons-------------------
        // EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Space(5f);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent(GetTexture(0, expandRotate), "Randomize the rotation of selected objects"), styleM))
            ToggleUI(1);
        //scale text
        if (GUILayout.Button(new GUIContent(GetTexture(1, expandScale), "Randomize the scale of selected objects"), styleM))
            ToggleUI(2);

        //spread text
        if (GUILayout.Button(new GUIContent(GetTexture(2, expandSpread), "Randomize the position of the selected objects"), styleM))
            ToggleUI(3);

        //stack text
        if (GUILayout.Button(new GUIContent(GetTexture(3, expandStack), "Stack the selected objects on an axis, cancelling their overlaps"), styleM))
            ToggleUI(4);

        if (GUILayout.Button(new GUIContent(GetTexture(4, expandProject), "Move the selected objects either in a direction or on to a specific surface"), styleM))
            ToggleUI(5);

        //place text
        if (GUILayout.Button(new GUIContent(GetTexture(5, expandPlace), "Store selected objects and then randomly clone them onto a surface by pressing G"), styleM))
            ToggleUI(6);

        EditorGUILayout.EndHorizontal();



        //--------------Layouts--------------
        if (expandRotate)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>Randomize Rotation</size>", "Randomize the rotation of selected objects"), styleM);
            EditorGUILayout.EndHorizontal();
            RotationLayout();
        }

        if (expandScale)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>Randomize Scale</size>", "Randomize the scale of selected objects"), styleM);
            EditorGUILayout.EndHorizontal();
            ScaleLayout();
        }

        if (expandSpread)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>Randomize Position</size>", "Randomize the position of the selected objects"), styleM);
            EditorGUILayout.EndHorizontal();
            SpreadLayout();
        }

        if (expandStack)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>Stack Objects</size>", "Stack the selected objects on an axis, cancelling their overlaps"), styleM);
            GUILayout.EndHorizontal();
            StackLayout();
        }

        if (expandProject)
        {
            //project text
            //extra text for showing which projection mode is used
            string projectText = "Project to ";
            switch (p_projType)
            {
                case ProjectionType.Direction:
                    projectText += "Direction";
                    break;

                case ProjectionType.Surface:
                    projectText += "Surface";
                    break;
            }
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>" + projectText + "</size>", "Move the selected objects either in a direction or on to a specific surface"), styleM);
            EditorGUILayout.EndHorizontal();
            ProjectLayout();
        }

        if (expandPlace)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(new GUIContent("<size=14>Clone and Place</size>", "Store selected objects and then randomly clone them onto a surface by pressing G"), styleM);
            EditorGUILayout.EndHorizontal();
            PlaceLayout();
        }

        EditorGUILayout.EndScrollView();
        //misc
        GUILayout.BeginHorizontal();

        string t;
        if (singleOpen)
            t = "s";
        else
            t = "m";

        if (GUILayout.Button(new GUIContent(t, "Toggle whether or not you can open single or multiple tools at the same time"), styleM, GUILayout.Width(15f)))
            SwitchExpand();

        GUILayout.Label(helpText, styleM);
        GUILayout.EndHorizontal();

        /*
        int i = 0;
             foreach (Texture a in uiTex)
            {
                Debug.Log(i + "  " + a.name);
                i++;
            }
            */
    }

    [MenuItem("Tools/Level Building Tools/Group Selected %g")]
    private static void GroupSelected()
    {
        Transform[] selObj = Selection.transforms;

        if (selObj.Length > 0f)
        {
            Undo.RecordObjects(selObj, "Group Objects");
            int i = 1;  //starting number to check
            Transform newParent = null;

            while (newParent == null)   //search while there is no parent created
            {
                if (GameObject.Find("PropGroup" + i))   //attempt to find PropGroup object
                    i++;
                else
                    newParent = new GameObject("PropGroup" + i).transform;  //if it doesnt exist, make the object and use it
            }
            foreach (Transform t in selObj)
            {
                Undo.SetTransformParent(t, newParent, "Parent to Group");
                //t.transform.SetParent(newParent, true);
            }
        }
    }

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.keyCode == KeyCode.G && e.type == EventType.KeyUp)
        {
             Debug.Log("trying project");
           
            RaycastHit hit;
            /*
            Vector2 mousePosition = Event.current.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
            mousePosition.y = -mousePosition.y;
            mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            */
            //  mousePosition.y = -mousePosition.y;
            //   Ra position = sceneView.camera.ScreenPointToRay(mousePos);

            // fakeMP.z = 1f;
            //    Vector3 point = sceneView.camera.ScreenToWorldPoint(fakeMP);
            //   Vector3 normal = Vector3.forward;

            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
            Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
            /*
            Debug.DrawRay(ray.origin, ray.direction * 25f, Color.yellow, 5f);
            Debug.DrawRay(ray.origin, SceneView.currentDrawingSceneView.camera.transform.forward * 25f, Color.red, 5f);
            Debug.DrawRay(ray.origin, SceneView.currentDrawingSceneView.camera.transform.up, Color.blue, 5f);
            Debug.DrawRay(ray.origin, SceneView.currentDrawingSceneView.camera.transform.right, Color.blue, 5f);
            Debug.DrawRay(ray.origin, -SceneView.currentDrawingSceneView.camera.transform.up, Color.blue, 5f);
            Debug.DrawRay(ray.origin, -SceneView.currentDrawingSceneView.camera.transform.right, Color.blue, 5f);
            */
            if (Physics.Raycast(ray, out hit))
            {
              
                DoPlace(hit);
                //Debug.Log("mousepos: " + mousePosition + ", fakeMP: " + fakeMP + ", point: " + point + ", hitpoint: " +hit.point);
            }
            else
            {
                SetMsg("No surface with collider found at mouse position", "maroon");
                toolWindowRef.Repaint();
            }
            e.Use();
        }
    }

    #region rotate

    /// <summary>handles laying out the UI for rotation</summary>
    private void RotationLayout()
    {
        GUILayout.BeginHorizontal();
        //set rotational axes
        r_doXRot = GUILayout.Toggle(r_doXRot, new GUIContent("RotateX", "Rotate around the X (red) axis, causing the object to pitch"));
        r_doYRot = GUILayout.Toggle(r_doYRot, new GUIContent("RotateY", "Rotate around the Y (green) axis, causing the object spin around (yaw)"));
        r_doZRot = GUILayout.Toggle(r_doZRot, new GUIContent("RotateZ", "Rotate around the Z (blue) axis, causing the object to bank (roll)"));
        //set rotation space
        r_rotateLocal = GUILayout.Toggle(r_rotateLocal, new GUIContent("Local", "Check this if you want to object to be rotated in local space"));
        r_additiveRotation = GUILayout.Toggle(r_additiveRotation, new GUIContent("Additive", "Rotations add up to each other, can be used to make spirals (only rotate in one direction for that, only works on objects that are aligned on the Y axis) READ COMMENTS FOR CUSTOMIZATION (DoRotation method)"));
        GUILayout.EndHorizontal();

        //set rotation values
        r_rotAngle = EditorGUILayout.Slider(new GUIContent("Rotation Degrees", "What rotations the object should snap to; determines how 'fine' the rotations are."), r_rotAngle, 1, 180);
        r_rotateCount = Mathf.RoundToInt(EditorGUILayout.Slider(new GUIContent("Rotation Count", "How many times the object should randomly rotate"), r_rotateCount, 1f, 30f));

        GUILayout.BeginHorizontal();
        //set rotation direction
        GUILayout.Label(new GUIContent("Direction", "Direction in which you want to rotate"), GUILayout.Width(60f));
        r_rotDir = (Directions)EditorGUILayout.EnumPopup(new GUIContent("", "Direction in which you want to rotate"), r_rotDir, GUILayout.Width(75f));
        if (GUILayout.Button(new GUIContent("Reset Rotation", "Reset the object's rotation; If Local is checked, it rotates to the parent object's rotation"), GUILayout.Width(120f)))
            ResetRotation(true);
        //init rotate
        if (GUILayout.Button(new GUIContent("Rotate!", "Rotate selected objects randomly")))
            DoRotation();

        GUILayout.EndHorizontal();
    }

    /// <summary>handles creating and applying random rotations on specific axes</summary>
    private void DoRotation()
    {
        Transform[] selObj = Selection.transforms; //selection objects

        //if we have something selected
        if (selObj.Length > 0)
        {
            Undo.RecordObjects(selObj, "Rotate Objects");

            //set local/world space
            Space space = Space.World;
            if (r_rotateLocal)
                space = Space.Self;

            //Unity doesn't store the selection order in Selection.transforms sadly, even thought there is no reason not to; it just returns randomly ordered transforms.
            //So i had to hack this quick sort into the script. It's not the best solution but i can't really come up with anything else.
            //https://feedback.unity3d.com/suggestions/selection-order
            //It's been a suggested feature since 2012, which makes it a bit weird that it hasn't been implemented yet, especially since programs like Maya make use of selection order extensively.
            //
            //Use go.transform.name for name sorting, although it does break when it breaks from  "name(9)" to (name(10)+)
            //You can also just change the sorting axis.
            //
            //Create array and sort it based on Y position.
            List<Transform> sortList = selObj.ToList();
            if (selObj.Length >= 2)
            {
                sortList = sortList.OrderBy(go => go.position.y).ToList();
                //foreach (Transform t in sortList){Debug.Log(t.name);}  //test your sort if necessary
            }

            //storage for additive rotation mode
            Vector3 totalRot = Vector3.zero;

            foreach (Transform t in sortList)
            {
                //pick random rotation amount
                int xCount = Random.Range(1, r_rotateCount);
                int yCount = Random.Range(1, r_rotateCount);
                int zCount = Random.Range(1, r_rotateCount);

                //Create vector3 of random directions based on settings
                Vector3 dir;
                switch (r_rotDir)
                {
                    case Directions.Positive:
                        dir = Vector3.one;
                        break;

                    case Directions.Negative:
                        dir = Vector3.one * -1f;
                        break;

                    case Directions.Both:
                        dir = new Vector3(RandomOneOneMinus(), RandomOneOneMinus(), RandomOneOneMinus());
                        break;

                    default:
                        dir = Vector3.one;
                        break;
                }

                //calculate rotations
                float x = (r_rotAngle * xCount * dir.x) + totalRot.x;
                float y = (r_rotAngle * yCount * dir.y) + totalRot.y;
                float z = (r_rotAngle * zCount * dir.z) + totalRot.z;

                Quaternion preRot = t.rotation;

                //apply rotations
                if (r_doXRot)
                    t.Rotate(Vector3.right, x, space);
                if (r_doYRot)
                    t.Rotate(Vector3.up, y, space);
                if (r_doZRot)
                    t.Rotate(Vector3.forward, z, space);

                //add to total for additive mode
                if (r_additiveRotation)
                {
                    totalRot.x += r_rotAngle * xCount * dir.x;
                    totalRot.y += r_rotAngle * yCount * dir.y;
                    totalRot.z += r_rotAngle * zCount * dir.z;
                }

                Debug.DrawRay(t.position, t.right * 0.75f, Color.white, 3f, false); //preX
                Debug.DrawRay(t.position, t.up * 0.75f, Color.white, 3f, false);    //preY
                Debug.DrawRay(t.position, t.forward * 0.75f, Color.white, 3f, false); //preZ
                Debug.DrawRay(t.position, preRot * Vector3.right * 0.75f, Color.gray, 3f, false);   //postX
                Debug.DrawRay(t.position, preRot * Vector3.up * 0.75f, Color.gray, 3f, false);  //postY
                Debug.DrawRay(t.position, preRot * Vector3.forward * 0.75f, Color.gray, 3f, false); //postZ
                Debug.DrawLine(t.position + t.right * 0.75f, t.position + (preRot * Vector3.right) * 0.75f, Color.red, 3f); //X
                Debug.DrawLine(t.position + t.up * 0.75f, t.position + (preRot * Vector3.up) * 0.75f, Color.green, 3f);//Y
                Debug.DrawLine(t.position + t.forward * 0.75f, t.position + (preRot * Vector3.forward) * 0.75f, Color.blue, 3f);//Z
                                                                                                                                //Debug.Log("angle " + r_rotAngle+ "| count: " + yCount + "| dir.y: " + dir.y+ "| totalrot " +totalRot.y);
            }

            if (!r_doXRot && !r_doYRot && !r_doZRot)
                SetMsg("ERROR: No rotation axes selected!", "maroon");
            else
                SetMsg("Rotated " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: Nothing selected to rotate", "maroon");
        }
    }

    /// <summary>handles resetting rotations locally and globally</summary>
    private void ResetRotation(bool useHist)
    {
        Transform[] selObj = Selection.transforms;

        if (selObj.Length > 0)
        {
            if (useHist)
                Undo.RecordObjects(selObj, "Reset Rotation");

            foreach (Transform t in selObj)
            {
                if (r_rotateLocal)
                    t.localRotation = Quaternion.identity;
                else
                    t.rotation = Quaternion.identity;
            }

            SetMsg("Reset " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: Nothing selected!", "maroon");
        }
    }

    #endregion rotate

    #region scale

    /// <summary>Handles creating the UI for Scale</summary>
    private void ScaleLayout()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Distribution Weight", "Determines the distribution of the random scale. Play around with the curve, it becomes apparent quickly what it does. If it's '1' on the vertical axis, it's always going to be maxscale, if it's '0.1', it' always going to be 1/10 between min and max, if it's a line from 0.0 to 1.0, it's going to be equally distributed"), GUILayout.Width(112f));
        c_scaleDistribution = EditorGUILayout.CurveField(c_scaleDistribution, Color.yellow, new Rect(new Vector2(0f, 0f), new Vector2(1f, 1f)), GUILayout.Width(50f));
        if (GUILayout.Button(new GUIContent("1", "Use Linear Distribution; Objects will have similar chances to be anywhere between min and max scale."), GUILayout.Width(18f)))
            c_scaleDistribution = GetCurve(1);

        if (GUILayout.Button(new GUIContent("2", "Use Weighted Distribution Light; Objects will be mostly halfway between min and max scale"), GUILayout.Width(18f)))
            c_scaleDistribution = GetCurve(2);

        if (GUILayout.Button(new GUIContent("3", "Use Weighted Distribution Heavy; Most objects will be small to medium size, with very few of them reaching max size"), GUILayout.Width(18f)))
            c_scaleDistribution = GetCurve(3);

        GUILayout.Label(new GUIContent("ScaleX", "Enable scaling on the local X axis"), GUILayout.Width(42f));
        c_scaleX = EditorGUILayout.Toggle(new GUIContent("", "Enable scaling on the local X axis"), c_scaleX, GUILayout.Width(16f));
        GUILayout.Label(new GUIContent("ScaleY", "Enable scaling on the local Y axis"), GUILayout.Width(42));
        c_scaleY = EditorGUILayout.Toggle(new GUIContent("", "Enable scaling on the local Y axis"), c_scaleY, GUILayout.Width(16f));
        GUILayout.Label(new GUIContent("ScaleZ", "Enable scaling on the local Z axis"), GUILayout.Width(42f));
        c_scaleZ = EditorGUILayout.Toggle(new GUIContent("", "Enable scaling on the local Z axis"), c_scaleZ, GUILayout.Width(16f));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(new GUIContent("Minimum Scale", "The minimum scale of the resulting randomization"), GUILayout.Width(97f));
        c_minScale = EditorGUILayout.FloatField(new GUIContent("", "The minimum scale of the resulting randomization"), c_minScale, GUILayout.Width(38f));
        GUILayout.Label(new GUIContent("Maximum Scale", "The maximum scale of the resulting randomization"), GUILayout.Width(102f));
        c_maxScale = EditorGUILayout.FloatField(new GUIContent("", "The maximum scale of the resulting randomization"), c_maxScale, GUILayout.Width(38f));
        FixScale();
        GUILayout.Label(new GUIContent("Use Current Scale", "Performs the scale operation based on the object's current scale, otherwise scale will be 1*min/max"), GUILayout.Width(110f));
        c_keepOriginalScale = EditorGUILayout.Toggle(new GUIContent("", "Performs the scale operation based on the object's current scale, otherwise scale will be 1*min/max"), c_keepOriginalScale, GUILayout.Width(16f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Reset Scale", "Sets scale on all selected objects to 1."), GUILayout.Width(140f)))
            ResetScale();

        if (GUILayout.Button(new GUIContent("Scale!", "Randomly scales objects. ")))
            DoScale();

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>Handles logic for random scaling</summary>
    private void DoScale()
    {
        Transform[] selObj = Selection.transforms;
        if (selObj.Length > 0)
        {
            FixScale();
            Undo.RecordObjects(selObj, "Scale Objects");

            foreach (Transform t in selObj)
            {
                //Set scale based on input
                Vector3 scale = Vector3.one;
                if (c_keepOriginalScale)
                    scale = t.localScale;

                float r = Random.value;
                float y = c_scaleDistribution.Evaluate(r);
                y = Mathf.Clamp(y, 0f, 1f);
                float modScale = Mathf.Lerp(c_minScale, c_maxScale, y);

                if (c_scaleX)
                    scale.x = scale.x * modScale;

                if (c_scaleY)
                    scale.y = scale.y * modScale;

                if (c_scaleZ)
                    scale.z = scale.z * modScale;

                t.localScale = scale;
            }

            SetMsg("Rotated " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: Nothing selected to scale", "maroon");
        }
    }

    /// <summary>Fixes flipped min/max scales</summary>
    private void FixScale()
    {
        if (c_minScale < 0f)
            c_minScale = 0.01f;

        if (c_maxScale <= c_minScale)
            c_maxScale = c_minScale + 0.01f;

        if (c_minScale >= c_maxScale)
            c_minScale = c_maxScale - 0.01f;
    }

    /// <summary>Resets scale</summary>
    private void ResetScale()
    {
        Transform[] selObj = Selection.transforms;
        if (selObj.Length > 0)
        {
            Undo.RecordObjects(selObj, "Reset Scale");

            foreach (Transform t in selObj)
            {
                t.localScale = Vector3.one;
            }

            SetMsg("Reset scale on " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: No objects selected to scale", "maroon");
        }
    }

    private AnimationCurve GetWeightedCurve()
    {
        AnimationCurve newCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.32f, 0.412f), new Keyframe(0.915f, 0.734f), new Keyframe(1, 1));
        AnimationUtility.SetKeyRightTangentMode(newCurve, 0, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
        AnimationUtility.SetKeyLeftTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
        AnimationUtility.SetKeyLeftTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
        AnimationUtility.SetKeyRightTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
        AnimationUtility.SetKeyLeftTangentMode(newCurve, 3, AnimationUtility.TangentMode.Linear);

        return newCurve;
    }

    private AnimationCurve GetCurve(int type)
    {
        AnimationCurve newCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        switch (type)
        {
            case 1:
                AnimationUtility.SetKeyRightTangentMode(newCurve, 0, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 1, AnimationUtility.TangentMode.Linear);
                break;

            case 2:
                newCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.32f, 0.412f), new Keyframe(0.915f, 0.734f), new Keyframe(1, 1));
                AnimationUtility.SetKeyRightTangentMode(newCurve, 0, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
                AnimationUtility.SetKeyRightTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 3, AnimationUtility.TangentMode.Linear);
                break;

            case 3:
                newCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.72f, 0.412f), new Keyframe(0.925f, 0.711f), new Keyframe(1, 1));
                AnimationUtility.SetKeyRightTangentMode(newCurve, 0, AnimationUtility.TangentMode.Linear);
                AnimationUtility.SetKeyRightTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 1, AnimationUtility.TangentMode.ClampedAuto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
                AnimationUtility.SetKeyRightTangentMode(newCurve, 2, AnimationUtility.TangentMode.Auto);
                AnimationUtility.SetKeyLeftTangentMode(newCurve, 3, AnimationUtility.TangentMode.Auto);
                break;
        }
        return newCurve;
    }

    #endregion scale

    #region spread

    /// <summary>Handles creating the controls for Spread</summary>
    private void SpreadLayout()
    {
        //distance
        s_spreadDistMin = EditorGUILayout.Slider(new GUIContent("Minimum Spread", "The minimum distance an object will move"), s_spreadDistMin, 0.01f, 20f);
        s_spreadDistMax = EditorGUILayout.Slider(new GUIContent("Maximum Spread", "The maximum distance an object will move"), s_spreadDistMax, 0.02f, 20f);
        FixSpreadInput();

        //spread dir
        GUILayout.BeginHorizontal();
        s_xSpread = GUILayout.Toggle(s_xSpread, new GUIContent("Spread X", "Enable spreading on the X axis"));
        s_ySpread = GUILayout.Toggle(s_ySpread, new GUIContent("Spread Y", "Enable spreading on the Y axis"));
        s_zSpread = GUILayout.Toggle(s_zSpread, new GUIContent("Spread Z", "Enable spreading on the Z axis"));
        s_additiveSpread = GUILayout.Toggle(s_additiveSpread, new GUIContent("Additive", "Spread values add up, meaning the objects won't overlap (given that mindistance is bigger than the size of your objects)"));
        s_avgSel = GUILayout.Toggle(s_avgSel, new GUIContent("Average Position", "Average the positions of selected objects before spreading them"));
        GUILayout.EndHorizontal();

        //actions
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Spread Direction", "Choose which directions to spread in"), GUILayout.Width(100f));
        s_spreadDir = (Directions)EditorGUILayout.EnumPopup(s_spreadDir, GUILayout.Width(70f));
        if (GUILayout.Button(new GUIContent("Reset Position", "Unparents the selected objects, and moves them to the world's origin."), GUILayout.Width(95f)))
            ResetPosition();

        if (GUILayout.Button("Spread!"))
            DoSpread();

        GUILayout.EndHorizontal();
    }

    /// <summary>Handles Spreading logic</summary>
    private void DoSpread()
    {
        Transform[] selObj = Selection.transforms;
        FixSpreadInput();   //fix input in case it's still wrong

        //need selected objects
        if (selObj.Length > 0)
        {
            Undo.RecordObjects(selObj, "Spread Objects");
            Vector3 sNeg = Vector3.zero;    //storage for negative maximum
            Vector3 sPos = Vector3.zero;    //storage for positive maximum
            Vector3 sPower = Vector3.zero;  //storage for using axes
            Vector3 posAvg = AveragePosition(selObj);   //get position when spreading from avg

            //assign axes
            if (s_xSpread)
                sPower.x = 1;
            if (s_ySpread)
                sPower.y = 1;
            if (s_zSpread)
                sPower.z = 1;

            //assign dir
            Vector3 dir;
            if (s_spreadDir == Directions.Positive)
                dir = Vector3.one;  //positive
            else
                dir = -Vector3.one; //negative

            foreach (Transform t in selObj)
            {
                Vector3 preMove = t.position;
                if (s_avgSel) //set position if we average
                    t.position = posAvg;

                //re-assign dir for each object if we spread positive and negative
                if (s_spreadDir == Directions.Both)
                    dir = new Vector3(RandomOneOneMinus(), RandomOneOneMinus(), RandomOneOneMinus());

                //get random spread on each axis IF we have that axis enabled
                Vector3 spread = new Vector3(
                    sPower.x * dir.x * Random.Range(s_spreadDistMin, s_spreadDistMax),
                    sPower.y * dir.y * Random.Range(s_spreadDistMin, s_spreadDistMax),
                    sPower.z * dir.z * Random.Range(s_spreadDistMin, s_spreadDistMax));

                //logic for storing and using negative and positive maximum spread for additive mode
                if (s_additiveSpread)
                {
                    //x
                    if (dir.x < 0f) //we spread to negative
                    {
                        spread.x += sNeg.x; //add previous minimum to current random spread
                        sNeg.x = spread.x;      //make the minimum the new value
                    }
                    else  //we spread to positive
                    {
                        spread.x += sPos.x; //add previous maximum to current random spread
                        sPos.x = spread.x;  //make the maximum the new value
                    }
                    //y
                    if (dir.y < 0f)
                    {
                        spread.y += sNeg.y;
                        sNeg.y = spread.y;
                    }
                    else
                    {
                        spread.y += sPos.y;
                        sPos.y = spread.y;
                    }
                    //z
                    if (dir.z < 0f)
                    {
                        spread.z += sNeg.z;
                        sNeg.z = spread.z;
                    }
                    else
                    {
                        spread.z += sPos.z;
                        sPos.z = spread.z;
                    }
                }
                //finally translate; local wouldn't make too much sense as an option
                t.Translate(spread, Space.World);
                //  Debug.DrawLine(preMove, t.position, Color.cyan, 2f, false);
                DrawArrow(preMove, t.position, Color.cyan, 2f);
            }

            SetMsg("Successfully spread " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: No selected objects found, cannot randomize position.", "maroon");
        }
    }

    /// <summary>Fixes flipped spread values</summary>
    private void FixSpreadInput()
    {
        if (s_spreadDistMin <= 0)
            s_spreadDistMin = 0f;

        if (s_spreadDistMax <= s_spreadDistMin)
            s_spreadDistMax += 0.05f;

        if (s_spreadDistMin >= s_spreadDistMax)
            s_spreadDistMin -= 0.05f;
    }

    #endregion spread

    #region project

    /// <summary>Handles creating the UI for Projecting</summary>
    private void ProjectLayout()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Type", "Pick between projecting in a general direction or on to the collider of a specific surface. If you want to place objects on terrain, you are probably better off using Direction while placing the objects above the terrain."), GUILayout.Width(38f));
        p_projType = (ProjectionType)EditorGUILayout.EnumPopup(p_projType, GUILayout.Width(68f));
        //GUILayout.Space(1f);
        //Toggle for rotating to surface

        GUILayout.Label(new GUIContent("Rotate to Surface", "Aligns the projected object to the surface it's being moved to"), GUILayout.Width(104f));
        p_rotateToSurf = GUILayout.Toggle(p_rotateToSurf, "", GUILayout.Width(20f));
        if (p_rotateToSurf)
        {
            //Input for alignment selection
            GUILayout.Label(new GUIContent("Align in Direction", "When rotating to a surface, align this axis of the object 'away' from the surface"), GUILayout.Width(102f));
            p_alignAxis = (Sides)EditorGUILayout.EnumPopup(p_alignAxis, GUILayout.Width(72f));
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Project Offset", "Offset the object in the align direction (this happens after clipping cancellation if your mesh has a meshfilter)"), GUILayout.Width(82f));
        p_projectOffset = EditorGUILayout.FloatField(p_projectOffset, GUILayout.Width(22f));
        GUILayout.Label(new GUIContent("Use Mesh Bottom", p_useMeshBottomTooltip), GUILayout.Width(82f));
        p_useMeshBottom = EditorGUILayout.Toggle(p_useMeshBottom, EditorStyles.toggle);
        if (p_projType == ProjectionType.Direction)
        {
            //Input for direction selection
            GUILayout.Label(new GUIContent("Project in Direction", "Direction in which the projection happens; Overridden if project on surface is set"), GUILayout.Width(114f));
            p_projectDirE = (Sides)EditorGUILayout.EnumPopup(p_projectDirE, GUILayout.Width(75f));

            //Toggle for parenting to surface
            p_parentToHitSurf = GUILayout.Toggle(p_parentToHitSurf, new GUIContent("Parent to Surface", "Parents the object to the hit surface, whether or not we're projecting on a surface or just in a general direction"), GUILayout.Width(118f));
        }
        else
        {
            //Input for surface we want to project onto
            GUILayout.Label(new GUIContent("Project on Surface", "Project objects onto the surface of a specific collider"), GUILayout.Width(115f));
            p_surfaceColl = EditorGUILayout.ObjectField(p_surfaceColl, typeof(Collider), true, GUILayout.Width(150f)) as Collider;
            //Clear surface
            if (GUILayout.Button(new GUIContent("x", "Clear"), GUILayout.Width(20f)))
                p_surfaceColl = null;
            //Tip for assigning surface
            if (GUILayout.Button(new GUIContent("g", "Press to get the collider of the selected object."), GUILayout.Width(20f)))
                GrabColl();
        }
        EditorGUILayout.EndHorizontal();

        //Reset position input
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Reset Position", "Unparents the selected objects, and moves them to the world's origin.")))
            ResetPosition();

        //Init project input
        if (GUILayout.Button(new GUIContent("Project!", "Moves selected objects to roughly snap onto the selected surface in 'direction' (or the nearest surface in 'direction' if nothing is selected)")))
        {
            ProjectObjects();
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>Handles calling directional or surface projection modes and changing layers</summary>
    private void ProjectObjects()
    {
        Transform[] selObj = Selection.transforms;

        if (selObj.Length > 0)
        {
            Undo.RecordObjects(selObj, "Project Objects");     //add to undo queue
            int count = 0;  //keep track of how many objects we've projected
            //surface project
            if (p_projType == ProjectionType.Surface)
            {
                //if we have collider assigned
                if (p_surfaceColl)
                {
                    //attempt projection to surface on each selected object and return true is successful
                    foreach (Transform t in selObj)
                    {
                        if (ProjectOnSurface(t))
                            count++;
                    }
                    if (count == selObj.Length)
                        SetMsg("Successfully projected " + count + "/" + selObj.Length + " objects onto " + p_surfaceColl.gameObject.name, "green");  //editor feedback 100%
                    else
                        SetMsg("Successfully projected " + count + "/" + selObj.Length + " objects onto " + p_surfaceColl.gameObject.name, "olive");  //editor feedback <100%
                }
                else //no collider
                {
                    SetMsg("ERROR: No collider selected as the projection surface", "maroon");
                }
            }
            //general project
            else
            {
                Vector3 generalDir = GetDirection(p_projectDirE); //get the general project direction based on settings

                //cache layers so that selections don't hit each other; set to ignoreraycast
                int[] cachedLayers = new int[selObj.Length];
                for (int i = 0; i < selObj.Length; i++)
                {
                    cachedLayers[i] = selObj[i].gameObject.layer;
                    selObj[i].gameObject.layer = 2;
                }

                //attempt projection in direction on each selected object and return true is successful
                foreach (Transform t in selObj)
                {
                    if (ProjectInDirection(t, generalDir))
                        count++;
                }

                //reset to original layers
                for (int i = 0; i < selObj.Length; i++)
                {
                    selObj[i].gameObject.layer = cachedLayers[i];
                }

                //feedback
                if (count == selObj.Length)
                    SetMsg("Successfully projected " + count + "/" + selObj.Length + " objects in the desired direction", "green");  //editor feedback
                else if (count > 0)
                    SetMsg("Successfully projected " + count + "/" + selObj.Length + " objects in the desired direction", "olive");  //editor feedback
                else
                    SetMsg("Could not project any of the " + selObj.Length + " objects onto a surface; check the direction settings and/or colliders of nearby surfaces.", "maroon");  //editor feedback
            }
        }
        else
        {
            SetMsg("ERROR: Cannot project, no objects are selected", "maroon");
        }
    }

    /// <summary>Handles surface projection using multiple raycasting methods to ensure that we hit the surface. Returns true if it succeeds</summary>
    private bool ProjectOnSurface(Transform obj)
    {
        Vector3 closePoint = p_surfaceColl.ClosestPointOnBounds(obj.position);
        Ray r = new Ray(obj.position, closePoint - obj.position);
        Ray r2 = new Ray(obj.position, p_surfaceColl.transform.position - obj.position); //sadly bounds makes it so that the raycast can miss, so we add an emergency ray

        RaycastHit hit;

        if (p_surfaceColl.Raycast(r, out hit, Mathf.Infinity))
        {
            ProjectSuccess(hit, obj);
            return true;
        }
        else if (p_surfaceColl.Raycast(r2, out hit, Mathf.Infinity))
        {
            ProjectSuccess(hit, obj);
            return true;
        }
        else
        {
            // Debug.LogError("Project on surface failed on " + obj.name + ". Possible causes: Your Surface has big holes in it. Try moving the objects that you wanted to project.");
            DrawCross(obj.position, obj, Color.red, false, 0.75f, 4f);
        }

        return false;
    }

    /// <summary>Handles directional projection and returns True if it succeeds</summary>
    private bool ProjectInDirection(Transform obj, Vector3 dir)
    {
        int mask = 1 << 2;  //mask of ignoreraycast to make SURE we don't hit it

        RaycastHit hit;
        if (Physics.Raycast(obj.position, dir, out hit, Mathf.Infinity, ~mask, QueryTriggerInteraction.Ignore))
        {
            obj.position = hit.point;   //move to hit surface
            if (p_rotateToSurf) //rotate to hit surf
            {
                obj.rotation = Quaternion.FromToRotation(GetDirection(p_alignAxis), hit.normal);
                obj.Rotate(hit.normal, Random.Range(-180f, 180f), Space.World);   //add random rotation on the align axis for variation
            }

            if (p_parentToHitSurf)  //parent
                obj.SetParent(hit.transform, true);

            FixOffset(obj, hit, p_alignAxis, p_projectOffset);

            return true;
        }
        else
        {
            // Debug.LogError("Projecting failed on " + obj.name, obj);
            DrawCross(obj.position, obj, Color.red, false, 0.75f, 4f);
            return false;
        }
    }

    /// <summary>Handles aligning an object to a raycast hit surface and fixing it's offset/rotation</summary>
    private void ProjectSuccess(RaycastHit hit, Transform projObj)
    {
        if (p_rotateToSurf)
        {
            projObj.rotation = Quaternion.FromToRotation(GetDirection(p_alignAxis), hit.normal);
        }

        if (p_parentToHitSurf)
        {
            projObj.SetParent(hit.transform, true);
        }

        projObj.position = hit.point;

        FixOffset(projObj, hit, p_alignAxis, p_projectOffset);
    }

    /// <summary>Handles moving the projected object so that it no longer clips with the surface it was projected onto</summary>
    private void FixOffset(Transform obj, RaycastHit hit, Sides pointAwaySide, float extraOffset)
    {
        float offset = 0f;

        //Calculate offset if we have a meshrenderer
        if (obj.GetComponentInChildren<MeshFilter>() != null && p_useMeshBottom)
        {
            //obj.rotation *  -GetDirection(p_alignAxis)
            Vector3 lowPoint = GetHierarchyLowestPoint(hit.normal, obj, pointAwaySide, false);
            offset = -Vector3.Dot((lowPoint - hit.point), hit.normal);
        }

        //Add input offset
        offset += extraOffset;
        //  Debug.Log(extraOffset);
        //Translate in the hit surface's direction by offset
        obj.Translate(hit.normal * offset, Space.World);

        //debug
        DrawCross(obj.position, obj, Color.green, false, offset / 2, 4f);
        Debug.DrawRay(hit.point, hit.normal * offset, Color.cyan, 4f, false);
        DrawCross(hit.point, obj, Color.cyan, true, 0.4f, 4f);
    }

    /// <summary>
    /// Take all meshfilters in a hierarchy, and get the lowest visual point of that hierarchy, relative to a direction.
    /// Do note that objects are expected to be rotated in said direction, "side" is just shorthand for which side of the object we're checking is lowest on the axis
    /// </summary>
    private Vector3 GetHierarchyLowestPoint(Vector3 dir, Transform obj, Sides side, bool invertSide)
    {
        dir.Normalize();
        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        Vector3 bestPoint = GetBoundsDirWorldPos(meshFilters[0], side, invertSide);
        if (meshFilters.Length >= 2)    //only do comparison if we have something to compare against
        {
            for (int i = 1; i < meshFilters.Length; i++)
            {
                Vector3 point = GetBoundsDirWorldPos(meshFilters[i], side, invertSide);
                if (Vector3.Dot((point - bestPoint), dir) < 0)
                {
                    // Debug.Log(meshFilters[i].transform.name + " is lower");
                    bestPoint = point;
                }
            }
        }
        else
        {
            return GetBoundsDirWorldPos(meshFilters[0], side, invertSide);
        }
        Debug.Log(bestPoint);

        return bestPoint;
    }

    /// <summary>Get the bounds of the side that we want the object to be aligned with towards the surface</summary>
    private Vector3 GetBoundsDirWorldPos(MeshFilter filter, Sides dir, bool opposite)
    {
        Bounds mB = filter.sharedMesh.bounds;
        Vector3 v3Center = mB.center;
        Vector3 v3Extents = mB.extents;
        Vector3 pos = new Vector3(v3Center.x, v3Center.y, v3Center.z);

        if (opposite)
        {
            switch (dir)
            {
                case Sides.Ypositive:
                    dir = Sides.Ynegative;
                    break;

                case Sides.Ynegative:
                    dir = Sides.Ypositive;
                    break;

                case Sides.Xnegative:
                    dir = Sides.Xpositive;
                    break;

                case Sides.Xpositive:
                    dir = Sides.Xnegative;
                    break;

                case Sides.Zpositive:
                    dir = Sides.Znegative;
                    break;

                case Sides.Znegative:
                    dir = Sides.Zpositive;
                    break;
            }
        }

        //What we do here is basically look at the object's bounding box in local space, and grab the center of a side of the bounding box.
        //The side we grab is based on what side we wanted the object to be aligned with.
        //We have to do all of this because mesh bounds are AABB; axis aligned bounding boxes, but we need an OOBB(Object oriented bounding box) for accurate clipping cancellation
        //Therefore if we want the object to be aligned in a direction, we grab it's lowest point on that direction.
        switch (dir)
        {
            case Sides.Ypositive: //object pointing up  (positive)   //grab bottom side
                pos.y += -v3Extents.y;
                break;

            case Sides.Ynegative:   //object pointing down  (negative)   //grab top side
                pos.y += v3Extents.y;
                break;

            case Sides.Xnegative: //object pointing left    (negative)   //grab right side
                pos.x += v3Extents.x;
                break;

            case Sides.Xpositive: //object pointing right   (positive)  //grab left side
                pos.x += -v3Extents.x;
                break;

            case Sides.Zpositive: //object pointing forward (positive)  //grab back side
                pos.z += -v3Extents.z;
                break;

            case Sides.Znegative: //object pointing backward    (negative)  //grab front side
                pos.z += v3Extents.z;
                break;

            default:    //default to point up
                pos.y += -v3Extents.y;
                break;
        }
        //Transform the point to world space based on the transform the meshfilter it's attached to within the main object's hierarchy
        return filter.transform.TransformPoint(pos);
    }

    /// <summary>Attempts to find collider on selection</summary>
    private void GrabColl()
    {
        if (Selection.transforms.Length > 0)
        {
            if (Selection.transforms[0].GetComponent<Collider>() != null)
                p_surfaceColl = Selection.transforms[0].GetComponent<Collider>();
            else
                SetMsg("ERROR: Selected object doesn't have a collider", "maroon");
        }
        else
        {
            SetMsg("ERROR: No selected object", "maroon");
        }
    }

    private void ResetPosition()
    {
        Transform[] selObj = Selection.transforms;

        if (selObj.Length > 0)
        {
            Undo.RecordObjects(selObj, "Reset Position");
            foreach (Transform t in selObj)
            {
                t.localPosition = Vector3.zero;
            }

            SetMsg("Reset position on " + selObj.Length + " objects", "green");
        }
        else
        {
            SetMsg("ERROR: Nothing selected to reset", "maroon");
        }
    }

    private enum ProjectionType
    {
        Direction,
        Surface,
    }

    #endregion project

    #region stack

    /// <summary>Handles creating the UI for stacking</summary>
    private void StackLayout()
    {
        t_stackOffset = EditorGUILayout.Slider(new GUIContent("Stack Offset", "Offset the stack to overlap more or less"), t_stackOffset, -5f, 10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Stacking Direction", "Direction in which to stack objects"), GUILayout.Width(115));
        t_stackType = (Sides)EditorGUILayout.EnumPopup(t_stackType, GUILayout.Width(80f));
        if (GUILayout.Button(new GUIContent("Stack!", "Stack objects in the desired direction, accounting for their size.")))
            DoStack();

        GUILayout.EndHorizontal();
    }

    /// <summary>Handles logic for stacking</summary>
    private void DoStack()
    {
        Transform[] selObj = Selection.transforms;
        //need objects
        if (selObj.Length > 0)
        {
            //need 2 or more
            if (selObj.Length >= 2)
            {
                //storage for objects to stack
                List<Transform> filterObj = new List<Transform>();
                //Filter out all meshes that don't have a meshfilter
                foreach (Transform t in selObj)
                {
                    MeshFilter a = t.GetComponentInChildren<MeshFilter>();
                    if (a != null)
                    {
                        if (a.sharedMesh != null)
                        {
                            filterObj.Add(t);
                        }
                    }
                }
                //need at least 2 meshfilters for stacking
                if (filterObj.Count >= 2)
                {
                    Undo.RecordObjects(selObj, "Stack Objects");

                    //grab the lowest position in selection
                    Vector3 lowestPos = filterObj[0].position;
                    foreach (Transform t in filterObj)
                    {
                        if (t.position.y < lowestPos.y)
                            lowestPos = t.position;
                    }

                    //move all objects to lowest position
                    foreach (Transform t in filterObj)
                    {
                        t.position = lowestPos;
                        t.rotation = Quaternion.identity;
                    }

                    Vector3 dir = GetDirection(t_stackType);

                    //core offset
                    for (int i = 1; i < filterObj.Count; i++)
                    {
                        Vector3 stackTop = GetHierarchyLowestPoint(dir, filterObj[i - 1], t_stackType, true);   //grab top of previous object
                        Vector3 stackBottom = GetHierarchyLowestPoint(dir, filterObj[i], t_stackType, false);   //grab bottom of current objcet
                        float dist = Vector3.Dot((stackBottom - stackTop), dir) - t_stackOffset;    //calculate offset on axis
                        filterObj[i].Translate(dir * -dist, Space.World);   //move on axis
                                                                            //debug
                        DrawCross(Vector3.MoveTowards(stackBottom, filterObj[i].position, -dist), filterObj[i], Color.yellow, true, 1.3f, 3f);
                        //Debug.Log("i: " + i + " " + selObjm[i].transform.name + " postop:" + stackTop + " stackbot:" + stackBottom + " offset:" + dist);
                    }

                    SetMsg("Successfully stacked " + filterObj.Count + "/" + selObj.Length + " objects", "green");
                }
                else
                {
                    SetMsg("ERROR: The script NEEDS at least one meshfilter per selection hierarchy. Stacking does take children's meshfilters into account.", "maroon");
                }
            }
            else if (selObj.Length == 1)
            {
                SetMsg("ERROR: You need to select more than 1 object for stacking.", "maroon");
            }
            else
            {
                SetMsg("ERROR: You need to select the objects that you want to stack.", "maroon");
            }
        }
        else
        {
            SetMsg("ERROR: No objects selected for stacking.", "maroon");
        }
    }

    #endregion stack

    #region place

    private void PlaceLayout()
    {
        if (l_objectsToPlace.Count > 0)
            GUILayout.Label(new GUIContent("<size=11>" + l_objectsToPlace.Count + " Objects selected. Press </size><size=13><color=teal>G</color></size><size=11> to clone at mouse (click scene view)" + "</size>", "Amount of objects selected"), styleM);
        else
            GUILayout.Label(new GUIContent("<size=11>" + "Select objects in Hierarchy or Project to start Cloning" + "</size>"), styleM);

        //select, parent, clear, grab
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Select", "Select one or more objects to designate them for cloning")))
            DoSelect();

        GUILayout.Label(new GUIContent("Parent Root", "Optional; Select a transform to parent to, for organizational purposes so that we don't litter the hierarchy with the spawned objects"), GUILayout.Width(75f));
        l_placeParent = EditorGUILayout.ObjectField(l_placeParent, typeof(Transform), true) as Transform;

        if (GUILayout.Button(new GUIContent("x", "Clear parent"), GUILayout.Width(19f)))
            l_placeParent = null;

        if (GUILayout.Button(new GUIContent("p", "Press to use selection as the parent"), GUILayout.Width(19f)))
            GrabTransform();

        GUILayout.EndHorizontal();

        //offset, rotate to surface, align axis
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Place Offset", "Offset the object in the rotation direction"), GUILayout.Width(75f));
        l_placeOffset = EditorGUILayout.FloatField(l_placeOffset, GUILayout.Width(25f));
        GUILayout.Label(new GUIContent("Use Mesh Bottom", p_useMeshBottomTooltip), GUILayout.Width(82f));
        p_useMeshBottom = EditorGUILayout.Toggle(p_useMeshBottom, EditorStyles.toggle);
        GUILayout.Label(new GUIContent("Rotate to Surface", "Align the objects to the hit surface"), GUILayout.Width(104f));
        l_rotateToSurf = GUILayout.Toggle(l_rotateToSurf, "", GUILayout.Width(20f));
        if (l_rotateToSurf)
        {
            GUILayout.Label(new GUIContent("Align in Direction", "When rotating to surface, use this side of the object as the 'bottom'"), GUILayout.Width(100f));
            l_alignAxis = (Sides)EditorGUILayout.EnumPopup(l_alignAxis, GUILayout.Width(92f));
        }
        GUILayout.EndHorizontal();
    }

    private void DoPlace(RaycastHit mouseSurface)
    {
        //  Debug.Log("doplace called");
        if (l_objectsToPlace.Count > 0)
        {
            //object creation
            int randomID = Random.Range(0, l_objectsToPlace.Count);
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(l_objectsToPlace[randomID]);
            //Debug.Log(newObj.name);
            if (newObj == null)
                newObj = (GameObject)Instantiate(l_objectsToPlace[randomID]);

            //placement
            newObj.transform.position = mouseSurface.point; //position at hit
            Undo.RegisterCreatedObjectUndo(newObj, "Clone Prop");

            //rotate to hit surf
            if (l_rotateToSurf)
            {
                newObj.transform.rotation = Quaternion.FromToRotation(GetDirection(l_alignAxis), mouseSurface.normal);
                newObj.transform.Rotate(mouseSurface.normal, Random.Range(-180f, 180f), Space.World);   //add random rotation on the align axis for variation
            }
            //  Debug.Log(l_placeOffset);
            FixOffset(newObj.transform, mouseSurface, l_alignAxis, l_placeOffset); //offset fix



            if (l_placeParent != null)  //parent to selected object
                newObj.transform.SetParent(l_placeParent, true);

            //   Debug.Log(randomID + " " + l_objectsToPlace[randomID].name);
            SetMsg("Placed " + newObj.transform.name + " at " + mouseSurface.transform.name, "green");
            //toolWindowRef.Repaint();
        }
        else
        {
            SetMsg("ERROR: No objects selected for placement", "maroon");  //editor feedback
            toolWindowRef.Repaint();
        }
    }

    private void GrabTransform()
    {
        if (Selection.transforms.Length > 0)
        {
            l_placeParent = Selection.activeTransform;
        }
        else
        {
            SetMsg("ERROR: No selected object", "maroon");
        }
    }

    private void DoSelect()
    {
        GameObject[] selObj = Selection.gameObjects;
        l_objectsToPlace.Clear();   //clear previous selection
        if (selObj.Length > 0)
        {
            foreach (GameObject g in selObj)
            {
                //get prefab
                Object a = PrefabUtility.GetCorrespondingObjectFromSource(g);

                if (a != null)  //object selected in scene, has prefab
                {
                    l_objectsToPlace.Add(a);
                }
                else if (g != null) //use actual selection if there is no prefab, since it's probably a prefab in project view then
                {
                    l_objectsToPlace.Add(g);
                }
                SetMsg("Designated " + l_objectsToPlace.Count + " objects for cloning", "green");
                Debug.Log(g.transform.name);
            }
        }
        else
        {
            SetMsg("ERROR: No objects selected", "maroon");
        }
    }

    #endregion place

    #region misc

    /// <summary>Sets the message to display in the tool window</summary>
    private void SetMsg(string text, string color)
    {
        //   Debug.Log(text + " text");
        //   Debug.Log(previous + " prev");
        //   Debug.Log(repId);
        if (color == "maroon")
        {
            if (text == previous || text + " " + repId == previous)
            {
                repId++;
                text = text + " " + repId;
            }
            else
            {
                repId = 0;
            }
        }
        msg = "<color=" + color + ">" + text + "</color>";

        previous = text;

        RandomTip();
    }

    private void RandomTip()
    {
        float r = Random.value;

        if (r < 0.35f)
            helpText = "<color=grey>Tip: Use Ctrl + G to GROUP selection</color>";
        else if (r > 0.35f && r < 0.7f)
            helpText = "<color=grey>Tip: Use Ctrl + Z to UNDO actions</color>";
        else if (r > 0.7f && r < 0.85f)
            helpText = "<color=grey>Tip: Reset pos/rot/scale isn't the same as Undo.</color>";
        else
            helpText = "<color=grey>Tip: Hover over labels for detailed help</color>";











    }

    /// <summary>returns -1 OR 1</summary>
    private float RandomOneOneMinus()
    {
        if (Random.Range(0, 2) == 0)
        {
            return -1;
        }
        return 1;
    }

    /// <summary>Enum for projection directions</summary>
    private enum Sides
    {
        Ypositive,
        Ynegative,
        Xpositive,
        Xnegative,
        Zpositive,
        Znegative,
    }

    /// <summary>Enum for directions</summary>
    private enum Directions
    {
        Positive,
        Negative,
        Both
    }

    /// <summary>Draws a cross; only works in play mode on older versions of unity?</summary>
    private void DrawCross(Vector3 pos, Transform refAxis, Color color, bool flat, float length, float t)
    {
        //x
        Debug.DrawRay(pos, refAxis.right * length, color, t, false);
        Debug.DrawRay(pos, -refAxis.right * length, color, t, false);
        if (!flat)
        {
            //y
            Debug.DrawRay(pos, refAxis.up * length, color, t, false);
            Debug.DrawRay(pos, -refAxis.up * length, color, t, false);
        }
        //z
        Debug.DrawRay(pos, refAxis.forward * length, color, t, false);
        Debug.DrawRay(pos, -refAxis.forward * length, color, t, false);
    }

    private void DrawArrow(Vector3 from, Vector3 to, Color color, float t)
    {
        float l = Vector3.Distance(from, to) * 0.2f;

        Debug.DrawLine(from, to, color, t, false);
        Debug.DrawRay(to, Vector3.RotateTowards(from - to, Vector3.up, 0.33f, 1f).normalized * l, color, t, false);
        Debug.DrawRay(to, Vector3.RotateTowards(from - to, -Vector3.up, 0.33f, 1f).normalized * l, color, t, false);
    }

    private Vector3 AveragePosition(Transform[] avgTra)
    {
        Vector3 total = Vector3.zero;
        foreach (Transform t in avgTra)
        {
            total += t.position;
        }

        return total / avgTra.Length;
    }

    /// <summary>Gets the Vector for the desired direction enum</summary>
    private Vector3 GetDirection(Sides dir)
    {
        Vector3 ret = Vector3.down;

        switch (dir)
        {
            case Sides.Ypositive:
                ret = Vector3.up;
                break;

            case Sides.Ynegative:
                ret = Vector3.down;
                break;

            case Sides.Xnegative:
                ret = Vector3.left;
                break;

            case Sides.Xpositive:
                ret = Vector3.right;
                break;

            case Sides.Zpositive:
                ret = Vector3.forward;
                break;

            case Sides.Znegative:
                ret = Vector3.back;
                break;

            default:
                break;
        }
        return ret;
    }

    private static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

    private void ToggleUI(int active)
    {
        RandomTip();
        if (singleOpen)
        {
            expandRotate = false;
            expandScale = false;
            expandSpread = false;
            expandStack = false;
            expandProject = false;
            expandPlace = false;
        }

        switch (active)
        {
            case 0:
                break;

            case 1:
                expandRotate = !expandRotate;
                break;

            case 2:
                expandScale = !expandScale;
                break;

            case 3:
                expandSpread = !expandSpread;
                break;

            case 4:
                expandStack = !expandStack;
                break;

            case 5:
                expandProject = !expandProject;
                break;

            case 6:
                expandPlace = !expandPlace;
                break;
        }
    }

    private void SwitchExpand()
    {
        ToggleUI(0);
        EditorWindow w = GetWindow<LevelBuildingTools>();

        singleOpen = !singleOpen;

        expandRotate = false;
        expandScale = false;
        expandSpread = false;
        expandStack = false;
        expandProject = false;
        expandPlace = false;

        if (singleOpen)
        {
            w.minSize = new Vector2(444f, 210f);
            w.maxSize = new Vector2(1000f, 210);
        }
        else
        {
            w.minSize = new Vector2(458, 300f);
            w.maxSize = new Vector3(1000f, 1000f);
        }
    }

    private Texture GetTexture(int i, bool on)
    {
        if (on)
            return uiTex[i];
        else
            return uiTex[6 + i];
    }

    #endregion misc
}