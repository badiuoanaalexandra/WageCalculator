import React from "react";
import ReactDOM from "react-dom";
import IntervalWage from "./intervalWage";

export default React.createClass({
    componentDidUpdate:function() {
        this.hideIntervalWage();
    },
    showIntervalWage:function() {
        if (this.refs.intervalWage.innerHTML === "") {
            ReactDOM.render(
                <IntervalWage intervalWage={this.props.person.IntervalWage} />,
                this.refs.intervalWage
            );
        } else {
            this.refs.intervalWage.style.display = "block";
        }

        this.refs.showDetails.style.display = "none";
        this.refs.hideDetails.style.display = "block";
    },
   hideIntervalWage:function() {
       this.refs.intervalWage.style.display = "none";
        this.refs.showDetails.style.display = "block";
        this.refs.hideDetails.style.display = "none";
    },
    render: function() {
        return (
    <div className="person">
            <div className="table">
                <div>
                    {this.props.person.PersonName}
                </div>
                 <div>
                    {this.props.person.IntervalWage.TotalWage} $
                </div>
                <div>
                    <a ref="showDetails" onClick={this.showIntervalWage}>View details</a>
                    <a ref="hideDetails" style={{ display: "none" }} onClick={this.hideIntervalWage}>Hide details</a>
                </div>
            </div>
    <div ref="intervalWage"></div>
     </div>
        );
    }
});
