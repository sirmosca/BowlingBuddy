using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using BowlingStats.ViewModel;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureScheduleViewModel : PropertyChangedBase
    {
        private ObservableCollection<Team> _teams;
        private ObservableCollection<ScheduleItemViewModel> _viewModels;
        private ObservableCollection<Week> _weeks;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureScheduleViewModel"/> class.
        /// </summary>
        /// <param name="matchups">The matchups.</param>
        /// <param name="weeks">The weeks.</param>
        /// <param name="teams">The teams.</param>
        public ConfigureScheduleViewModel(IEnumerable<WeeklyMatchup> matchups, List<Week> weeks, List<Team> teams)
        {
            _weeks = new ObservableCollection<Week>(weeks);
            _teams = new ObservableCollection<Team>(teams);
            _viewModels = new ObservableCollection<ScheduleItemViewModel>();
            _viewModels.CollectionChanged += CollectionChanged;
            Initialize(matchups);
        }

        /// <summary>
        /// Gets or sets the schedule item view models.
        /// </summary>
        /// <value>
        /// The schedule item view models.
        /// </value>
        public ObservableCollection<ScheduleItemViewModel> ScheduleItemViewModels
        {
            get { return _viewModels; }
            set
            {
                _viewModels = value;
                NotifyOfPropertyChange(() => ScheduleItemViewModels);
            }
        }

        /// <summary>
        /// Determines whether this instance [can save schedule].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save schedule]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSaveSchedule()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can print schedule].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can print schedule]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrintSchedule()
        {
            return ScheduleItemViewModels.Count > 0;
        }

        /// <summary>
        /// Prints the schedule.
        /// </summary>
        public void PrintSchedule()
        {
            List<WeeklyMatchup> matchups = GetWeeklyMatchups();
            new PrintScheduleReport().Print(matchups);
        }

        /// <summary>
        /// Saves the schedule.
        /// </summary>
        public void SaveSchedule()
        {
            List<WeeklyMatchup> matchups = GetWeeklyMatchups();
            EventAggregationProvider.EventAggregator.Publish(matchups);
        }

        private List<WeeklyMatchup> GetWeeklyMatchups()
        {
            return ScheduleItemViewModels.Select(
                scheduleItemViewModel =>
                new WeeklyMatchup
                    { Week = scheduleItemViewModel.SelectedWeek, TeamOne = scheduleItemViewModel.SelectedTeamOne, TeamTwo = scheduleItemViewModel.SelectedTeamTwo }).ToList();
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ScheduleItemViewModel model in from object newItem in e.NewItems select newItem as ScheduleItemViewModel)
            {
                model.Teams = _teams;
                model.Weeks = _weeks;
            }
        }

        private void Initialize(IEnumerable<WeeklyMatchup> matchups)
        {
            foreach (ScheduleItemViewModel vm in
                matchups.Select(
                    weeklyMatchup =>
                    new ScheduleItemViewModel
                        {
                            TeamOneName = weeklyMatchup.TeamOne.Name,
                            TeamTwoName = weeklyMatchup.TeamTwo.Name,
                            WeekName = weeklyMatchup.Week.Description,
                            SelectedTeamOne = weeklyMatchup.TeamOne,
                            SelectedTeamTwo = weeklyMatchup.TeamTwo,
                            SelectedWeek = weeklyMatchup.Week
                        }))
            {
                ScheduleItemViewModels.Add(vm);
            }
        }
    }
}