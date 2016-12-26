using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public partial class Helper : MonoBehaviour
{
    private static Helper HelperObject;

    public static Vector3 ClickToPlane(float y = 0)
    {
        float mag;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up * y);
        plane.Raycast(ray, out mag);

        Vector3 point = ray.origin + ray.direction * mag;

        return point;
    }

    #region DelayedActions


    public static void DelayAction(Action method, float delay)
    {
        if (HelperObject == null) CreateHelperObject();

        HelperObject.StartCoroutine(Delayer(method, delay));
    }

    static IEnumerator Delayer(Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke();
    }
    public static void DelayAction<T1>(Action<T1> method, T1 arg1, float delay)
    {
        if (HelperObject == null) CreateHelperObject();

        HelperObject.StartCoroutine(Delayer(method, arg1, delay));
    }

    static IEnumerator Delayer<T1>(Action<T1> method, T1 arg1, float delay)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke(arg1);
    }
    public static void DelayAction<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2, float delay)
    {
        if (HelperObject == null) CreateHelperObject();

        HelperObject.StartCoroutine(Delayer(method, arg1, arg2, delay));
    }

    static IEnumerator Delayer<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2, float delay)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke(arg1, arg2);
    }
    public static void DelayAction<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3, float delay)
    {
        if (HelperObject == null) CreateHelperObject();

        HelperObject.StartCoroutine(Delayer(method, arg1, arg2, arg3, delay));
    }

    static IEnumerator Delayer<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3, float delay)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke(arg1, arg2, arg3);
    }
    public static void DelayAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, float delay)
    {
        if (HelperObject == null) CreateHelperObject();
        
        HelperObject.StartCoroutine(Delayer(method, arg1, arg2, arg3, arg4, delay));
    }

    static IEnumerator Delayer<T1, T2, T3, T4>(Action<T1,T2,T3,T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, float delay)
    {
        yield return new WaitForSeconds(delay);
        method.Invoke(arg1,arg2,arg3,arg4);
    }

    #endregion

    private static void CreateHelperObject()
    {
        HelperObject = new GameObject("HelperObject",typeof(Helper)).GetComponent<Helper>();
    }
}
