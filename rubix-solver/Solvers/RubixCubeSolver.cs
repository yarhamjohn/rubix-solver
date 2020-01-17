namespace rubix_solver.Solvers
{
    public class RubixCubeSolver
    {
        private readonly RubixCube _cube;

        public RubixCubeSolver(RubixCube cube)
        {
            _cube = cube;
        }

        public void Solve()
        {
            SolveFirstLayer();
            SolveSecondLayer();
            SolveThirdLayer();
        }

        private void SolveFirstLayer()
        {
            var solver = new FirstLayerSolver(_cube);
            solver.Solve();
        }

        private void SolveSecondLayer()
        {
            var solver = new SecondLayerSolver(_cube);
            solver.Solve();
        }

        private void SolveThirdLayer()
        {
            var solver = new ThirdLayerSolver(_cube);
            solver.Solve();
        }
    }
}
