using MLAgents;

namespace BallPractice {
    public class BallAcademy : Academy {
        public override void AcademyReset() {
            Monitor.SetActive(true);
        }
    }
}