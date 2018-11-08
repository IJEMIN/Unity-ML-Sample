using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

namespace BallPractice {
    public class BallAgent : Agent {
        public Transform platform;
        public Rigidbody ballRigidbody;
        public Transform target;

        public float speed = 10f;

        private bool eatTarget = false;
        private bool dead = false;


        public override void AgentReset() {
            Vector3 randomPos = Random.insideUnitSphere * 4f;
            randomPos.y = 0.5f;

            ballRigidbody.velocity = Vector3.zero;
            transform.position = platform.position + randomPos;

            dead = false;
            ResetTarget();
        }

        public override void CollectObservations() {
            Vector3 distanceToTarget = target.position - transform.position;

            // Relative position
            AddVectorObs(distanceToTarget.x / 5f);
            AddVectorObs(distanceToTarget.z / 5f);

            Vector3 distanceFromPivot = transform.position - platform.position;

            AddVectorObs(distanceFromPivot.x / 5f);
            AddVectorObs(distanceFromPivot.z / 5f);

            // Agent velocity
            AddVectorObs(ballRigidbody.velocity.x / 10f);
            AddVectorObs(ballRigidbody.velocity.z / 10f);
        }


        public override void AgentAction(float[] actions, string textAction) {
            if (eatTarget)
            {
                AddReward(1.0f);
                ResetTarget();
            }
            else if (dead)
            {
                AddReward(-1.0f);
                Done();
            }

            // Time penalty
            AddReward(-0.005f);

            // Actions, size = 2
            Vector3 velocity = new Vector3(actions[0], 0, actions[1]);
            ballRigidbody.AddForce(velocity * speed);

            Monitor.Log($"{name} Reward", GetCumulativeReward(), transform);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("dead"))
            {
                dead = true;
            }
            else if (other.CompareTag("target"))
            {
                eatTarget = true;
            }
        }

        void ResetTarget() {
            Vector3 targetRandomPos = Random.insideUnitSphere * 4f;
            targetRandomPos.y = 0.5f;

            target.position = platform.position + targetRandomPos;
            eatTarget = false;
        }
    }
}