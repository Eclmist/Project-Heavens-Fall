using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WindowsInput;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControllerUnitTest : MonoBehaviour
{
	public bool executeUnitTest;
	private bool unitTestRunning;

    private float timeElapsed;
    private float timeout;
    private bool useTimeout;
    private UnitTestPhases unitTestPhase;
	private TestState state;
    private string failureCondition;

	private GameObject testComponentObject;
	private bool testComponentExecutionCheck;

	private string resultString = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!unitTestRunning)
		{
			if (!executeUnitTest) return;
			executeUnitTest = false;
			unitTestRunning = true;
		}

		timeElapsed += Time.deltaTime;
		DebugHelper.WriteDebug(gameObject, string.Format("Unit Test Phase: {0}\n" +
		                                                 "Time Elapsed: {1}\n" +
		                                                 "Timeout: {2}\n" +
		                                                 "Failure Condition: {3}\n" +
		                                                 "State: {4}",
														 unitTestPhase.ToString(),
														 timeElapsed,
														 (!useTimeout? "NA" : timeout.ToString()),
														 failureCondition,
														 state.ToString()), 
														 
											Color.red);


	    switch (unitTestPhase)
	    {
		    case UnitTestPhases.platformFallThroughTest:
				//Arrange
			    if (state == TestState.start)
			    {
				    //Teleport player to location
				    testComponentObject = GameObject.Find("PlatformUnitTestStartPoint");

				    timeout = 20;
				    useTimeout = true;
				    failureCondition = "Player Fall Through Platform";
					Player.Instance.transform.position = testComponentObject.transform.position;
				    state = TestState.running;
					PrintToCanvas(Color.yellow, unitTestPhase.ToString() + "\nRunning...");
			    }

			    //Test
			    if (state == TestState.running)
			    {
				    //Nothing to test player should stand on platform

				    //Assert
				    //Player should stay on platform
				    if (Player.Instance.transform.position.y < 2f)
				    {
					    TestFail("Player Fell Through Platform");
						state = TestState.cleanup;
				    }
				    if (timeElapsed > timeout)
				    {
						TestSuccess();
						state = TestState.cleanup;
					}

				}


			    //Cleanup
				if (state == TestState.cleanup)
				{
					//Teleport to Null zone and wait 2 seconds
					if (!failureCondition.Equals(""))
					{
						DefaultCleanup();
						Player.Instance.transform.position = GameObject.Find("NullZone").transform.position;
					}

					if (timeElapsed > 2)
					{
						state = TestState.start;
						unitTestPhase++;
						DefaultCleanup();
					}
					
				}

			    break;
		    case UnitTestPhases.windzoneTest:
				//Arrange

			    float windHeight = 16.91572f;

				if (state == TestState.start)
				{
					//Teleport player to location
					testComponentObject = GameObject.Find("WindZoneTestStartPoint");

					timeout = 0;
					useTimeout = false;
					failureCondition = "Player flies x (~1) units";
					Player.Instance.transform.position = testComponentObject.transform.position;
					state = TestState.running;
					PrintToCanvas(Color.yellow, unitTestPhase.ToString() + "\nRunning...");

					MetricsTester.Instance.maxHeight = 0;
				}

				//Test
				if (state == TestState.running)
				{
					//Wait for player to reach max height
					if (Player.Instance.transform.position.y < MetricsTester.Instance.maxHeight - 5)
					{
						//Assert
						//Player should stay on platform
						if (MetricsTester.Instance.maxHeight > windHeight + 2f)
							TestFail("Player Flew over x " + windHeight + " + 1 units\n" + MetricsTester.Instance.maxHeight);
						else if (MetricsTester.Instance.maxHeight < windHeight - 2f)
							TestFail("Player fail to fly " + windHeight + " + 1 units\n" + MetricsTester.Instance.maxHeight);
						else
							TestSuccess(MetricsTester.Instance.maxHeight.ToString());

						state = TestState.cleanup;
					}

				}


				//Cleanup
				if (state == TestState.cleanup)
				{
					//Teleport to Null zone and wait 2 seconds
					if (!failureCondition.Equals(""))
					{
						DefaultCleanup();
						Player.Instance.transform.position = GameObject.Find("NullZone").transform.position;
					}

					if (timeElapsed > 2)
					{
						state = TestState.start;
						unitTestPhase++;
						DefaultCleanup();
					}
				}
				break;
		    case UnitTestPhases.jumpTest:

				float jumpHeight = 7.454294f;

				//Arrange
				if (state == TestState.start)
				{
					//Teleport player to location
					testComponentObject = GameObject.Find("JumpTestStartPoint");

					timeout = 0;
					useTimeout = false;
					failureCondition = "Player jumps "+ jumpHeight + " (~2) units";
					Player.Instance.transform.position = testComponentObject.transform.position;
					state = TestState.running;
					PrintToCanvas(Color.yellow, unitTestPhase.ToString() + "\nRunning...");

					MetricsTester.Instance.maxHeight = 0;

				}

				//Test
				if (state == TestState.running)
				{

					if (timeElapsed > 2 && !testComponentExecutionCheck)
					{
						Player.Instance.GetComponent<PlayerControllerA>().jumpping = true;
						testComponentExecutionCheck = true;
					}

					//Wait for player to reach max height
					if (Player.Instance.transform.position.y < MetricsTester.Instance.maxHeight - 5)
					{
						//Assert
						//Player should stay on platform
						if (MetricsTester.Instance.maxHeight > jumpHeight + 2f)
							TestFail("Player jumped over x " + jumpHeight + " + 2 units\n" + MetricsTester.Instance.maxHeight);
						else if (MetricsTester.Instance.maxHeight < jumpHeight - 2f)
							TestFail("Player fail to jump x " + jumpHeight + " + 2 units\n" + MetricsTester.Instance.maxHeight);
						else
							TestSuccess(MetricsTester.Instance.maxHeight.ToString());

						state = TestState.cleanup;
					}

				}


				//Cleanup
				if (state == TestState.cleanup)
				{

				}
				break;
		    default:
			    throw new ArgumentOutOfRangeException();
	    }


		if ((int) unitTestPhase == Enum.GetValues(typeof(UnitTestPhases)).Length - 1 && state == TestState.cleanup)
		{
			DebugHelper.WriteDebug(gameObject,"Unit Test Ended \n \n" + resultString, Color.red);
			unitTestRunning = false;
		}
	}

	private void DefaultCleanup()
	{
		testComponentObject = null;
		timeElapsed = 0;
		timeout = 0;
		useTimeout = false;
		failureCondition = "";
		testComponentExecutionCheck = false;
	}

	private void TestFail(string message = "")
	{
		string testName = unitTestPhase.ToString();
		string print = string.Format("{0}\n{1}\n{2}", testName, "Result: Failure", message);
		resultString += Helper.ColorString(Color.red, print) + "\n\n";
		PrintToCanvas(Color.red, print);
	}

	private void TestSuccess(string message = "")
	{
		string testName = unitTestPhase.ToString();
		string print = string.Format("{0}\n{1}\n{2}", testName, "Result: Success", message);
		resultString += Helper.ColorString(Color.green, print) + "\n\n";
		PrintToCanvas(Color.green, print);
	}

	private void PrintToCanvas(Color color, string str)
	{
		testComponentObject.GetComponentInChildren<Text>().text = Helper.ColorString(color, str);
	}

    private enum UnitTestPhases
    {
        platformFallThroughTest,
		windzoneTest,
		jumpTest
    }

	private enum TestState
	{
		start,
		running,
		cleanup
	}
}
