import React from "react";
import ReactDOM from "react-dom";
import MonthlyWage from "./monthlyWage";

export default React.createClass({
    showMonthlyWage:function() {
        if (this.refs.monthlyWage.innerHTML === "") {
            ReactDOM.render(
                <MonthlyWage monthlyWage={this.props.person.MonthlyWage} />,
                this.refs.monthlyWage
            );
        } else {
            this.refs.monthlyWage.style.display = "block";
        }

        this.refs.showDetails.style.display = "none";
        this.refs.hideDetails.style.display = "block";
    },
   hideMonthlyWage:function() {
        this.refs.monthlyWage.style.display = "none";
        this.refs.showDetails.style.display = "block";
        this.refs.hideDetails.style.display = "none";
    },
    render: function() {
        return (
    <div className="person">
            <div className="table">
                <div className="name">
                    {this.props.person.PersonName}
                </div>
                 <div className="name">
                    {this.props.person.MonthlyWage.TotalMonthlyWage} $
                </div>
                <div>
                    <a ref="showDetails" onClick={this.showMonthlyWage}>View details</a>
                    <a ref="hideDetails" style={{ display: "none" }} onClick={this.hideMonthlyWage}>Hide details</a>
                </div>
            </div>
    <div ref="monthlyWage"></div>
     </div>
        );
    }
});
