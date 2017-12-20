using System;
using System.Collections.Generic;

namespace Walls.Julian.Log4Net.Extensions
{
    /// <summary>
    /// Utility method which adds the keyValues to the Log4Net logical thread context
    /// and removes them on Dispose.
    /// </summary>
    public class AdditionalLogicalThreadContext : IDisposable
    {
        private readonly IDictionary<string, object> _keyValues;
        private bool disposed = false;

        public AdditionalLogicalThreadContext(IDictionary<string, object> keyValues)
        {
            _keyValues = keyValues ?? throw new ArgumentNullException(nameof(keyValues));
            
            foreach (var keyValue in _keyValues)
            {
                log4net.LogicalThreadContext.Properties[keyValue.Key] = keyValue.Value;
            }          
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var keyValue in _keyValues)
                {
                    log4net.LogicalThreadContext.Properties.Remove(keyValue.Key);
                }
            }
            disposed = true;
        }
    }
}
