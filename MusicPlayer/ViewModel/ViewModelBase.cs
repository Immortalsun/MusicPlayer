
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MusicPlayer.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region Fields
        private bool _disposed = false;
        private string _displayName;

        #endregion

        #region Properties
        public virtual string DisplayName
        {
            get { return _displayName; }
            protected set { SetAndNotify(ref _displayName, value); }
        }

        #endregion

        #region Constructor



        #endregion

        #region Debug methods

        /// <summary>
        /// Throw warning if the object does not have a property with the passed name.
        /// Method only exists in DEBUG build
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyNameValid(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;
                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// <para>The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.</para>
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion Debug methods

        #region INotifyPropertyChanged Members
        /// <summary>
        /// This handler is raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This method raised the change event
        /// </summary>
        /// <param name="propertyName">The name of the property with a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyNameValid(propertyName);

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Signals the on property changed event.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        /// <param name="broadcast">Whether to broadcast the notification or not.</param>
        protected virtual void OnPropertyChanged<T>(
            T oldValue = default(T),
            T newValue = default(T),
            bool broadcast = false,
            [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            }

            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// This method allows for passing the ref to a field and a value, which will allow for one line setting and notifying.
        /// <para>This method relies on the CallerMemberName attribute to get the property name,
        ///  so if it used outside of a member, the name must be passed to nofify a change.</para>
        /// </summary>
        /// <typeparam name="T">Type of the Property/Field</typeparam>
        /// <param name="field">Field to set.</param>
        /// <param name="newValue">Value to apply to field.</param>
        /// <param name="broadcast">Whether to broadcast the property change with the MessangerInstance.</param>
        /// <param name="propertyName">Name of property to nofify.</param>
        protected virtual void SetAndNotify<T>(
            ref T field,
            T newValue,
            bool broadcast = false,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return;

            var oldValue = field;
            field = newValue;
            OnPropertyChanged(oldValue, newValue, broadcast, propertyName);
        }
        #endregion INotifyPropertyChanged Members

        #region IDisposable Members
        /// <summary>
        /// Clean up the object, and make it subject to garbage collection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Children can handle their own cleanup with this method.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose of managed resources.

                }

                // dispose of unmanaged resources

                // set _disposed to true.
                _disposed = true;
            }
        }


#if DEBUG
        /// <summary>
        /// When debugging, we can see that objects are garbage collected.
        /// </summary>
        ~ViewModelBase()
        {
            //var message = string.Format("{0} ({1}) ({2}) Finalized"
            //    , this.GetType().Name
            //    , this.DisplayName
            //    , this.GetHashCode());
            //Debug.WriteLine(message);
            Dispose(false);
        }
#endif
        #endregion IDisposable Members
    }
}