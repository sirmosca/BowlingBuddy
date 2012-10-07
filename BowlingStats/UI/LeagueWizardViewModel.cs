using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    public class LeagueWizardViewModel : PropertyChangedBase
    {
        public object CurrentView
        {
            get { return new ConfigurePlayerTeamView(); }
        }
    }
}
