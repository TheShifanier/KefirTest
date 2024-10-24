using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context;
using Assets.UiTest.Results;

namespace Assets.UiTest.TestSteps
{
    public class MoveAcrossBoundariesAndCheckPlayerPostitionStep : UiTestStepBase
    {
        public override string Id => "move_across_boundaries_position";
        public override double TimeOut => 100;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            yield return Commands.PlayerMoveCommand(new UnityEngine.Vector3(35, 0, 35), new ResultData<PlayerMoveResult>());
            yield return Commands.WaitForSecondsCommand(3, new ResultData<SimpleCommandResult>());
            var boundariesPlayerPosition = Cheats.GetPlayerPosition();
            if (boundariesPlayerPosition.y < 0) 
            {
                Fail("Player fall under the scene textures");
            }
            if (boundariesPlayerPosition.Equals(new UnityEngine.Vector3(35, 0, 35)))
            {
                Fail("Player should not cross game zone boundaries and stay in it");
            }

            yield return Commands.PlayerMoveCommand(new UnityEngine.Vector3(-35, 0, -35), new ResultData<PlayerMoveResult>());
            yield return Commands.WaitForSecondsCommand(3, new ResultData<SimpleCommandResult>());
            boundariesPlayerPosition = Cheats.GetPlayerPosition();
            if (boundariesPlayerPosition.y < 0)
            {
                Fail("Player fall under the scene textures");
            }
            if (boundariesPlayerPosition.Equals(new UnityEngine.Vector3(-35, 0, -35)))
            {
                Fail("Player should not cross game zone boundaries and stay in it");
            }
        }
    }
}