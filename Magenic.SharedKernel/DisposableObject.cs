using System;
using System.Diagnostics;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Generic base class for object implementing IDisposable. Provides best practices 
    /// for object disposal, including finalization.
    /// </summary>
    /// <remarks>Override DisposeManagedResources for your disposal code.</remarks>
    public abstract class DisposableObject : IDisposable
    {
        #region Properties

        /// <summary>
        /// Indicates if this object has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Override this method to dispose managed resources 
        /// (normal CLR garbage collected objects).
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Override this method to dispose any unmanaged resources such as COM allocated
        /// objects, legacy 3rd party, etc. Normally this isn't needed.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disposes this object and suppresses finalization.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes this object; dispose managed resources is skipped in this case.
        /// </summary>
        ~DisposableObject()
        {
            if (!Disposed)
            {
                Debug.WriteLine($"WARNING: Object {GetType().FullName} finalized without being disposed!");
            }

            Dispose(false);
        }

        #endregion


        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();
            Disposed = true;
        }

        #endregion
    }
}
