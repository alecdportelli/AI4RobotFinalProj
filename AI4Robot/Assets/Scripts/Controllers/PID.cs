using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Controllers
{
    public class PID
    {
        private float Kp;
        private float Ki;
        private float Kd;

        private float outputMin;
        private float outputMax;

        private float integral;         // Integral term accumulator
        private float previousError;    // Previous error for derivative calculation
        private float setpoint;         // Desired target value


        public PID(float Kp, float Ki, float Kd, float outputMin, float outputMax)
        {
            this.Kp = Kp;
            this.Ki = Ki;
            this.Kd = Kd;

            this.outputMin = outputMin;
            this.outputMax = outputMax;

            Reset();
        }


        public void SetSetpoint(float setpoint)
        {
            this.setpoint = setpoint;
        }


        public float Update( float input, float deltaTime )
        {
            // Kp
            float error = setpoint - input;
            float proportional = this.Kp * error;

            // Ki
            integral += error * deltaTime;
            float integralTerm = this.Ki * integral;

            // Kd
            float derivative = (error - previousError) / deltaTime;
            float derivativeTerm = this.Kd * derivative;

            // Combine terms 
            float output = proportional + integralTerm + derivativeTerm;

            // Determine output 
            output = Math.Max(outputMin, Math.Min(outputMax, output));

            // Set error to las 
            previousError = error;

            return output;
        }


        public void Reset()
        {
            integral = 0.0f;
            previousError = 0.0f;
        }
    }
}