using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core.Internal;
using MauiGymApp.Models;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public class LiftGroupViewModel : ObservableCollection<LiftViewModel>, INotifyPropertyChanged
    {
        private bool isExpanded = true;

        public MovementPattern MovementPattern { get; private set; }
        public Color GroupColor { get; private set; }

        public LiftGroupViewModel(MovementPattern pattern, List<LiftViewModel> lifts) : base(lifts)
        {
            MovementPattern = pattern;
            //GroupColor = MovementPatternColors.GetColor(pattern);
            IsExpanded = true;
            
        }

        public string Name => MovementPattern.ToString();

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public static IEnumerable<LiftGroupViewModel> CreateLiftGroups(IEnumerable<LiftViewModel> lifts)
        {
            var temp = new List<LiftViewModel>(lifts);
            var groups = new List<LiftGroupViewModel>();

            foreach (var mp in Enum.GetValues<MovementPattern>())
            {
                var group = new LiftGroupViewModel(mp, new List<LiftViewModel>());
              
                group.InsertRange(0, temp.Where(l => l.MovementPattern == mp));

                if (group.Any())
                    groups.Add(group);
            }
            return groups;
        }
    }
}
