/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          CollisionManager
 *   
 *   Description:    Manual collision detection for players
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace PAPIOnline
{
    public class CollisionManager
    {

        private List<IPlayer> players = new List<IPlayer>();
        private Vector2 wallExtentMin;
        private Vector2 wallExtentMax;
        private float playerRadius;

        public CollisionManager(Vector2 wallExtentMin, Vector2 wallExtentMax, float playerRadius)
        {
            this.wallExtentMin = wallExtentMin;
            this.wallExtentMax = wallExtentMax;
            this.playerRadius = playerRadius;
        }

        public CollisionManager RegisterPlayer(IPlayer player)
        {
            players.Add(player);
            return this;
        }

        public bool WillPlayerCollide(IPlayer player, Vector3 direction)
        {
            return CheckCollisionWithOtherPlayers(player, direction) || CheckCollisionWithWalls(player, direction);
        }

        private bool CheckCollisionWithOtherPlayers(IPlayer player, Vector3 direction)
        {
            foreach (IPlayer otherPlayer in players)
            {
                if (otherPlayer == player)
                    continue;

                // Prevent stucking when the agent tries to get away
                if (Vector3.Dot((otherPlayer.GetPosition() - player.GetPosition()), direction) < 0)
                    continue;

                float totalRadiusSquare = (2 * playerRadius) * (2 * playerRadius);

                // Check if the sphere colliders collide
                if (Vector3.SqrMagnitude(player.GetPosition() - otherPlayer.GetPosition()) < totalRadiusSquare)
                    return true;
            }

            return false;
        }

        private bool CheckCollisionWithWalls(IPlayer player, Vector3 direction)
        {

            Vector3 nextPosition = player.GetPosition() + direction;

            //All second conditions: Prevent stucking when the agent tries to get away

            if (nextPosition.x + playerRadius > wallExtentMax.x && direction.x > 0)
            {
                return true;
            }

            if (nextPosition.x - playerRadius < wallExtentMin.x && direction.x < 0)
            {
                return true;
            }

            if (nextPosition.z + playerRadius > wallExtentMax.y && direction.z > 0)
            {
                return true;
            }

            if (nextPosition.z - playerRadius < wallExtentMin.y && direction.z < 0)
            {
                return true;
            }

            return false;
        }
    }
}
