using System;

namespace PDD.DataManagement
{
    public class DeadlinessInfo
    {
        public enum Deadly
        {
            Safe,
            Deadly
        }

        public DeadlinessInfo(Deadly isDeadly, string message)
        {
            IsDeadly = isDeadly;
            Message = message;
        }

        public Deadly IsDeadly { get; set; }
        public string Message { get; set; }
        public bool IsDeadlyBool => IsDeadly == Deadly.Deadly;
        public bool IsSafeBool => IsDeadly == Deadly.Safe;
    }
}