using System;
using System.Diagnostics;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Base class for classes implementing IDisposable. Provides best practices 
    /// for object disposal including finalization.
    /// </summary>
    /// <remarks>Override DisposeManagedResources for your disposal code.</remarks>
    public abstract class DisposableObject : IDisposable
    {
        #region Finalizers

        /// <summary>
        /// Finalizes this object if dispose managed resources was skipped.
        /// </summary>
        ~DisposableObject()
        {
            if (!Disposed)
            {
                Debug.WriteLine(
                    $"WARNING: object {GetType().FullName} finalized without being disposed!");
            }

            Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if this object has been disposed.
        /// </summary>
        public bool Disposed { get; private set; }

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

        #region Private Methods

        /// <summary>
        /// Internal Dispose method in charge of executing resource cleanup methods.
        /// </summary>
        /// <param name="disposing">True indicates call is being made from Dispose method.</param>
        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
                Disposed = true;
            }
        }

        #endregion
    }
}
