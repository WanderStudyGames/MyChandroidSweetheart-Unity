using UnityEngine;

public static class GameObjectUtility
{
    public static bool TryGetFirstObjectTransform(string tag, out Transform transform)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        if (objects.Length == 0) { transform = null; return false; }
        return GetTransform(objects[0], out transform);
    }
    private static bool GetTransform(GameObject obj, out Transform transform)
    {
        if (obj == null)
        {
            transform = null;
            return false;
        }
        else
        {
            transform = obj.transform;
            return true;
        }
    }
    public static bool TryGetObjectTransform(string name, out Transform transform)
    {
        var obj = GameObject.Find(name);
        return GetTransform(obj, out transform);
    }
    public static GameObject SpawnUnique(GameObject template) { return SpawnUnique(Vector3.zero, Quaternion.identity, template); }
    public static GameObject SpawnUnique(Vector3 position, Quaternion rotation, GameObject template)
    {
        PurgeObjectsByTag(template.tag);
        return GameObject.Instantiate(template, position, rotation);
    }
    public static void PurgeObjectsByTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            AudioListener[] au = obj.GetComponentsInChildren<AudioListener>();
            foreach (AudioListener au2 in au) { au2.enabled = false; }
            Object.Destroy(obj);
        }
    }
    public static bool GameObjectExists(string tag) => GameObject.FindGameObjectWithTag(tag) != null;
    public static GameObject InstantiateAsChild(GameObject childObjectTemplate, GameObject parentObject)
    {
        if (parentObject == null) { Logger.Log("Companion Manager.SetInteractObject: parentObject is null"); return null; }
        if (childObjectTemplate == null) { return null; }
        var newObject = GameObject.Instantiate(childObjectTemplate);
        newObject.transform.SetParent(parentObject.transform);
        newObject.transform.ResetLocal();
        return newObject;
    }

    public static void SetLayerOfChildren(GameObject gameObject, int layer)
    {
        GameObject[] children = gameObject.GetComponentsInChildren<GameObject>();
        foreach (GameObject child in children)
        {
            child.layer = layer;
        }
        gameObject.layer = layer;
    }

    public static bool TryRaycastGetComponent<T>(Camera camera, LayerMask layerMask, out T scanObject, float distance = 1000f)
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, distance, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out T sObj))
            {
                scanObject = sObj;
                return true;
            }
        }
        scanObject = default;
        return false;
    }
    public static bool TryGetComponent<T>(this RaycastHit hit, out T component, float maxDistance = 1000, LayerMask layerMask = default)
    {
        component = default;
        if (hit.distance > 0 && hit.distance < maxDistance && layerMask.Contains(hit.collider.gameObject.layer) && hit.collider.gameObject.TryGetComponent(out component))
        {
            return true;
        }
        return false;
    }
}