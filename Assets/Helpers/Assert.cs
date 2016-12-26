using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UAssert = UnityEngine.Assertions.Assert;

public static class Assert{

    public static void HardAssert(bool isTrue, string message = "")
    {
        if (!Debug.isDebugBuild) UAssert.raiseExceptions = true;
        if (message != "") UAssert.IsTrue(isTrue, message);
        else UAssert.IsTrue(isTrue);
        if (!isTrue) Debug.Break();
        if (!Debug.isDebugBuild) UAssert.raiseExceptions = false;
    }

    public static void SoftAssert(bool isTrue, string message = "")
    {
        if (message != "") UAssert.IsTrue(isTrue, message);
        else UAssert.IsTrue(isTrue);
    }
}
