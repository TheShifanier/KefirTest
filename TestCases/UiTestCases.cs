using Assets.UiTest.TestSteps;
using System.Collections.Generic;

namespace Assets.UiTest.Runner
{
    public class UiTestCases : IUiTestCases
    {
        private Dictionary<int, IUiTestCase> _tests = new Dictionary<int, IUiTestCase>();

        public UiTestCases()
        {
            _tests.Add(0, new CheckMapBoundariesTestCase()); // Баг KEFIR-7
            _tests.Add(1, new MoveItemsFromPocketsToAllBackpackCells()); // Баг KEFIR-8
            _tests.Add(2, new CutAllTreesWithAxesTestCase()); // Баг KEFIR-1
            _tests.Add(3, new CutAllTreesWithoutAxesTestCase()); // Баг KEFIR-2
            _tests.Add(4, new CutTreesForWoodCount()); // Баг KEFIR-3
            _tests.Add(5, new CheckAxeCutLifecycleTestCase()); 
            _tests.Add(6, new StackingLimitsTestCase());
            _tests.Add(7, new SawmillProgressTestCase());
            _tests.Add(8, new SawmillSkipProgressTestCase()); // Баг KEFIR-5, KEFIR-6
        }

        public IUiTestCase GetTestCase(int test)
        {
            return _tests[test];
        }
    }
}