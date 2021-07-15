using System;
using Uchu.World;
using Uchu.World.Scripting.Native;

namespace Uchu.StandardScripts.Base
{
    /// <summary>
    /// Native implementation of scripts/ai/act/l_act_generic_activity_mgr.lua
    /// </summary>
    public class GenericActivityManager : ObjectScript
    {
        /// <summary>
        /// Scripted activity component of the object.
        /// </summary>
        private ScriptedActivityComponent _scriptedActivityComponent;
        
        /// <summary>
        /// Creates the object script.
        /// </summary>
        /// <param name="gameObject">Game object to control with the script.</param>
        public GenericActivityManager(GameObject gameObject) : base(gameObject)
        {
            this._scriptedActivityComponent = gameObject.GetComponent<ScriptedActivityComponent>();
        }

        /// <summary>
        /// Sets up the activity.
        /// </summary>
        /// <param name="maxPlayers">Max players of the activity.</param>
        public void SetupActivity(int maxPlayers)
        {
            // TODO: self:SetActivityParams{ modifyMaxUsers = true, maxUsers = nMaxUsers, modifyActivityActive = true,  activityActive = true}
        }
        
        /// <summary>
        /// Returns if the player is in the activity.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Whether the player is in the activity.</returns>
        public bool IsPlayerInActivity(Player player)
        {
            return this._scriptedActivityComponent.Participants.Contains(player);
        }

        /// <summary>
        /// Updates the player for the activity.
        /// </summary>
        /// <param name="player">Player to update.</param>
        /// <param name="removePlayer"></param>
        public void UpdatePlayer(Player player, bool removePlayer = false)
        {
            if (removePlayer)
            {
                // Remove the player.
                this.RemoveActivityUser(player);
            }
            else
            {
                // Add the player.
                this.AddActivityUser(player);
                this.InitialActivityScore(player, 0);
            }
        }

        /// <summary>
        /// Sets the initial activity score for a player.
        /// </summary>
        /// <param name="player">Player to set.</param>
        /// <param name="score">Score to set.</param>
        public void InitialActivityScore(Player player, float score)
        {
            this.SetActivityUserData(player, 0, score);
        }

        /// <summary>
        /// Adds to the activity value of a player.
        /// </summary>
        /// <param name="player">Player to add to.</param>
        /// <param name="index">Index to change.</param>
        /// <param name="value">Value to add.</param>
        public void UpdateActivityValue(Player player, int index, float value)
        {
            var newValue = this.GetActivityUserData(player, index) + value;
            this.SetActivityUserData(player, index, newValue);
        }
        
        /// <summary>
        /// Sets the activity value of a player.
        /// </summary>
        /// <param name="player">Player to set.</param>
        /// <param name="index">Index to change.</param>
        /// <param name="value">Value to set.</param>
        public void SetActivityValue(Player player, int index, float value)
        {
            this.SetActivityUserData(player, index, value);
        }
        
        /// <summary>
        /// Gets the activity value of a player.
        /// </summary>
        /// <param name="player">Player to get.</param>
        /// <param name="index">Index to get.</param>
        /// <returns>The value of the player.</returns>
        public float GetActivityValue(Player player, int index)
        {
            return this.GetActivityUserData(player, index);
        }
        
        /// <summary>
        /// Stops the activity for a player.
        /// </summary>
        /// TODO: Add parameter names
        public void StopActivity(Player player, float score, float value1 = -1, float value2 = -1, bool quit = false)
        {
            if (quit)
            {
                // Remove the player from the activity.
                this.RemoveActivityUser(player);
            }
            else
            {
                // Set the values.
                this.SetActivityUserData(player, 0, score);
                if (value1 >= 0)
                {
                    this.SetActivityUserData(player, 1, value1);
                }
                if (value2 >= 0)
                {
                    this.SetActivityUserData(player, 2, value2);
                }
                
                // Distribute the rewards.
                // TODO: self:DistributeActivityRewards{ userID = player, bAutoAddCurrency = true, bAutoAddItems = true }
                
                // Update the leaderboard.
                // TODO: self:UpdateActivityLeaderboard{ userID = player }
                
                // Remove the player from the activity.
                this.RemoveActivityUser(player);
                
                /* TODO
                 local actID = self:GetActivityID().activityID
                -- get the leaderboard data for the user and update summary screen if it exists
                player:RequestActivitySummaryLeaderboardData{target = self, queryType = 1, gameID = actID }
                self:NotifyClientObject{name = "ToggleLeaderBoard", param1 = actID, paramObj = player , rerouteID = player}
                 */
                
                // Remove the player from the activity.
                this.RemoveActivityUser(player);
            }
        }

        public void GetLeaderboardData(Player player, int activityId, int numberOfResults)
        {
            // TODO: Implement.
        }

        /// <summary>
        /// Starts a timer for the activity.
        /// </summary>
        /// <param name="timerName">Name of the timer to start.</param>
        /// <param name="updateTime">Interval to update the timer while running.</param>
        /// <param name="stopTime">Time to stop the timer.</param>
        public void ActivityTimerStart(string timerName, int updateTime, int stopTime = 0)
        {
            // TODO: Implement.
        }

        /// <summary>
        /// Resets a timer for the activity.
        /// </summary>
        /// <param name="timerName">Name of the timer to reset.</param>
        public void ActivityTimerReset(string timerName)
        {
            // TODO: Implement.
        }
        
        /// <summary>
        /// Stops a timer for the activity.
        /// </summary>
        /// <param name="timerName">Name of the timer to stop.</param>
        public void ActivityTimerStop(string timerName)
        {
            // TODO: Implement.
        }

        /// <summary>
        /// Stops all timers for the activity.
        /// </summary>
        public void ActivityTimerStopAllTimers()
        {
            // TODO: Implement.
        }

        /// <summary>
        /// Adds time to a timer for the activity.
        /// </summary>
        /// <param name="timerName">Name of the timer to add to.</param>
        /// <param name="addTime">Time to add.</param>
        public void ActivityTimerAddTime(string timerName, int addTime)
        {
            // TODO: Implement.
        }

        /// <summary>
        /// Returns the remaining time for a timer.
        /// </summary>
        /// <param name="timerName">Name of the timer to fetch.</param>
        /// <returns>The remaining time for a timer.</returns>
        public float ActivityTimerGetRemainingTime(string timerName)
        {
            return 0; // TODO: Implement.
        }

        /// <summary>
        /// Returns the elapsed time for a timer.
        /// </summary>
        /// <param name="timerName">Name of the timer to fetch.</param>
        /// <returns>The elapsed time for a timer.</returns>
        public float ActivityTimerGetCurrentTime(string timerName)
        {
            return 0; // TODO: Implement.
        }
    }
}