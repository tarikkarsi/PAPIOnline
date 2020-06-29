/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          BattleArena
 *   
 *   Description:    Arena that contains players and reward texts
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public class BattleArena : MonoBehaviour
	{
		[HideInInspector]
		public static int WIDTH = 50;
		public static int HALF_WIDTH = WIDTH / 2;
		public static int HEIGHT = 50;
		public static int HALF_HEIGHT = HEIGHT / 2;
		public static int MAX_DISTANCE = 70; // near hypotenus

		public static float AGENT_Y = 0.5f;
		public static float AGENT_RADIUS = 0.7f;
		public const string BLUE_AGENT_TAG = "blueAgent";
		public const string BLUE_INFO_TAG = "blueInfo";
		public const string RED_AGENT_TAG = "redAgent";
		public const string RED_INFO_TAG = "redInfo";

		private PlayerAgent blueAgent;
		private PlayerAgent redAgent;
		private BattleInfo blueBattleInfo;
		private BattleInfo redBattleInfo;

		private CollisionManager collisionManager;

		private Stack<Vector3> agentPositions = new Stack<Vector3>(2);
		

		public void Awake()
		{
			SetComponents();
			SetCollisionManager();
			SetAgentPositions();
		}

		private void SetComponents()
		{
			Transform t = this.transform;
			foreach (Transform tr in t)
			{
				switch (tr.tag)
				{
					case BLUE_AGENT_TAG:
						this.blueAgent = tr.GetComponent<PlayerAgent>();
						break;
					case RED_AGENT_TAG:
						this.redAgent = tr.GetComponent<PlayerAgent>();
						break;
					case BLUE_INFO_TAG:
						this.blueBattleInfo = tr.GetComponent<BattleInfo>();
						break;
					case RED_INFO_TAG:
						this.redBattleInfo = tr.GetComponent<BattleInfo>();
						break;
				}
			}
		}

		private void SetCollisionManager()
		{
			Vector2 wallExtentMin = new Vector2(this.transform.position.x - HALF_WIDTH, this.transform.position.z - HALF_HEIGHT);
			Vector2 wallExtentMax = new Vector2(this.transform.position.x + HALF_WIDTH, this.transform.position.z + HALF_HEIGHT);
			this.collisionManager = new CollisionManager(wallExtentMin, wallExtentMax, AGENT_RADIUS);
		}

		private void SetAgentPositions()
		{
			// Set positions of two agents
			// Select a random corner
			int cornerX = Random.Range(0, 2) == 0 ? 1 : -1;
			int cornerY = Random.Range(0, 2) == 0 ? 1 : -1;
			Vector3 agent1Pos = new Vector3(cornerX * (HALF_WIDTH - 5), AGENT_Y, cornerY * (HALF_HEIGHT - 5));
			// Position agents opposite corners
			Vector3 agent2Pos = new Vector3(-agent1Pos.x, AGENT_Y, -agent1Pos.z);
			// Align positions with arena position
			this.agentPositions.Push(agent1Pos + this.transform.position);
			this.agentPositions.Push(agent2Pos + this.transform.position);
		}

		public Vector3 GetNextAgentPosition()
		{
			// Add new positions if empty
			if (this.agentPositions.Count == 0)
			{
				this.SetAgentPositions();
			}
			return this.agentPositions.Pop();
		}

		public PlayerAgent GetRival(string agentTag)
		{
			return agentTag.Equals(BLUE_AGENT_TAG) ? redAgent : blueAgent;
		}

		public BattleInfo GetBattleInfo(string agentTag)
		{
			return agentTag.Equals(BLUE_AGENT_TAG) ? blueBattleInfo : redBattleInfo;
		}

		public CollisionManager GetCollisionManager()
		{
			return this.collisionManager;
		}

		public Vector3 GetPosition()
		{
			return this.transform.position;
		}

		public void PauseGame()
		{
			this.redAgent.PauseAgent();
			this.blueAgent.PauseAgent();
		}

		public void ResumeGame()
		{
			this.redAgent.ResumeAgent();
			this.blueAgent.ResumeAgent();
		}

	}

}