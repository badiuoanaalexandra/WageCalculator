import React from "react";
import moment from "moment";

export default React.createClass({
    //getInitialState: function () {
    //    return { m: moment()};
    //},
    render: function() {
        var dailyHours = this.props.person.Hours.map(function(dailyHour) {
            return (
                <div className="hour" key={dailyHour.DailyHourId}>
                    <div>{moment(dailyHour.StartDate).format('MMMM Do YYYY, HH:mm')}</div>
                        <div>{moment(dailyHour.EndDate).format('MMMM Do YYYY, HH:mm')}</div>
                 </div>
            );
        });
        return (
            <div className="person">
    <div className="name">
        {this.props.person.PersonName}
    </div>
    {dailyHours}
     </div>
        );
    }
});
