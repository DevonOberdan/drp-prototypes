using UnityEngine;

public class CustomYieldInstructions
{
    public class WaitForUnpausedSeconds : CustomYieldInstruction
    {
        private readonly float seconds;
        private float elapsedTime;

        public WaitForUnpausedSeconds(float seconds)
        {
            this.seconds = seconds;
            elapsedTime = 0f;
        }

        public override bool keepWaiting 
        {
            get 
            {
                if (!PauseManager.PauseState)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                }

                return elapsedTime < seconds;
            }
        }
    }

}
