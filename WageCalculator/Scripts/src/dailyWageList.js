import React from "react";
import DailyWage from "./dailyWage";

export default React.createClass({
    render: function() {
        var dailyWageKey = 0;
        var dailyWages = this.props.dailyWages.map(function(dailyWage) {
            dailyWageKey++;
        return (
           <DailyWage dailyWage = {dailyWage} key={dailyWageKey}/>
        );
    });
return (
    <div className="daily-wage-list">
            <table>
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Working Shifts</th>
                    <th>Total wage / day</th>
                    <th>Evening compensation / day</th>
                    <th>Overtime compensation / day</th>
                </tr>
             </thead>
             <tbody>
                    {dailyWages}
       
            </tbody>
            </table>
    </div>
    );
}
});

