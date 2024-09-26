using Base.Enum;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  public class LayerSetup : MonoBehaviour
  {
    [MenuItem("Tools/Setup Layers")]
    private static void SetupLayers()
    {
      ClearLayers();

      foreach (Layer layer in System.Enum.GetValues(typeof(Layer)))
      {
        CreateLayer(layer.ToString());
      }
    }

    private static void ClearLayers()
    {
      SerializedObject tagManager = new(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
      SerializedProperty layersProp = tagManager.FindProperty("layers");

      for (int i = 8; i < layersProp.arraySize; i++)
      {
        SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(i);
        if (!string.IsNullOrEmpty(layerSP.stringValue))
        {
          layerSP.stringValue = "";
        }
      }

      tagManager.ApplyModifiedProperties();
    }

    private static void CreateLayer(string layerName)
    {
      SerializedObject tagManager = new(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
      SerializedProperty layersProp = tagManager.FindProperty("layers");

      for (int i = 8; i < layersProp.arraySize; i++)
      {
        SerializedProperty layerSP = layersProp.GetArrayElementAtIndex(i);
        if (layerSP.stringValue == "")
        {
          layerSP.stringValue = layerName;
          tagManager.ApplyModifiedProperties();
          return;
        }
      }

      Debug.LogError("All layers are full!");
    }
  }
}