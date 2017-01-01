using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnitAssert = NUnit.Framework.Assert;

public class ProgressionManagerTester {

	[Test]
	public void ProfileCreationAndDeletion()
	{

        ProgressionManager.DeleteProfile("TestProfile1");
        ProgressionManager.DeleteProfile("TestProfile2");
        ProgressionManager.DeleteProfile("TestProfile3");

        string[] profiles;

        //Creation
        ProgressionManager.CreateNewProfile("TestProfile1");

        UnitAssert.AreNotEqual(ProgressionManager.CurrentProfile, null);

        UnitAssert.IsTrue(ProgressionManager.SaveProfile(), "Failed to Save profile");
	    profiles = ProgressionManager.GetSavedProfiles();

        //Assert
        UnitAssert.AreNotEqual(profiles.Length, 0);
        UnitAssert.AreEqual("TestProfile1", profiles[0]);

        //Deletion
	    ProgressionManager.DeleteProfile("TestProfile1");
	    profiles = ProgressionManager.GetSavedProfiles();

        UnitAssert.IsTrue(!profiles.Contains("TestProfile1"), "Profile still Exist after deletion");
	}

    [Test]
    public void ProfileListing()
    {
        
        ProgressionManager.DeleteProfile("TestProfile1");
        ProgressionManager.DeleteProfile("TestProfile2");
        ProgressionManager.DeleteProfile("TestProfile3");

        string[] profiles;

        //Create multiple Profiles
        ProgressionManager.CreateNewProfile("TestProfile1");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile2");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile3");
        ProgressionManager.SaveProfile();

        profiles = ProgressionManager.GetSavedProfiles();
        UnitAssert.AreEqual(3, profiles.Length);
        UnitAssert.Contains("TestProfile1", profiles);
        UnitAssert.Contains("TestProfile2", profiles);
        UnitAssert.Contains("TestProfile3", profiles);

        //Deletion
        ProgressionManager.DeleteProfile("TestProfile1");
        ProgressionManager.DeleteProfile("TestProfile2");
        ProgressionManager.DeleteProfile("TestProfile3");
        profiles = ProgressionManager.GetSavedProfiles();

        UnitAssert.IsTrue(!profiles.Contains("TestProfile1"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile2"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile3"), "Profile still Exist after deletion");
    }

    [Test]
    public void ProfilePersistance()
    {
        string[] profiles;


        //Create multiple Profiles
        ProgressionManager.CreateNewProfile("TestProfile1");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel1");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel2");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel3");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile2");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel4");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel5");
        ProgressionManager.CurrentProfile.OverrideLevelCleared("UnitTestLevel6");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile3");
        ProgressionManager.SaveProfile();

        ProgressionManager.LoadProfile("TestProfile1");
        UnitAssert.AreEqual("TestProfile1", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel1"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel2"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel3"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel4"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel5"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel6"));
        ProgressionManager.LoadProfile("TestProfile2");
        UnitAssert.AreEqual("TestProfile2", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel1"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel2"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel3"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel4"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel5"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel6"));
        ProgressionManager.LoadProfile("TestProfile3");
        UnitAssert.AreEqual("TestProfile3", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel1"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel2"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel3"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel4"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel5"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelCleared("UnitTestLevel6"));

        //Deletion
        ProgressionManager.DeleteProfile("TestProfile1");
        ProgressionManager.DeleteProfile("TestProfile2");
        ProgressionManager.DeleteProfile("TestProfile3");
        profiles = ProgressionManager.GetSavedProfiles();

        UnitAssert.IsTrue(!profiles.Contains("TestProfile1"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile2"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile3"), "Profile still Exist after deletion");
    }

    [Test]
    public void LevelUnlockTester()
    {

        string[] profiles;


        //Create multiple Profiles
        ProgressionManager.CreateNewProfile("TestProfile1");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel1");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel2");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel3");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile2");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel4");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel5");
        ProgressionManager.CurrentProfile.SetLevelUnlocked("UnitTestLevel6");
        ProgressionManager.SaveProfile();
        ProgressionManager.CreateNewProfile("TestProfile3");
        ProgressionManager.SaveProfile();


        ProgressionManager.LoadProfile("TestProfile1");
        UnitAssert.AreEqual("TestProfile1", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel1"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel2"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel3"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel4"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel5"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel6"));
        ProgressionManager.LoadProfile("TestProfile2");
        UnitAssert.AreEqual("TestProfile2", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel1"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel2"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel3"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel4"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel5"));
        UnitAssert.IsTrue(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel6"));
        ProgressionManager.LoadProfile("TestProfile3");
        UnitAssert.AreEqual("TestProfile3", ProgressionManager.CurrentProfile.profileName);
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel1"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel2"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel3"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel4"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel5"));
        UnitAssert.IsFalse(ProgressionManager.CurrentProfile.IsLevelUnlocked("UnitTestLevel6"));


        //Deletion
        ProgressionManager.DeleteProfile("TestProfile1");
        ProgressionManager.DeleteProfile("TestProfile2");
        ProgressionManager.DeleteProfile("TestProfile3");
        profiles = ProgressionManager.GetSavedProfiles();

        UnitAssert.IsTrue(!profiles.Contains("TestProfile1"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile2"), "Profile still Exist after deletion");
        UnitAssert.IsTrue(!profiles.Contains("TestProfile3"), "Profile still Exist after deletion");
    }
}
