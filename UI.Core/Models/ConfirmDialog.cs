using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Models;

namespace UI.Core.Models
{
    public class ConfirmDialog : INotifyPropertyChanged, IDialog, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private volatile bool _TerminateSignal = false;

        private EventWaitHandle _WaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        private string _Message;

        public string Message
        {
            get { return this._Message; }
            set { this.SetPropertyField(ref this._Message, ref value); }
        }

        private DialogResult _Result;

        public DialogResult Result
        {
            get { return this._Result; }
            set
            {
                this.SetPropertyField(ref this._Result, ref value);
                this._WaitHandle.Set();
            }
        }

        public async Task<DialogResult> AwaitResultAsync()
        {
            await Task.Run(() =>
            {
                while (!this._TerminateSignal && this.Result == DialogResult.None)
                    this._WaitHandle.WaitOne();
            });

            return this.Result;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }

        protected void SetPropertyField<T>(ref T oldValue, ref T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return;

            oldValue = newValue;
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #region IDisposable Support

        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing)
                {
                    this._TerminateSignal = true;
                    this._WaitHandle.Set();
                }

                this._Disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
