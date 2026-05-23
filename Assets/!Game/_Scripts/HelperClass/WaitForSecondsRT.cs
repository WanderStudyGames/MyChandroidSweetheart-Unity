using UnityEngine;


//EXAMPLE


//IEnumerator MyCoroutine()
//{
//    WaitForSecondsRT wait = new WaitForSecondsRT(1);
//    while (true)
//    {
//        // ...
//        yield return wait.NewTime(0.5f);
//        //  ...
//    }
//}



public class WaitForSecondsRT : CustomYieldInstruction
{
    float m_Time;
    public override bool keepWaiting
    {
        get { return (m_Time -= Time.unscaledDeltaTime) > 0; }
    }
    public WaitForSecondsRT(float aWaitTime)
    {
        m_Time = aWaitTime;
    }
    public WaitForSecondsRT NewTime(float aTime)
    {
        m_Time = aTime;
        return this;
    }
}
