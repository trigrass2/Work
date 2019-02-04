using GalaSoft.MvvmLight;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Stations.Model
{
    public class DeadLine : ViewModelBase
    {
        

        public DeadLine(string time)
        {
            startDeadLine();
            TotalSecond = int.Parse(time) * 60;
            //_colorTimer = Brushes.Black;
        }

        private DispatcherTimer _dispatcherTimer = null;
        
        private string _timerText;
        public string TimerText
        {
            get
            {
                return _timerText;
            }
            set
            {
                _timerText = value;
                RaisePropertyChanged();
            }
        }

        public int TotalSecond;        

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TotalSecond -= 1;           
            TimerText = string.Format("{1}{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(TotalSecond).Duration(), (TotalSecond < 0) ? "-" : "");           
            
            CommandManager.InvalidateRequerySuggested();
        }

        public void startDeadLine()
        {
            try
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                _dispatcherTimer.Start();
            }
            catch (Exception ex)
            {                
                throw;
            }
            
        }
    }
}
