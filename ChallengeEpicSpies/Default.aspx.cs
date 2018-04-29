using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChallengeEpicSpies
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Do this if this is the first call
            if (!Page.IsPostBack) 
            {
                previousEndCalendar.SelectedDate = DateTime.Now.Date;
                newStartCalendar.SelectedDate = DateTime.Now.Date.AddDays(14);
                newStartCalendar.VisibleDate = newStartCalendar.SelectedDate;
                newEndCalendar.SelectedDate = DateTime.Now.Date.AddDays(21);
                newEndCalendar.VisibleDate = newEndCalendar.SelectedDate;
                resultLabel.Text = "";
            }

            // to aoid jumping up after each selection (page post back)
            Page.MaintainScrollPositionOnPostBack = true;
        }

        protected void assignButton_Click(object sender, EventArgs e)
        {
            var prevEndDate = previousEndCalendar.SelectedDate;
            var newStartDate = newStartCalendar.SelectedDate;
            var newEndDate = newEndCalendar.SelectedDate;

            string result;

            if (CheckRestTime(prevEndDate, newStartDate) && CheckEndDate(newStartDate, newEndDate))
            {
                TimeSpan newAssignmentDuration = newEndDate.Subtract(newStartDate).Duration();

                int costPerDay = 500;
                int assignmentCost = newAssignmentDuration.Days * costPerDay;
                assignmentCost += (newAssignmentDuration.Days > 21) ? 1000 : 0;

                result = string.Format("New Assignment '{0}' was assigned to the spy '{1}'." +
                    "<br />Total project cost: {2:c}" +
                    "<br />Total number of assignment: {3} days",
                    newAssignmentTextBox.Text, spyCodeNameTextBox.Text,
                    assignmentCost,
                    newAssignmentDuration.Days
                    );
            }
            else
            {
                result = "Error! Either of following happened: " +
                         "<br /> You must allow at least 2 weeks between assignments." + 
                         "<br /> The assignment end date must be after the assignment start date.";
            }

            resultLabel.Text = result;        
        }

        // it returns true if the newEndDate is greater than newStartDate
        // if false, it sets the newEndCalender to one day after newStartDate
        private bool CheckEndDate(DateTime newStartDate, DateTime newEndDate)
        {
            bool output = false;

            if (newEndDate > newStartDate)
            {
                output = true;
            }
            else
            {
                output = false;
                newEndCalendar.SelectedDate = newStartDate.AddDays(1);
                newEndCalendar.VisibleDate = newEndCalendar.SelectedDate;          
            }

            return output;
        }

        // returns true if the rest time is at least 13 days
        // if not, it sets the newStartCalender to 14 days after the last end job
        private bool CheckRestTime(DateTime prevEndDate, DateTime newStartDate)
        {
            var restDuration = newStartDate.Subtract(prevEndDate).Duration().Days;
            var output = true;

            if (restDuration <= 13)
            {
                newStartCalendar.SelectedDate =
                    previousEndCalendar.SelectedDate.AddDays(14);
                newStartCalendar.VisibleDate = newStartCalendar.SelectedDate;

                output = false;
            }

            return output;
        }
    }
}