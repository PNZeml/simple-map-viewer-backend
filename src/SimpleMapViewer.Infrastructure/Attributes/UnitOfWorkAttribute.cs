using System;
using System.Data;

namespace SimpleMapViewer.Infrastructure.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class UnitOfWorkRequiredAttribute : Attribute {
        public IsolationLevel IsolationLevel { get; }

        public UnitOfWorkRequiredAttribute(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
        ) {
            IsolationLevel = isolationLevel;
        }
    }
}