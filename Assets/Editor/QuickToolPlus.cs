using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickToolPlus : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private Material m_Material;
    private Color m_Color = new Color(1.0f, 1.0f, 1.0f);
    private bool m_WMShader;

    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowExample()
    {
        QuickToolPlus wnd = GetWindow<QuickToolPlus>();
        wnd.titleContent = new GUIContent("QuickToolPlus");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        // Queries all the buttons (via class name) in our root and passes them
        // in the SetupButton method.
        var toolButtons = root.Query(className: "quicktool-button");
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(VisualElement button)
    {
        //If it is a primative button, spawn primative on click
        if (button.parent.name == "primative_buttons")
        {
            Debug.Log("This is a primative button");
            button.RegisterCallback<PointerUpEvent, string>(CreateObject, button.name);
        }
        else if (button.parent.name == "material_buttons")
        {
            Debug.Log("This is a material button");
            button.RegisterCallback<PointerUpEvent, string>(SelectMaterial, button.name);
        }
        else if (button.parent.name == "color_buttons")
        {
            Debug.Log("This is a color button");
            button.RegisterCallback<PointerUpEvent, string>(SelectColor, button.name);
        }
    }

    private void CreateObject(PointerUpEvent _, string primitiveTypeName)
    {
        string primitiveName = primitiveTypeName.Substring(0, primitiveTypeName.Length - 7);

        var pt = (PrimitiveType)Enum.Parse
                     (typeof(PrimitiveType), primitiveName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
        if(m_Material != null)
        {
            go.GetComponent<Renderer>().sharedMaterial = m_Material;
        }
        if(m_Color != null)
        {
            go.GetComponent<Renderer>().sharedMaterial.color = m_Color;
        }
    }

    private void SelectMaterial(PointerUpEvent _, string materialTypeName)
    {
        Debug.Log("Selecting Material!");
        string materialName = materialTypeName.Substring(0, materialTypeName.Length - 7);
        if (materialName.Equals("default_material"))
        {
            m_Material = Instantiate(Resources.Load<Material>("Materials/" + materialName));
        }
        else
        {
            if (m_WMShader)
            {
                m_Material = Instantiate(Resources.Load<Material>("Materials/" + materialName + "_WMShader"));
            }
            else
            {
                m_Material = Instantiate(Resources.Load<Material>("Materials/" + materialName + "_StandardShader"));
            }
        }
    }
    private void SelectColor(PointerUpEvent _, string colorTypeName)
    {
        Debug.Log("Selecting Color!");
        string colorName = colorTypeName.Substring(0, colorTypeName.Length - 7);
        // Set Color Variable
        Color selectedColor;
        switch (colorName.ToLower())
        {
            case "red":
                selectedColor = Color.red;
                break;
            case "green":
                selectedColor = Color.green;
                break;
            case "blue":
                selectedColor = Color.blue;
                break;
            case "black":
                selectedColor = Color.black;
                break;
            default:
                selectedColor = Color.white; // Fallback to white if the color is unknown
                break;
        }

        m_Color = selectedColor;
    }
}