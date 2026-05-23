using UnityEngine;

public interface ISprocketPushable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="force"></param>
    /// <returns>Returns boolean indicating whether object can be pushed away from</returns>
    bool Push(Vector3 force);
}
