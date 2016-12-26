﻿import React from "react";
import ReactDOM from "react-dom";
import DailyWageList from "./dailyWageList";

export default React.createClass({
    showDailyWages:function() {
        if (this.refs.dailyWages.innerHTML === "") {
        ReactDOM.render(
          <DailyWageList dailyWages={this.props.monthlyWage.DailyWages} />,
          this.refs.dailyWages
         );
        } else {
            this.refs.dailyWages.style.display = "block";
        }

        this.refs.linkShowDailyWages.style.display = "none";
        this.refs.linkHideDailyWages.style.display = "block";
    },
    hideDailyWages:function() {
        this.refs.dailyWages.style.display = "none";
        this.refs.linkShowDailyWages.style.display = "block";
        this.refs.linkHideDailyWages.style.display = "none";
    },
    render: function() {
        return (
            <div className="monthly-wage">
                <div className="table-titles">
                     <div>Total wage</div>
                     <div>Evening compensation</div>
                     <div>Overtime compensation</div>
                </div>
                <div className="table">
                    <div>
                        {this.props.monthlyWage.TotalMonthlyWage} $<br/>{this.props.monthlyWage.TotalWorktimeHours} h
                    </div>
                    <div>
                        {this.props.monthlyWage.TotalEveningCompensation} $<br/>{this.props.monthlyWage.TotalEveningHours} h
                    </div>
                     <div>
                        {this.props.monthlyWage.TotalOvertimeCompensation} $<br/>{this.props.monthlyWage.TotalOvertimeHours} h
                    </div>
                    <div>
                        <a ref="linkShowDailyWages" onClick={this.showDailyWages}>View amounts/day</a>
                        <a ref="linkHideDailyWages" style={{ display: "none" }} onClick={this.hideDailyWages}>Hide amounts/day</a>
                    </div>
                </div>
                <div ref="dailyWages"></div>
            </div>
        );
    }
});
