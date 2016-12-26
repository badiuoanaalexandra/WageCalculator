import React from "react";
import moment from "moment";
import WorkingShift from "./workingShift";

export default React.createClass({
    render: function() {
        var workingShiftKey = 0;
        var workingShifts = this.props.dailyWage.WorkingDay.WorkingShifts.map(function(workingShift) {
            workingShiftKey++;
            return (
               <WorkingShift key={workingShiftKey} workingShift={workingShift} />
        );
      });
        return (
         <tr>
            <td>
                    {moment(this.props.dailyWage.WorkingDay.Date).format('MMMM Do YYYY')}
            </td>
            <td>
                    {workingShifts}
            </td>
               <td>
                    {this.props.dailyWage.TotalWage} $<br/>
                    {this.props.dailyWage.WorkingHours} h
                </td>
                 <td>
                     {this.props.dailyWage.EveningCompensation} $<br/>
                    {this.props.dailyWage.EveningHours} h
                </td>
                <td>
                    {this.props.dailyWage.OvertimeCompensation} $<br/>
                    {this.props.dailyWage.OvertimeHours} h
                </td>
            </tr>
        );
    }
});
